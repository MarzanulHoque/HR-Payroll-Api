# AI Work Plan & Instruction Set

This document serves as the permanent memory and strict rulebook for the AI assistant working on the HR & Payroll Management System. 

## 1. Project Context
- **Project Name**: Enterprise HR & Payroll Management System
- **Tech Stack**: ASP.NET Core Web API, Entity Framework Core (SQLite), MediatR (CQRS), JWT Authentication.
- **Architecture**: Strict Clean Architecture (Domain, Application, Infrastructure, API).

## 2. Core AI Constraints & Rules
The AI must strictly adhere to the following workflow for every feature request:

1. **Follow Clean Architecture & CQRS**: 
   - Add core tables to `HRMS.Domain`.
   - Add DB mappings to `HRMS.Infrastructure`.
   - Place MediatR Commands/Queries inside `HRMS.Application`.
   - Expose endpoints gracefully in `HRMS.API`.

2. **Always Write Unit Tests**: 
   - Every single newly added feature (Commands/Queries) MUST have an accompanying `xUnit` + `Moq` test inside the `HRMS.Application.UnitTests` project.
   - Run `dotnet test` to verify before finalizing the step.

3. **Generate Plain-English Explanations**: 
   - For every major feature, create or update a markdown file in the `Explanations/` folder.
   - It must describe the architectural flow and contain relevant C# code snippets for non-technical comprehension.

4. **Phase-by-Phase Version Control**: 
   - Never pile on too many features without committing.
   - Run `dotnet build` and `dotnet test`. 
   - Use `git add .` and `git commit -m "feat: [Description]"` upon the completion of a module.
   - **Crucial Git Rule**: Do NOT mention the word "explanation" or "explanations" in the commit message. Create the explanation files, but keep the commit message focused on the code features.

5. **Secrets & Gitignore Compliance**: 
   - Never commit `Explanations/` or `AI_INSTRUCTIONS.md` to Git. (They are ignored).

---

## 3. Work Plan Execution Checklist

### ✅ Completed Phases
- [x] Initial Architecture Setup & SQLite EF Core Configuration.
- [x] Security: JWT Authentication & BCrypt Password Hashing.
- [x] Core HR: Employee Management CRUD.
- [x] Core HR: Department & Designation Management CRUD.
- [x] Operations: Attendance Management (Clock-in / Clock-out).

### 🚀 Upcoming Execution Plan (In Order)
- [x] **Phase 1: Leave Management**: 
  - `LeaveRequest` Entity (EmployeeId, StartDate, EndDate, Reason, Status).
  - Submit Leave Request (Command), Approve/Reject Leave Request (Command).
- [x] **Phase 2: Payroll Processing**: 
  - `SalarySlip` Entity (EmployeeId, BaseSalary, Deductions, NetPay, Month).
  - Generate Payroll Command.
- [x] **Phase 3: Dashboard & Reporting (Optional/Later)**: 
  - API queries to fetch metrics like "Total Employees", "Leaves Pending", "Attendance Today".
