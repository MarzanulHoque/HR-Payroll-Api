using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        // Registers our DbContext implementation against the Application layer's interface
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<Data.ApplicationDbContext>());

        // Register token service
        services.AddScoped<ITokenService, TokenService>();
        // Register permission service
        services.AddScoped<IPermissionService, PermissionService>();
        // Register email service as No-Op by default (safe for tests/dev).
        services.AddScoped<IEmailService, NoOpEmailService>();
        // Register audit service
        services.AddScoped<IAuditService, AuditService>();
        // Register notifications
        services.AddScoped<INotificationService, NotificationService>();
        // Register reporting
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
