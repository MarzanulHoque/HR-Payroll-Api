# Changelog

## Unreleased

### Security
- Store refresh tokens as SHA-256 hashes at rest (database column `TokenHash`).
- Track rotated refresh tokens via `ReplacedByTokenHash` and revoke sessions on reuse detection.

### Fixes
- Hardened database seeding to avoid KeyNotFoundException during tests.

### Features
- CSV bulk employee import endpoint: `POST api/v1/employees/import` (admin/HR-only expected).

## Migration notes
- EF migration `RefreshToken_Hash` added. Before deploying to production, plan token rotation/invalidation because existing tokens are not reversible to hashes.
