Implementation status vs PRD (snapshot: 2026-06-01)

Overview: each PRD feature is marked `Implemented`, `Partially implemented`, or `Not implemented` with brief notes and locations.

1) Authentication & Authorization
- Login / Register / Refresh tokens: Implemented (see `HRMS.API/Controllers/AuthController.cs`).
- Forgot-password / Reset-password: Not implemented (no handlers found).
- RBAC / Permissions / Dynamic policy provider: Implemented (permission provider, handler, seeding) — see `HRMS.API/Authorization` and `HRMS.Infrastructure` seed.

2) Company Configuration
- Organization settings / policies: Implemented (`CompanyConfigurationController` + `OrganizationPolicy` DTOs).

3) Employee Management
- CRUD (GET/POST/PUT/DELETE): Implemented (`HRMS.API/Controllers/EmployeesController.cs`).
- CSV import endpoint: Implemented (`POST api/v1/employees/import`).
- Employee documents/experiences/education: Not implemented (no entities/controllers found).

4) Department & Designation
- Department CRUD / Designation CRUD / hierarchy / manager assignment: Implemented — controllers present; added department tree endpoint with manager-mini DTO, parent/manager existence checks and simple cycle prevention in create/update handlers; unit tests passing.

5) Attendance Management
- Check-in / Check-out: Implemented (`AttendanceController` clock-in/clock-out).
- Shift management / EmployeeShifts / advanced rules (late/early/overtime calc): Not implemented (shift entities/controllers missing; org policy exists).
- Attendance reports: Partial — basic reporting present via `ReportService` (CSV attendance export implemented).

6) Leave Management
- Apply / Approve / Reject / Leave balance / History: Implemented (see `LeaveRequestsController` and application handlers).

7) Payroll Management
- Payroll generation / salary slips listing: Implemented (`PayrollController`, `GeneratePayroll` command).
- Payslip PDF generation: Implemented (simple PDF generator in `PayrollController` — minimal renderer).
- Payslip email sending: Implemented (uses `SendPayslipEmail` command + `IEmailService` NoOp implementation).
- Payroll locking / increment history: Not fully implemented (no explicit locking/increment modules found).

8) Dashboard & Analytics
- Dashboard endpoints: Implemented (`DashboardController`).
- Realtime updates via SignalR: Implemented (SignalR hub + EmailUserIdProvider + controller broadcasting).
- Advanced charts / cached stats: Partial — chart data endpoints may exist; Redis caching not implemented.

9) Notification Module
- In-app notifications: Implemented (`NotificationsController`, entity, service).
- Email notifications: Implemented via `NoOpEmailService` (dev-only).
- Notification templates: Not implemented (no template entity/controller found).

10) Reporting Module
- Employee / Attendance / Payroll CSV: Implemented (`ReportsController` + `ReportService`).
- Excel / PDF exports: Partial — CSV implemented; full Excel/PDF exports (advanced) not implemented beyond simple PDF for payslips.

11) Audit Logging Module
- Audit logs (old/new value, user, timestamp, IP): Implemented (`AuditLog` entity + `AuditService`, integrated in handlers).

12) Enterprise Features
- CQRS (MediatR): Implemented across application.
- Redis caching: Not implemented.
- Background jobs / Hangfire: Not implemented (no Hangfire setup found).
- Email queue (Hangfire-backed): Not implemented (NoOp email present for dev).

13) API Versioning
- Basic URL versioning used (`/api/v1/*`): Implemented.

14) Dockerization & DevOps
- Dockerfile / docker-compose: Not implemented (no Dockerfile / compose found).
- GitHub Actions: Not implemented.

15) Frontend (Angular)
- Frontend project: Not present in this repository.

16) Non-functional / Observability
- Serilog: Not configured (no `UseSerilog` found).
- Monitoring / metrics: Not implemented.

Recommended next high-priority work
- Implement forgot-password / reset-password flows and email tokens.
- Add Shift entities and endpoints for shift management and assignment.
- Add Hangfire (or background worker) for queued email sending / payroll jobs and seed recurring jobs.
- Add Redis caching for dashboard/lookup endpoints.
- Implement NotificationTemplates and template management CRUD.
- Add Dockerfile and `docker-compose.yml` for local dev stack (API + SQL + Redis).

File generated: `IMPLEMENTATION_STATUS.md` (this file)

If you want, I can open any of the specific unimplemented areas and scaffold the missing pieces — which should I prioritize?

## Future Implementation (backlog)

The following PRD items were removed from the active todo list and recorded here for future implementation and planning:

- Seed `notification.send` permission in `DatabaseSeeder` — ensure the permission exists and is assigned to the appropriate role(s) (e.g., Admin).
- Implement forgot-password & reset-password flows and email token handlers (`POST /api/v1/auth/forgot-password`, `POST /api/v1/auth/reset-password`).
- Add `Shift` and `EmployeeShift` entities plus shift-management endpoints (create, assign, list, update).
- Add a background worker/Hangfire setup for queued email jobs, payroll jobs, and recurring tasks.
- Add Redis caching for dashboard statistics and frequently-used lookup data; design cache keys and invalidation.
- Implement `NotificationTemplate` CRUD and placeholder/template rendering support.
- Add SignalR integration tests to validate hub connectivity and end-to-end delivery.
- Add `Dockerfile` and `docker-compose.yml` to run API + database + Redis locally for development.

These items are preserved in this file as future work items. If you want, I can scaffold any of them now — tell me which to begin.