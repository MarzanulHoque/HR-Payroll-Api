# HR Payroll Project

Enterprise HR & Payroll Management System (HRMS) - ASP.NET Core Web API backend implemented with Clean Architecture.

## Status
- Architecture: Clean Architecture (Domain / Application / Infrastructure / API)
- Database: SQLite (code-first)
- Auth: JWT + BCrypt
- Version branch: `v1.0.0`
- Completed phases: Initial architecture, Auth, Employee CRUD, Department & Designation, Attendance, Leave Management, Payroll Processing, Dashboard Metrics, Advanced Authentication & RBAC, Core HR Extensions
- Current branch work includes: dynamic permission policies, permission handler, role-permission seeding, refresh-token support, employee search/filtering foundation, department hierarchy, and manager assignment

## Tech stack
- .NET 10, ASP.NET Core Web API
- Entity Framework Core (SQLite)
- MediatR (CQRS)
- xUnit & Moq for unit tests

## Getting started
1. Install .NET 10 SDK.
2. From repository root run:

```powershell
dotnet build
cd HRMS.API
dotnet run
cd ..
dotnet test
```

The first local run seeds demo data automatically when the database is empty. The seeded credentials are stored in `postman/credentials.json` and use the shared demo password `Password123!`.

## Project layout
- `HRMS.Domain` — domain entities
- `HRMS.Application` — business logic, CQRS handlers
- `HRMS.Infrastructure` — EF Core, persistence
- `HRMS.API` — controllers and web host

## Development workflow notes
- Follow Clean Architecture: add entities to `HRMS.Domain`, handlers to `HRMS.Application`, mappings in `HRMS.Infrastructure`, and endpoints in `HRMS.API`.
- Every new feature must include `xUnit` tests in `HRMS.Application.UnitTests` and run `dotnet test` before committing.
- Generated local database files and build outputs are ignored via `.gitignore`.
- The startup path no longer reseeds on every launch; seeding is effectively one-time for a fresh local database.

## Completed modules
- Core HR: Employee, Department, Designation CRUD
- Attendance: Check-in / Check-out
- Leave Management: Submit / Approve / Reject
- Payroll Processing: SalarySlip generation, NetPay calculations
- Dashboard: Top-level metrics API
- Advanced Authentication & RBAC: permissions, dynamic policy provider, permission handler, admin protection, refresh tokens, and role-permission support
- Core HR Extensions: employee search/filtering and department hierarchy / manager assignment data model support

## Seeded demo data
- Roles: Admin, HR, Payroll, Manager, Employee
- Users: admin, HR admin, payroll user, manager, and multiple demo employees
- Permissions: payroll, employee, attendance, leave, and role management permissions
- HR records: departments, designations, attendance records, leave requests, salary slips, and refresh tokens

## Next phases
Planned roadmap is maintained privately and will be reflected in public documentation as phases are completed.

## Tests
Run `dotnet test` from solution root.

## Contact
Repo owner and maintainer: see project remote.
