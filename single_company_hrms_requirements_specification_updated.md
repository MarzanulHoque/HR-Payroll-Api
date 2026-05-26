# Enterprise HR & Payroll Management System (HRMS)
## Detailed Requirement Specification Document

---

# 1. Project Overview

## Project Name
Enterprise HR & Payroll Management System (HRMS)

## Project Type
Enterprise web application for a single organization/company.

## Technology Stack

### Frontend
- Angular 19+
- TypeScript
- RxJS
- Angular Material / PrimeNG
- SCSS

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MediatR (CQRS)
- FluentValidation
- AutoMapper
- Serilog
- Redis
- Hangfire
- SignalR

### DevOps & Deployment
- Docker
- Docker Compose
- GitHub Actions (Later)

---

# 2. Project Goals

The goal of the system is to:

- Manage employees and HR operations
- Handle attendance and leave workflows
- Process payroll calculations
- Provide enterprise-grade architecture
- Demonstrate scalable backend practices
- Showcase modern Angular frontend architecture

---

# 3. User Roles

## 3.1 System Admin
Primary system administrator for the organization.

### Notes
- System Admin users will be inserted manually into the database.
- No public UI or API will exist for creating System Admin accounts.

### Permissions
- Manage users and roles
- Configure organization settings
- View all reports
- Monitor system activities

---

## 3.2 HR Admin
Responsible for HR operations.

### Permissions
- Employee management
- Payroll management
- Attendance management
- Leave approvals
- Reports
- Department management

---

## 3.3 Manager
Team-level authority.

### Permissions
- Approve/reject leave
- View team attendance
- View team reports
- Team employee overview

---

## 3.4 Employee
General employee user.

### Permissions
- View own profile
- Apply leave
- View payslips
- Check attendance
- Update limited profile info

---

# 4. Functional Modules

---

# 4.1 Authentication & Authorization Module

## Objective
Secure access control using JWT authentication and role-based authorization.

---

## Features

### User Registration
- User creation by HR/Admin
- Password hashing
- Email verification

### Login
- JWT authentication
- Refresh token support
- Access token expiration

### Forgot Password
- Email reset link
- OTP/token verification
- Password reset expiration

### Role-Based Access Control (RBAC)
- Roles
- Permissions
- Dynamic permission mapping

### Session Management
- Refresh token rotation
- Logout from all devices
- Session expiration

---

## APIs

### Auth APIs
- POST /api/v1/auth/login
- POST /api/v1/auth/register
- POST /api/v1/auth/refresh-token
- POST /api/v1/auth/logout
- POST /api/v1/auth/forgot-password
- POST /api/v1/auth/reset-password

---

## Database Tables
- Users
- Roles
- Permissions
- UserRoles
- RolePermissions
- RefreshTokens

---

# 4.2 Company Configuration Module

## Objective
Manage organization-specific settings and policies.

---

## Features

### Organization Settings
- Working days configuration
- Office hours
- Time zone settings
- Currency settings
- Payroll configuration

### HR Policies
- Attendance policy
- Leave policy
- Payroll policy

---

## Database Tables
- OrganizationPolicies

---

# 4.3 Employee Management Module

## Objective
Manage employee information and lifecycle.

---

## Features

### Employee CRUD
- Add employee
- Edit employee
- Soft delete employee
- Employee status

### Employee Profile
- Personal information
- Emergency contact
- Address

### Employment Information
- Department
- Designation
- Joining date
- Employment type
- Reporting manager

### Employee Search & Filtering
- Search by name
- Search by employee ID
- Filter by department
- Filter by status

---

## APIs
- GET /api/v1/employees
- GET /api/v1/employees/{id}
- POST /api/v1/employees
- PUT /api/v1/employees/{id}
- DELETE /api/v1/employees/{id}

---

## Database Tables
- Employees
- Departments
- Designations
- EmployeeDocuments
- EmployeeExperiences
- EmployeeEducations

---

# 4.4 Department & Designation Module

## Features
- Department CRUD
- Designation CRUD
- Department hierarchy
- Manager assignment

---

## Database Tables
- Departments
- Designations

