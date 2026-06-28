Phase 3.5: Advanced Authentication & RBAC — Module Instructions

Overview

This instruction set guides the implementation of Advanced Authentication and Role-Based Access Control (RBAC) in the HRMS project. Follow these steps in the `feature/phase-3.5-rbac` branch. Each step produces minimal, test-covered commits.

Tasks (high-level)

1) Domain: Add `Permission` and `RolePermission` entities.
   - File: `HRMS.Domain/Entities/Permission.cs`.
   - Add `DbSet<Permission>` and `DbSet<RolePermission>` to `ApplicationDbContext` and configure composite key for `RolePermission`.

2) Application Interfaces: Add `IPermissionService`.
   - Location: `HRMS.Application/Common/Interfaces/Auth/IPermissionService.cs`.
   - Method: `Task<bool> UserHasPermissionAsync(Guid userId, string permissionName, CancellationToken ct = default)`.

3) Infrastructure Service: Implement `PermissionService`.
   - Location: `HRMS.Infrastructure/Services/PermissionService.cs`.
   - Should read roles/role-permissions and return whether user has the named permission.
   - Inject `IApplicationDbContext` for DB access.

4) Authorization handlers and policies.
   - Implement `PermissionRequirement : IAuthorizationRequirement` and `PermissionHandler : AuthorizationHandler<PermissionRequirement>` in `HRMS.Infrastructure/Authorization`.
   - Register policies in `HRMS.Infrastructure/DependencyInjection.AddInfrastructureLayer` (or `HRMS.API` startup): `options.AddPolicy("permission:X", policy => policy.Requirements.Add(new PermissionRequirement("X")));` — but prefer dynamic policy registration via helper.

5) Controller usage.
   - Example: protect `PayrollController.Generate` with `[Authorize(Policy = "payroll.generate")]` or use `IAuthorizationService` for programmatic checks.

6) Seeding and admin endpoints.
   - Seed initial roles and permissions in `HRMS.Infrastructure` seed logic (e.g., `Admin` role with core permissions).
   - Expose admin endpoints to manage roles/permissions behind Admin role.

7) Tests.
   - Unit tests for `PermissionService` and `PermissionHandler` under `HRMS.Application.UnitTests` and `HRMS.Infrastructure.UnitTests`.
   - Integration tests via `WebApplicationFactory` to assert policy-based access control on endpoints.

Branching and commits

- Work on `feature/phase-3.5-rbac` branch.
- Make multiple focused commits: `feat(rbac): add Permission entity`, `feat(rbac): add PermissionService and tests`, `feat(rbac): add authorization handler and policies`.
- Avoid the word "explanation" in commit messages.

Verification steps

1. Build and run tests: `dotnet build` && `dotnet test`.
2. Run API locally and verify an admin user can perform a protected action and a regular user cannot.

Notes

- Keep `Explanations/` local-only; do not stage or commit those files.
- Use DI to register `IPermissionService` and `PermissionHandler`.
