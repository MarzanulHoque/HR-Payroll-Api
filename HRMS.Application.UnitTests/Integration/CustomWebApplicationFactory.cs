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

            // seed
            var permission = new Permission { Id = _permissionId, Name = "payroll.generate" };
            var role = new Role { Id = _roleId, Name = "Admin" };
            var user = new User { Id = _userId, Email = "inttest@example.com", FirstName = "IntTest" };

            db.Permissions.Add(permission);
            db.Roles.Add(role);
            db.Users.Add(user);
            db.RolePermissions.Add(new RolePermission { RoleId = _roleId, PermissionId = _permissionId });
            db.UserRoles.Add(new UserRole { UserId = _userId, RoleId = _roleId });
            db.SaveChanges();
        });
    }
}
