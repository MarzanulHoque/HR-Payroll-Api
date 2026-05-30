using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();

        const string samplePassword = "Password123!";
        var samplePasswordHash = BCrypt.Net.BCrypt.HashPassword(samplePassword);

        if (!await db.Roles.AnyAsync(cancellationToken))
        {
            db.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "HR" },
                new Role { Name = "Payroll" },
                new Role { Name = "Manager" },
                new Role { Name = "Employee" });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Permissions.AnyAsync(cancellationToken))
        {
            db.Permissions.AddRange(
                new Permission { Name = "payroll.generate" },
                new Permission { Name = "payroll.view" },
                new Permission { Name = "employee.manage" },
                new Permission { Name = "employee.view" },
                new Permission { Name = "attendance.manage" },
                new Permission { Name = "attendance.view" },
                new Permission { Name = "leave.manage" },
                new Permission { Name = "leave.approve" },
                new Permission { Name = "leave.apply" },
                new Permission { Name = "roles.manage" });

            await db.SaveChangesAsync(cancellationToken);
        }

        var roles = await db.Roles.ToDictionaryAsync(r => r.Name, cancellationToken);
        var permissions = await db.Permissions.ToDictionaryAsync(p => p.Name, cancellationToken);

        if (!await db.Users.AnyAsync(cancellationToken))
        {
            var adminUser = new User
            {
                Email = "admin@hrms.local",
                PasswordHash = samplePasswordHash,
                FirstName = "System",
                LastName = "Admin"
            };

            var hrUser = new User
            {
                Email = "hr.admin@hrms.local",
                PasswordHash = samplePasswordHash,
                FirstName = "Hannah",
                LastName = "Reed"
            };

            var payrollUser = new User
            {
                Email = "payroll@hrms.local",
                PasswordHash = samplePasswordHash,
                FirstName = "Paul",
                LastName = "Wright"
            };

            var managerUser = new User
            {
                Email = "manager@hrms.local",
                PasswordHash = samplePasswordHash,
                FirstName = "Maya",
                LastName = "Patel"
            };

            var employeeUsers = new[]
            {
                new User { Email = "employee1@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Alice", LastName = "Johnson" },
                new User { Email = "employee2@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Bob", LastName = "Smith" },
                new User { Email = "employee3@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Clara", LastName = "Williams" },
                new User { Email = "employee4@hrms.local", PasswordHash = samplePasswordHash, FirstName = "David", LastName = "Brown" },
                new User { Email = "employee5@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Eva", LastName = "Green" },
                new User { Email = "employee6@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Frank", LastName = "Miller" },
                new User { Email = "employee7@hrms.local", PasswordHash = samplePasswordHash, FirstName = "Grace", LastName = "Lee" }
            };

            db.Users.AddRange(adminUser, hrUser, payrollUser, managerUser);
            db.Users.AddRange(employeeUsers);
            await db.SaveChangesAsync(cancellationToken);
        }

        var users = await db.Users.ToDictionaryAsync(u => u.Email, cancellationToken);

        if (!await db.Set<UserRole>().AnyAsync(cancellationToken))
        {
            db.AddRange(
                new UserRole { UserId = users["admin@hrms.local"].Id, RoleId = roles["Admin"].Id },
                new UserRole { UserId = users["hr.admin@hrms.local"].Id, RoleId = roles["HR"].Id },
                new UserRole { UserId = users["payroll@hrms.local"].Id, RoleId = roles["Payroll"].Id },
                new UserRole { UserId = users["manager@hrms.local"].Id, RoleId = roles["Manager"].Id },
                new UserRole { UserId = users["employee1@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee2@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee3@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee4@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee5@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee6@hrms.local"].Id, RoleId = roles["Employee"].Id },
                new UserRole { UserId = users["employee7@hrms.local"].Id, RoleId = roles["Employee"].Id });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Employees.AnyAsync(cancellationToken))
        {
            db.Employees.AddRange(
                new Employee
                {
                    FirstName = "Maya",
                    LastName = "Patel",
                    Email = "maya.patel@hrms.local",
                    Department = "Corporate",
                    Designation = "Director",
                    JoiningDate = DateTime.UtcNow.AddYears(-4)
                },
                new Employee
                {
                    FirstName = "Hannah",
                    LastName = "Reed",
                    Email = "hannah.reed@hrms.local",
                    Department = "Human Resources",
                    Designation = "HR Manager",
                    JoiningDate = DateTime.UtcNow.AddYears(-3)
                },
                new Employee
                {
                    FirstName = "Paul",
                    LastName = "Wright",
                    Email = "paul.wright@hrms.local",
                    Department = "Finance",
                    Designation = "Finance Lead",
                    JoiningDate = DateTime.UtcNow.AddYears(-3)
                },
                new Employee
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "employee1@hrms.local",
                    Department = "Technology",
                    Designation = "Software Engineer",
                    JoiningDate = DateTime.UtcNow.AddMonths(-18)
                },
                new Employee
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "employee2@hrms.local",
                    Department = "Operations",
                    Designation = "Operations Specialist",
                    JoiningDate = DateTime.UtcNow.AddMonths(-20)
                },
                new Employee
                {
                    FirstName = "Clara",
                    LastName = "Williams",
                    Email = "employee3@hrms.local",
                    Department = "Payroll",
                    Designation = "Payroll Officer",
                    JoiningDate = DateTime.UtcNow.AddMonths(-16)
                },
                new Employee
                {
                    FirstName = "David",
                    LastName = "Brown",
                    Email = "employee4@hrms.local",
                    Department = "Human Resources",
                    Designation = "Recruitment Specialist",
                    JoiningDate = DateTime.UtcNow.AddMonths(-10)
                },
                new Employee
                {
                    FirstName = "Eva",
                    LastName = "Green",
                    Email = "employee5@hrms.local",
                    Department = "Compliance",
                    Designation = "Compliance Analyst",
                    JoiningDate = DateTime.UtcNow.AddMonths(-14)
                },
                new Employee
                {
                    FirstName = "Frank",
                    LastName = "Miller",
                    Email = "employee6@hrms.local",
                    Department = "Support",
                    Designation = "Support Associate",
                    JoiningDate = DateTime.UtcNow.AddMonths(-9)
                },
                new Employee
                {
                    FirstName = "Grace",
                    LastName = "Lee",
                    Email = "employee7@hrms.local",
                    Department = "Recruitment",
                    Designation = "Recruiter",
                    JoiningDate = DateTime.UtcNow.AddMonths(-12)
                });

            await db.SaveChangesAsync(cancellationToken);
        }

        var employees = await db.Employees.ToDictionaryAsync(e => e.Email, cancellationToken);

        if (!await db.Departments.AnyAsync(cancellationToken))
        {
            var corporate = new Department
            {
                Name = "Corporate",
                Description = "Executive leadership and overall company operations"
            };

            var humanResources = new Department
            {
                Name = "Human Resources",
                Description = "Employee lifecycle, policy, and staffing",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["hannah.reed@hrms.local"].Id
            };

            var finance = new Department
            {
                Name = "Finance",
                Description = "Accounts, planning, and salary operations",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["paul.wright@hrms.local"].Id
            };

            var payroll = new Department
            {
                Name = "Payroll",
                Description = "Monthly payroll processing and payslips",
                ParentDepartmentId = finance.Id,
                ManagerId = employees["employee3@hrms.local"].Id
            };

            var operations = new Department
            {
                Name = "Operations",
                Description = "Day-to-day service delivery and coordination",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["employee2@hrms.local"].Id
            };

            var technology = new Department
            {
                Name = "Technology",
                Description = "Software delivery and technical support",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["employee1@hrms.local"].Id
            };

            var compliance = new Department
            {
                Name = "Compliance",
                Description = "Policy compliance and audit readiness",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["employee5@hrms.local"].Id
            };

            var support = new Department
            {
                Name = "Support",
                Description = "Internal support and service requests",
                ParentDepartmentId = corporate.Id,
                ManagerId = employees["employee6@hrms.local"].Id
            };

            var recruitment = new Department
            {
                Name = "Recruitment",
                Description = "Hiring and onboarding coordination",
                ParentDepartmentId = humanResources.Id,
                ManagerId = employees["employee7@hrms.local"].Id
            };

            db.Departments.AddRange(corporate, humanResources, finance, payroll, operations, technology, compliance, support, recruitment);
            await db.SaveChangesAsync(cancellationToken);
        }

        var departments = await db.Departments.ToDictionaryAsync(d => d.Name, cancellationToken);

        if (!await db.Designations.AnyAsync(cancellationToken))
        {
            db.Designations.AddRange(
                new Designation
                {
                    Title = "Director",
                    Description = "Executive owner of the organization",
                    DepartmentId = departments["Corporate"].Id
                },
                new Designation
                {
                    Title = "HR Manager",
                    Description = "Leads HR operations",
                    DepartmentId = departments["Human Resources"].Id
                },
                new Designation
                {
                    Title = "Finance Lead",
                    Description = "Oversees finance activities",
                    DepartmentId = departments["Finance"].Id
                },
                new Designation
                {
                    Title = "Payroll Officer",
                    Description = "Processes salary runs",
                    DepartmentId = departments["Payroll"].Id
                },
                new Designation
                {
                    Title = "Operations Specialist",
                    Description = "Coordinates operational tasks",
                    DepartmentId = departments["Operations"].Id
                },
                new Designation
                {
                    Title = "Software Engineer",
                    Description = "Builds internal applications",
                    DepartmentId = departments["Technology"].Id
                },
                new Designation
                {
                    Title = "Recruitment Specialist",
                    Description = "Supports hiring and onboarding",
                    DepartmentId = departments["Human Resources"].Id
                },
                new Designation
                {
                    Title = "Compliance Analyst",
                    Description = "Reviews policy and audit controls",
                    DepartmentId = departments["Compliance"].Id
                },
                new Designation
                {
                    Title = "Support Associate",
                    Description = "Handles internal support requests",
                    DepartmentId = departments["Support"].Id
                },
                new Designation
                {
                    Title = "Recruiter",
                    Description = "Coordinates hiring and interviews",
                    DepartmentId = departments["Recruitment"].Id
                });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Set<RolePermission>().AnyAsync(cancellationToken))
        {
            var adminRole = roles["Admin"].Id;
            var hrRole = roles["HR"].Id;
            var payrollRole = roles["Payroll"].Id;
            var managerRole = roles["Manager"].Id;
            var employeeRole = roles["Employee"].Id;

            var rolePermissions = new[]
            {
                new RolePermission { RoleId = adminRole, PermissionId = permissions["payroll.generate"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["payroll.view"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["employee.manage"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["employee.view"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["attendance.manage"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["attendance.view"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["leave.manage"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["leave.approve"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["leave.apply"].Id },
                new RolePermission { RoleId = adminRole, PermissionId = permissions["roles.manage"].Id },

                new RolePermission { RoleId = hrRole, PermissionId = permissions["employee.manage"].Id },
                new RolePermission { RoleId = hrRole, PermissionId = permissions["employee.view"].Id },
                new RolePermission { RoleId = hrRole, PermissionId = permissions["attendance.manage"].Id },
                new RolePermission { RoleId = hrRole, PermissionId = permissions["attendance.view"].Id },
                new RolePermission { RoleId = hrRole, PermissionId = permissions["leave.manage"].Id },
                new RolePermission { RoleId = hrRole, PermissionId = permissions["leave.approve"].Id },

                new RolePermission { RoleId = payrollRole, PermissionId = permissions["payroll.generate"].Id },
                new RolePermission { RoleId = payrollRole, PermissionId = permissions["payroll.view"].Id },

                new RolePermission { RoleId = managerRole, PermissionId = permissions["employee.view"].Id },
                new RolePermission { RoleId = managerRole, PermissionId = permissions["attendance.view"].Id },
                new RolePermission { RoleId = managerRole, PermissionId = permissions["leave.approve"].Id },

                new RolePermission { RoleId = employeeRole, PermissionId = permissions["payroll.view"].Id },
                new RolePermission { RoleId = employeeRole, PermissionId = permissions["leave.apply"].Id },
                new RolePermission { RoleId = employeeRole, PermissionId = permissions["attendance.view"].Id }
            };

            db.AddRange(rolePermissions);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.RefreshTokens.AnyAsync(cancellationToken))
        {
            db.RefreshTokens.AddRange(
                new RefreshToken
                {
                    Token = Guid.NewGuid().ToString("N"),
                    Expires = DateTime.UtcNow.AddDays(7),
                    UserId = users["admin@hrms.local"].Id
                },
                new RefreshToken
                {
                    Token = Guid.NewGuid().ToString("N"),
                    Expires = DateTime.UtcNow.AddDays(7),
                    UserId = users["hr.admin@hrms.local"].Id
                });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.AttendanceRecords.AnyAsync(cancellationToken))
        {
            var attendanceDate = DateTime.UtcNow.Date.AddDays(-1);
            db.AttendanceRecords.AddRange(
                new AttendanceRecord
                {
                    EmployeeId = employees["employee1@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(9).AddMinutes(5),
                    ClockOutTime = attendanceDate.AddHours(18).AddMinutes(2),
                    Status = "Present"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee2@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(9).AddMinutes(20),
                    ClockOutTime = attendanceDate.AddHours(18).AddMinutes(10),
                    Status = "Late"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee3@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(8).AddMinutes(55),
                    ClockOutTime = attendanceDate.AddHours(17).AddMinutes(45),
                    Status = "Present"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee4@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(9).AddMinutes(10),
                    ClockOutTime = attendanceDate.AddHours(18),
                    Status = "Present"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee5@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(9).AddMinutes(2),
                    ClockOutTime = attendanceDate.AddHours(17).AddMinutes(50),
                    Status = "Present"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee6@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(9).AddMinutes(30),
                    ClockOutTime = attendanceDate.AddHours(18).AddMinutes(5),
                    Status = "Late"
                },
                new AttendanceRecord
                {
                    EmployeeId = employees["employee7@hrms.local"].Id,
                    Date = attendanceDate,
                    ClockInTime = attendanceDate.AddHours(8).AddMinutes(50),
                    ClockOutTime = attendanceDate.AddHours(17).AddMinutes(40),
                    Status = "Present"
                });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.LeaveRequests.AnyAsync(cancellationToken))
        {
            var today = DateTime.UtcNow.Date;
            db.LeaveRequests.AddRange(
                new LeaveRequest
                {
                    EmployeeId = employees["employee1@hrms.local"].Id,
                    LeaveType = "Sick",
                    StartDate = today.AddDays(2),
                    EndDate = today.AddDays(3),
                    Reason = "Medical appointment",
                    Status = "Pending"
                },
                new LeaveRequest
                {
                    EmployeeId = employees["employee2@hrms.local"].Id,
                    LeaveType = "Vacation",
                    StartDate = today.AddDays(-10),
                    EndDate = today.AddDays(-8),
                    Reason = "Family trip",
                    Status = "Approved"
                },
                new LeaveRequest
                {
                    EmployeeId = employees["employee3@hrms.local"].Id,
                    LeaveType = "Personal",
                    StartDate = today.AddDays(5),
                    EndDate = today.AddDays(5),
                    Reason = "Personal work",
                    Status = "Rejected"
                },
                new LeaveRequest
                {
                    EmployeeId = employees["employee5@hrms.local"].Id,
                    LeaveType = "Vacation",
                    StartDate = today.AddDays(14),
                    EndDate = today.AddDays(16),
                    Reason = "Annual leave",
                    Status = "Pending"
                },
                new LeaveRequest
                {
                    EmployeeId = employees["employee6@hrms.local"].Id,
                    LeaveType = "Sick",
                    StartDate = today.AddDays(-4),
                    EndDate = today.AddDays(-3),
                    Reason = "Medical recovery",
                    Status = "Approved"
                },
                new LeaveRequest
                {
                    EmployeeId = employees["employee7@hrms.local"].Id,
                    LeaveType = "Personal",
                    StartDate = today.AddDays(8),
                    EndDate = today.AddDays(8),
                    Reason = "Family commitment",
                    Status = "Pending"
                });

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.SalarySlips.AnyAsync(cancellationToken))
        {
            db.SalarySlips.AddRange(
                new SalarySlip
                {
                    EmployeeId = employees["employee1@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 6500m,
                    Deductions = 250m,
                    NetPay = 6250m,
                    Status = "Generated"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee2@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 5800m,
                    Deductions = 150m,
                    NetPay = 5650m,
                    Status = "Paid"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee3@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 7200m,
                    Deductions = 300m,
                    NetPay = 6900m,
                    Status = "Generated"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee4@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 6100m,
                    Deductions = 200m,
                    NetPay = 5900m,
                    Status = "Generated"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee5@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 5600m,
                    Deductions = 120m,
                    NetPay = 5480m,
                    Status = "Paid"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee6@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 5400m,
                    Deductions = 100m,
                    NetPay = 5300m,
                    Status = "Generated"
                },
                new SalarySlip
                {
                    EmployeeId = employees["employee7@hrms.local"].Id,
                    Month = "May 2026",
                    BaseSalary = 5900m,
                    Deductions = 180m,
                    NetPay = 5720m,
                    Status = "Generated"
                });

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
