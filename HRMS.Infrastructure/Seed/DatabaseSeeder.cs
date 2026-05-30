using HRMS.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();

        // Roles
        if (!db.Roles.Any())
        {
            var adminRole = new Role { Name = "Admin" };
            var hrRole = new Role { Name = "HR" };
            var payrollRole = new Role { Name = "Payroll" };

            db.Roles.AddRange(adminRole, hrRole, payrollRole);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Permissions
        if (!db.Permissions.Any())
        {
            var perms = new[]
            {
                new Permission { Name = "payroll.generate" },
                new Permission { Name = "payroll.view" },
                new Permission { Name = "employee.manage" },
                new Permission { Name = "roles.manage" }
            };

            db.Permissions.AddRange(perms);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Assign some defaults: Admin -> all
        var admin = db.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (admin != null && !db.RolePermissions.Any(rp => rp.RoleId == admin.Id))
        {
            var allPerms = db.Permissions.ToList();
            foreach (var p in allPerms)
            {
                db.RolePermissions.Add(new RolePermission { RoleId = admin.Id, PermissionId = p.Id });
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
