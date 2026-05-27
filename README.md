# HR Payroll Project

Enterprise HR & Payroll Management System (HRMS) - ASP.NET Core Web API backend implemented with Clean Architecture.

## Status
- Architecture: Clean Architecture (Domain / Application / Infrastructure / API)
- Database: SQLite (code-first)
- Auth: JWT + BCrypt
- Completed phases: Initial architecture, Auth, Employee CRUD, Department & Designation, Attendance, Leave Management, Payroll Processing, Dashboard Metrics

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
dotnet ef migrations add InitialCreate --project ../HRMS.Infrastructure
dotnet ef database update --project ../HRMS.Infrastructure
cd ..
dotnet test
```

## Project layout
- `HRMS.Domain` — domain entities
- `HRMS.Application` — business logic, CQRS handlers
- `HRMS.Infrastructure` — EF Core, persistence
- `HRMS.API` — controllers and web host

## Development workflow notes
- Follow Clean Architecture: add entities to `HRMS.Domain`, handlers to `HRMS.Application`, mappings in `HRMS.Infrastructure`, and endpoints in `HRMS.API`.
- Every new feature must include `xUnit` tests in `HRMS.Application.UnitTests` and run `dotnet test` before committing.
- Internal docs and AI instructions are ignored in Git (`AI_INSTRUCTIONS.md`, `Explanations/`).

## Completed modules
- Core HR: Employee, Department, Designation CRUD
- Attendance: Check-in / Check-out
- Leave Management: Submit / Approve / Reject
- Payroll Processing: SalarySlip generation, NetPay calculations
- Dashboard: Top-level metrics API

## Next phases (tracked in `AI_INSTRUCTIONS.md`)
See `AI_INSTRUCTIONS.md` for the detailed phased roadmap (Auth & RBAC, Advanced Attendance/Shift, Advanced Payroll, Company Configuration, Background Jobs, Notifications, Reporting, Audit Logging).

## Tests
Run `dotnet test` from solution root.

## Contact
Repo owner and maintainer: see project remote.
