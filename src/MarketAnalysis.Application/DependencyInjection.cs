namespace MarketAnalysis.Application;

using FluentValidation;
using MediatR;
using MarketAnalysis.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

/// <summary>
/// Dependency injection configuration for Application layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Register AutoMapper
        services.AddAutoMapper(assembly);

        // Register FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Register pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
