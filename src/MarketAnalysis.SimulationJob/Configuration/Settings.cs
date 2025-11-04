namespace MarketAnalysis.SimulationJob.Configuration;

public class SimulationSettings
{
    public int IntervalSeconds { get; set; } = 5;
    public int InitialDelaySeconds { get; set; } = 10;
    public double MinPriceChangePercent { get; set; } = -2.0;
    public double MaxPriceChangePercent { get; set; } = 2.0;
    public int MinVolume { get; set; } = 100000;
    public int MaxVolume { get; set; } = 10000000;
    public bool EnableBatchMode { get; set; } = true;
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = "http://localhost:5000";
    public int TimeoutSeconds { get; set; } = 30;
}
