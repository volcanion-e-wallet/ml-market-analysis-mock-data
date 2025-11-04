using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using MarketAnalysis.SimulationJob.Services;
using MarketAnalysis.SimulationJob.Configuration;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Market Analysis Simulation Job");

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();
        })
        .ConfigureServices((context, services) =>
        {
            // Bind configuration
            services.Configure<SimulationSettings>(context.Configuration.GetSection("SimulationSettings"));
            services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));

            // Configure HttpClient
            services.AddHttpClient<IMarketApiClient, MarketApiClient>();

            // Register services
            services.AddSingleton<IStockDataProvider, StockDataProvider>();
            services.AddSingleton<IMarketSimulator, MarketSimulator>();
            
            // Register hosted service
            services.AddHostedService<SimulationWorker>();
        })
        .Build();

    await host.RunAsync();
    
    Log.Information("Market Analysis Simulation Job stopped");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Market Analysis Simulation Job terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
