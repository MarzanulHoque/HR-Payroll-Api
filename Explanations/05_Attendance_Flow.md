# Attendance API Flow

This application allows employees to clock in and clock out to track their working hours. 

## How it works:

### 1. Clocking In
- **Objective:** Record the time an employee starts working.
- **Endpoint:** `POST /api/v1/attendance/clock-in`
- **Validation Rules:**
  - The `EmployeeId` passed in the request must belong to a real Employee in the database.
  - The employee **cannot** clock in if they are already clocked in for the day (we check if there's a record for today where `ClockOutTime` is null).
- **Result:** We create a new `AttendanceRecord` tying the employee to the current UTC date and `ClockInTime`.

### 2. Clocking Out
- **Objective:** Record the time an employee stops working.
- **Endpoint:** `POST /api/v1/attendance/clock-out`
- **Validation Rules:**
  - The system searches for the employee's active record (a record for today where `ClockOutTime` is still null).
  - If a record is found, it updates the `ClockOutTime` to the current UTC time.
  - If they never clocked in today or they already clocked out, it returns an error.

By ensuring users pass a valid **JWT Token** (using the `[Authorize]` attribute on the controller), we make sure only registered members of the HRMS system can hit these endpoints.
