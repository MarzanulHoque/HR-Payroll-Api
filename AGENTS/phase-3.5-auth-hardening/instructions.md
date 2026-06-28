Phase 3.5 — Auth Hardening

Goal
- Harden authentication flows: token rotation, revocation, logout, forgot/reset password, and email verification.

Scope
- Implement server-side refresh token rotation and revocation (blacklist/invalidation).
- Implement logout endpoint that revokes active refresh tokens for a user/session.
- Implement forgot-password and reset-password flows with short-lived tokens and email hooks.
- Add email verification flow for new accounts.
- Add unit tests and minimal integration tests for token lifecycle.

Constraints
- Keep changes backwards-compatible with existing JWT access token flows.
- Add new DB table or columns only if idempotent and migrated via EnsureCreated/EF Migrations pattern.

Deliverables
- Application handlers/commands for Logout, Refresh rotation, ForgotPassword, ResetPassword, VerifyEmail.
- Infrastructure service `IRefreshTokenService` + concrete implementation.
- Unit tests for each handler and tests for rotation/revocation.
- Postman collection updates and README notes.

Implementation notes
- Start by creating `feature/v1.0.1-auth-hardening` branch (done).
- Scaffold `HRMS.Application` handlers and `HRMS.Infrastructure` token service.
- Implement database flags for `RefreshToken` (`IsRevoked`, `ReplacedByToken`, `RevokedAt`).
- Add tests before implementing infrastructure details where possible.

Next step: implement Logout + refresh-token revocation and add tests.
