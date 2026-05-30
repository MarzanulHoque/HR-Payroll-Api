Agent Skill: Phase 3.5 — Advanced Auth & RBAC

Purpose

This skill instructs the coding assistant (agent) how to implement Phase 3.5: Advanced Authentication and Role-Based Access Control for the HRMS project. It defines the responsibilities, constraints, and safe editing patterns the agent must follow when making changes related to authentication, authorization, permissions, and RBAC.

When to use this skill
- Implementing new domain entities for permissions and role-permission mappings.
- Adding services to check permissions (`IPermissionService` + implementation).
- Adding authorization `AuthorizationHandler` classes and registering policies.
- Adding seeding or administrative endpoints for managing roles & permissions.
- Writing unit and integration tests that cover permission checks and policy handlers.

Constraints and rules
- Do not modify or commit any `Explanations/` or `AI_INSTRUCTIONS.md` files as part of feature commits — those remain local-only.
- Create a dedicated branch per module. Use branch name format `feature/phase-3.5-rbac`.
- All changes that add or modify code must include unit tests for new handlers/services.
- Avoid wide-reaching refactors; keep changes minimal and localized to the RBAC scope.

Coding patterns and expectations
- Add domain entities under `HRMS.Domain/Entities` (e.g., `Permission`, `RolePermission`).
- Register new DbSets and configure composite keys in `HRMS.Infrastructure/Data/ApplicationDbContext.cs`.
- Define an `IPermissionService` in `HRMS.Application.Common.Interfaces.Auth` and implement in `HRMS.Infrastructure/Services` (e.g., `PermissionService`).
- Implement policy handlers under `HRMS.Infrastructure/Authorization` and register them in `DependencyInjection`.
- Use `IApplicationDbContext` for DB access inside the `PermissionService` and handlers; mock it in unit tests.

Testing requirements
- Unit tests for `PermissionService` and any `AuthorizationHandler` (xUnit + Moq).
- Integration tests should use SQLite in-memory to verify DB mappings and policy behavior when applied to controllers.

Commit and branch rules
- Use branch per module (already created). Commit messages must avoid the word "explanation".
- Keep commits atomic: one logical change per commit (e.g., add entities, add service, add tests).

Safety and review
- After making changes run `dotnet build` and `dotnet test` locally. Fix any failing tests before creating a pull request.
- Include concise notes in the PR describing what was added and how to verify permissions.
