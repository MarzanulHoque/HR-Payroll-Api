# Phase 3.5 Auth Hardening Implementation Plan

Overview
- Branch: `feature/v1.0.1-auth-hardening`
- Goals: token rotation, revocation, logout, password reset, email verification, tests, docs.

Milestones
1. Logout and refresh token revocation/rotation
2. Forgot-password and reset-password handlers + email token backend
3. Email verification flow
4. Tests: unit + integration for token lifecycle
5. Postman updates and changelog

Notes
- Prefer idempotent DB changes; use `EnsureCreated` or EF migrations when necessary.
- Break work into small commits; run `dotnet build` and `dotnet test` after each handler.
