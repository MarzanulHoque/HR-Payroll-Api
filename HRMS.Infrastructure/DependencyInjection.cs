using HRMS.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        // Registers our DbContext implementation against the Application layer's interface
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<Data.ApplicationDbContext>());

        return services;
    }
}
