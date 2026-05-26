# HR Payroll Project Documentation

## Overview
This is the backend API project for the Enterprise HR & Payroll Management System (HRMS). It serves as a uniform API that will be consumed later by independent Angular and React clients.

## Architecture
- **HRMS.Domain**: Core entities and models.
- **HRMS.Application**: Business logic, CQRS (MediatR), and DTOs.
- **HRMS.Infrastructure**: EF Core, SQLite configuration, and external services.
- **HRMS.API**: ASP.NET Core Web API endpoints.

## Current Progress (feature/api-foundation)
1. Scaffolded clean architecture solution.
2. Initialized SQLite database (`hrms.db`) and `ApplicationDbContext`.
3. Created initial `Employee` entity and applied DB migrations.
4. Initialized Git repository.
5. Added uniform API response models (Success/Error wrapping).
6. Installed and configured MediatR for CQRS pattern inside `HRMS.Application`.
7. Authored boilerplate Command and Handler for creating an `Employee`.

## Next Steps
- Integrate `ApplicationDbContext` Interface for Application layer isolation (Dependency Inversion).
- Create base API endpoint Controller to test MediatR command.
- Add authentication models and JWT setup.
