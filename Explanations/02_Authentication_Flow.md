# Authentication Flow

We implemented JSON Web Token (JWT) authentication for security.

## How it works:
1. **Registration**: 
   - A user sends a `RegisterCommand` with Email, Password, etc.
   - We hash the password using `BCrypt` (meaning we never store plain text passwords).
   - We save the `User`.

2. **Login**: 
   - A user sends a `LoginCommand` with Email and Password.
   - We verify the password hash.
   - We generate an **Access Token** (short-lived, e.g., 60 minutes) and a **Refresh Token** (long-lived, e.g., 7 days).
   - The user uses the Access Token to call secured APIs by passing it in the HTTP Headers (`Authorization: Bearer <token>`).

3. **Refresh Token endpoint**:
   - Once the Access Token expires, the user can call `/api/v1/auth/refresh-token` with their valid Refresh Token to get a brand new Access Token without having to type their password again.

4. **Security**:
   - Controllers can be protected by adding the `[Authorize]` attribute above them. Only users passing a valid Access Token can access those endpoints (like the Employee CRUD operations).
