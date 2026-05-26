# Architecture Overview

This project uses **Clean Architecture**, dividing code into four parts to keep everything organized and maintainable:

1. **Domain Layer (`HRMS.Domain`)**: 
   - Contains your core database tables (Entities) like `User`, `Employee`, `Department`.
   - Has no dependencies on anything else.

2. **Application Layer (`HRMS.Application`)**: 
   - Contains your Business Logic.
   - Uses the **CQRS Pattern (Command Query Responsibility Segregation)** using a package called **MediatR**.
   - **Commands**: Operations that change data (Create, Update, Delete).
   - **Queries**: Operations that only read data (GetById, GetAll).
   - It only depends on the `Domain` layer. It uses interfaces (like `IApplicationDbContext`) so it doesn't care whether you use SQLite or SQL Server.

3. **Infrastructure Layer (`HRMS.Infrastructure`)**: 
   - Communicates with the outside world (e.g., your SQLite Database, Identity token generation).
   - Implements the `IApplicationDbContext` interface using `ApplicationDbContext` (Entity Framework Core).

4. **API Layer (`HRMS.API`)**: 
   - The entry point of your application.
   - Contains Controllers (`AuthController`, `EmployeesController`) that receive HTTP requests (like POST or GET) and send them to the Application layer using MediatR.
   - Configures application startup, dependency injection, and Swagger UI.