---

# 4.5 Attendance Management Module

## Objective
Track employee attendance and working hours.

---

## Features

### Daily Attendance
- Check-in
- Check-out
- Work duration calculation

### Shift Management
- Shift creation
- Shift assignment
- Flexible shifts

### Attendance Rules
- Late marking
- Early leave detection
- Overtime calculation

### Attendance Reports
- Monthly attendance report
- Late report
- Absent report

### Advanced Features
- Auto attendance generation

---

## APIs
- POST /api/v1/attendance/check-in
- POST /api/v1/attendance/check-out
- GET /api/v1/attendance/report

---

## Database Tables
- Attendances
- Shifts
- EmployeeShifts
- AttendancePolicies

---

# 4.6 Leave Management Module

## Objective
Manage employee leave requests and approvals.

---

## Features

### Leave Types
- Casual leave
- Sick leave
- Annual leave
- Unpaid leave

### Leave Request Workflow
Employee -> Manager -> HR

### Leave Features
- Apply leave
- Approve/reject leave
- Leave balance tracking
- Leave history

### Leave Policies
- Carry forward
- Leave limits
- Holiday handling

---

## APIs
- POST /api/v1/leaves/apply
- POST /api/v1/leaves/approve
- POST /api/v1/leaves/reject
- GET /api/v1/leaves/balance

---

## Database Tables
- Leaves
- LeaveTypes
- LeaveBalances
- LeaveApprovals

---

# 4.7 Payroll Management Module

## Objective
Automate salary and payroll processing.

---

## Features

### Salary Structure
- Basic salary
- Allowances
- Bonus
- Deductions

### Payroll Processing
- Monthly payroll generation
- Tax calculation
- Overtime inclusion
- Leave deduction

### Payslip
- PDF payslip generation
- Email payslip delivery

### Payroll Locking
- Prevent modification after finalization

### Increment Management
- Salary increment history
- Promotion tracking

---

## Payroll Formula

Net Salary =
(Basic + Allowances + Bonus + Overtime)
- (Tax + Leave Deduction + Other Deductions)

---

## APIs
- POST /api/v1/payroll/process
- GET /api/v1/payroll/payslip/{employeeId}
- GET /api/v1/payroll/history

---

## Database Tables
- Payrolls
- SalaryStructures
- SalaryComponents
- PayrollDeductions
- PayrollBonuses
- TaxConfigurations

---

# 4.8 Dashboard & Analytics Module

## Objective
Provide visual business insights.

---

## Features

### Dashboard Cards
- Total employees
- Present today
- Leave requests
- Payroll expense

### Charts
- Attendance trends
- Department distribution
- Payroll expenses
- Leave statistics

### Realtime Updates
Using SignalR.

---

# 4.9 Notification Module

## Objective
Provide realtime and email notifications.

---

## Features

### In-App Notifications
- Leave approval
- Payroll processed
- Attendance alerts

### Email Notifications
- Welcome email
- Password reset
- Payslip delivery
- Leave updates

### SignalR Notifications
Realtime alerts.

---

## Database Tables
- Notifications
- NotificationTemplates

---

# 4.10 Reporting Module

## Objective
Generate downloadable reports.

---

## Features

### Reports
- Employee report
- Attendance report
- Payroll report
- Leave summary

### Export Formats
- Excel
- PDF
- CSV

### Filters
- Date range
- Department
- Employee

---

# 4.11 Audit Logging Module

## Objective
Track all important system changes.

---

## Features

### Track
- Old value
- New value
- User action
- Timestamp
- IP address

---

## Database Tables
- AuditLogs

---

# 5. Enterprise Features

---

# 5.1 CQRS Architecture

## Technology
- MediatR

---

## Structure

### Commands
- CreateEmployeeCommand
- UpdateEmployeeCommand

### Queries
- GetEmployeesQuery
- GetEmployeeDetailsQuery

---

# 5.2 Redis Caching

## Cache Targets
- Dashboard statistics
- Lookup data
- Employee lists
- Frequently accessed reports

---

## Cache Strategy

### Cache Key Example
cache:employees

