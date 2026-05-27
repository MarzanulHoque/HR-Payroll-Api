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

6. **Communication Rule**:
  - Don't generate text in the chat. Just perform the required actions.

7. **README Maintenance**:
  - Keep `README.md` up to date to reflect the current project state, setup steps, and high-level module explanations. Update it for each completed phase.

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
- [x] **Phase 3: Dashboard & Reporting**: 
  - API queries to fetch metrics like "Total Employees", "Leaves Pending", "Attendance Today", "Payroll Expense".
- [ ] **Phase 3.5: Advanced Auth & RBAC**:
  - Advanced Auth APIs: Logout (token invalidation), Forgot Password, Reset Password, Email Verification, and Refresh Tokens (`RefreshToken` entity).
  - RBAC Entities: `Roles`, `Permissions`, `UserRoles`, `RolePermissions`. Dynamic permission mapping APIs.
- [ ] **Phase 3.6: Core HR Extensions**:
  - Employee Search & Filtering (by Name, ID, Department, Status).
  - Department Hierarchy and Manager Assignment extensions.
- [ ] **Phase 4: Advanced Attendance & Shifts module**:
  - `Shift` & `EmployeeShift` Entities for shift assignments.
  - Overtime calculation, late marks, absent reports, auto-generation of attendance.
- [ ] **Phase 5: Advanced Leave & Payroll Policies**:
  - `LeaveType` & `LeaveBalance` (Casual, Sick, Annual). Handling carryovers, limits, and holiday handling.
  - Multi-level Leave Workflow extensions (Employee -> Manager -> HR).
  - Payroll extensions: Tax calculation, Overtime inclusion, leave deductions, PDF payslip generation, Salary Increments/Promotions tracking, and Payroll Locking.
- [ ] **Phase 6: Company Configuration Module**:
  - `OrganizationPolicy` Entity to manage settings (working days, office hours, timezone, currency, HR/Leave/Payroll policies).
- [ ] **Phase 7: Background Jobs (Hangfire)**:
  - Setup Hangfire for async tasks (email queue, monthly automated payroll processing, attendance summaries).
- [ ] **Phase 8: Notification Module**:
  - `Notification` & `NotificationTemplate` Entities.
  - SignalR real-time hubs (Leave approvals, Payroll processed).
  - Email alerts (Welcome, password reset, Payslip delivery) wired to Hangfire.
- [ ] **Phase 9: Reporting Module**:
  - Export structured reports (Employee, Attendance, Payroll, Leave summary) containing Date/Department/Employee filters.
  - Support Excel, CSV, PDF formats.
- [ ] **Phase 10: Audit Logging Module**:
  - EF Core Interceptors to track entity changes into `AuditLogs` table (Old Value, New Value, Action, User, Timestamp, IP).
