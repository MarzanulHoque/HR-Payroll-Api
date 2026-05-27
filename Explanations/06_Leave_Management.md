# Leave Management API Flow

This module allows employees to submit leave requests (Sick, Vacation, etc.) and allows Managers or HR to approve or reject them.

## 1. Submitting A Leave Request
- **Objective:** An employee applies for leaves.
- **Endpoint:** `POST /api/v1/leaverequests`
- **Payload:**
  ```json
  {
    "employeeId": "guid-here",
    "leaveType": "Sick",
    "startDate": "2026-06-01T00:00:00Z",
    "endDate": "2026-06-05T00:00:00Z",
    "reason": "Doctor appointment"
  }
  ```
- **Validation**:
  - The employee must exist in the database.
  - The `StartDate` cannot be strictly after the `EndDate`.
- **Result:** The system creates a new `LeaveRequest` entity with a default `Status` of `"Pending"`.

## 2. Processing (Approving/Rejecting) A Leave Request
- **Objective:** A manager updates the status of a pending leave request.
- **Endpoint:** `PUT /api/v1/leaverequests/{id}/process`
- **Payload:** Just a simple string: `"Approved"` or `"Rejected"`
- **Validation:** 
  - Validates that the leave request exists via the `id`.
  - Ensures the string sent is strictly one of the allowed statuses.
- **Result:** The `Status` property on the `LeaveRequest` gets updated and saved to the database.

## 3. Viewing Leaves
- **Objective:** An admin dashboard can view who is on leave and who has requested leave.
- **Endpoint:** `GET /api/v1/leaverequests`
- **Result:** Returns all leave requests mapped out in a clean `LeaveRequestDto` containing the Employee's full name alongside the dates and status of the leave.
