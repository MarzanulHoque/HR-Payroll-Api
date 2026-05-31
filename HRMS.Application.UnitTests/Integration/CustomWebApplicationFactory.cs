using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using HRMS.Infrastructure.Data;
using HRMS.Domain.Entities;

namespace HRMS.Application.UnitTests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Guid _userId;
    private readonly Guid _roleId;
    private readonly Guid _permissionId;

    public CustomWebApplicationFactory(Guid userId, Guid roleId, Guid permissionId)
    {
        _userId = userId;
        _roleId = roleId;
        _permissionId = permissionId;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Ensure the app does not run Development-only startup seeds during tests
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // replace DbContext to use a shared in-memory SQLite connection so EF provider matches the app
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            var sqliteConn = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
            sqliteConn.Open();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(sqliteConn);
            });

            // replace authentication with test scheme
            services.RemoveAll(typeof(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // seed the set of users/roles/permissions that other initialization code expects
            var permission = new Permission { Id = _permissionId, Name = "payroll.generate" };
            var role = new Role { Id = _roleId, Name = "Admin" };

            var adminUser = new User { Id = _userId, Email = "admin@hrms.local", FirstName = "System", LastName = "Admin" };
            var hrUser = new User { Email = "hr.admin@hrms.local", FirstName = "Hannah", LastName = "Reed" };
            var payrollUser = new User { Email = "payroll@hrms.local", FirstName = "Paul", LastName = "Wright" };
            var managerUser = new User { Email = "manager@hrms.local", FirstName = "Maya", LastName = "Patel" };

            var employeeUsers = new[]
            {
                new User { Email = "employee1@hrms.local", FirstName = "Alice", LastName = "Johnson" },
                new User { Email = "employee2@hrms.local", FirstName = "Bob", LastName = "Smith" },
                new User { Email = "employee3@hrms.local", FirstName = "Clara", LastName = "Williams" },
                new User { Email = "employee4@hrms.local", FirstName = "David", LastName = "Brown" },
                new User { Email = "employee5@hrms.local", FirstName = "Eva", LastName = "Green" },
                new User { Email = "employee6@hrms.local", FirstName = "Frank", LastName = "Miller" },
                new User { Email = "employee7@hrms.local", FirstName = "Grace", LastName = "Lee" }
            };

            db.Permissions.Add(permission);
            db.Roles.Add(role);
            db.Users.AddRange(adminUser, hrUser, payrollUser, managerUser);
            db.Users.AddRange(employeeUsers);
            db.RolePermissions.Add(new RolePermission { RoleId = _roleId, PermissionId = _permissionId });
            db.UserRoles.Add(new UserRole { UserId = _userId, RoleId = _roleId });
            db.SaveChanges();

            // Diagnostic: print seeded users to test output to verify presence of admin@hrms.local
            var seeded = db.Users.Select(u => u.Email).ToList();
            Console.WriteLine("[TestSeed] Seeded users: " + string.Join(", ", seeded));
        });
    }
}
