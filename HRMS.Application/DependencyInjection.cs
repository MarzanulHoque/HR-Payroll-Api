using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all application-layer dependencies (like MediatR and AutoMapper).
    /// Called from the main API layer's Program.cs.
    /// </summary>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Registers MediatR and automatically scans this assembly for all query/command handlers.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // Example: services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