### Cache Invalidation
- On update
- On delete

---

# 5.3 Background Jobs

## Technology
- Hangfire

---

## Jobs
- Payroll processing
- Email sending
- Attendance summary
- Monthly reports

---

# 5.4 Email Queue System

## Objective
Handle email sending asynchronously using a background job system with a fake/log-based email sender for development.

---

## Workflow
API Request  
→ Queue Email Job  
→ Hangfire Background Job  
→ Log-Based Email Sender (Console/Serilog)

---

## Email Strategy (Development Mode)

Instead of sending real emails:
- Emails will be written to logs
- Stored in Hangfire job history
- Optionally saved in database table for tracking

---

## Email Types
- Welcome email
- Password reset email
- Payslip email
- Leave approval notification

---

## Implementation Approach

### Email Sender Service
- FakeEmailSenderService
- Logs email content (To, Subject, Body)
- No SMTP or external provider required

### Benefits
- No external dependency
- Easy local testing
- Safe for development and demos

---

## Database Tables
- Notifications
- NotificationTemplates

---

# 5.5 API Versioning

## Strategy
URL versioning.

Examples:
- /api/v1/employees
- /api/v2/employees

---

# 5.6 Dockerization

## Containers
- API
- Angular app
- SQL Server
- Redis

---

# 6. Non-Functional Requirements

## Security
- JWT authentication
- Refresh tokens
- Password hashing
- HTTPS
- Role authorization
- Input validation
- SQL injection prevention
- XSS prevention

---

## Performance
- Pagination
- Async programming
- Redis caching
- Optimized queries
- Database indexing

---

## Scalability
- Clean architecture
- Modular structure
- CQRS
- Docker support

---

## Maintainability
- SOLID principles
- Repository pattern
- Unit testing support
- Logging
- Centralized exception handling

---

# 7. Angular Frontend Architecture

```txt
src/app
 ├── core
 ├── shared
 ├── layout
 ├── features
 ├── services
 ├── state
 └── models
```

---

## Feature Modules
- auth
- employee
- payroll
- attendance
- leave
- dashboard

---

# 8. Backend Architecture

```txt
src/
 ├── HRMS.API
 ├── HRMS.Application
 ├── HRMS.Domain
 ├── HRMS.Infrastructure
```

---

## HRMS.API
- Controllers
- Middleware
- Swagger
- Authentication setup

---

## HRMS.Application
- CQRS
- DTOs
- Validators
- Interfaces
- Business rules

---

## HRMS.Domain
- Entities
- Enums
- Domain models

---

## HRMS.Infrastructure
- EF Core
- Repositories
- Redis
- Hangfire
- External services

---

# 9. Database Design Overview

## Core Tables

### Identity
- Users
- Roles
- Permissions
- RefreshTokens

### HR
- Employees
- Departments
- Designations

### Attendance
- Attendances
- Shifts

### Leave
- Leaves
- LeaveTypes
- LeaveBalances

### Payroll
- Payrolls
- SalaryStructures

### Company
- OrganizationPolicies

### System
- Notifications
- AuditLogs

---

# 10. API Standards

## Success Response
```json
{
  "success": true,
  "message": "Employee created successfully",
  "data": {}
}
```

---

## Error Response
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": []
}
```

---

# 11. Logging & Monitoring

## Technology
- Serilog

---

## Log Types
- Request logs
- Error logs
- Performance logs
- Audit logs

---

# 12. Development Roadmap

---

# Phase 1

## Foundation
- Project setup
- Clean architecture
- JWT authentication
- Role management
- Organization policy configuration
- Employee CRUD

---

# Phase 2

## Core Business Modules
- Attendance
- Leave management
- Payroll management
- Dashboard
- Reporting

---

# Phase 3

## Enterprise Features
- CQRS
- Redis caching
- Hangfire
- Email queue
- API versioning

---

# Phase 4

## Deployment & Optimization
- Docker
- Docker Compose
- Logging improvements
- Query optimization
- Production deployment

---

# 13. Future Enhancements

## Optional Features
- Mobile app

- Microservices migration
- Kafka event bus

