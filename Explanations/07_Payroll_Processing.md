# Payroll Processing Flow

This module is responsible for generating salary slips for employees on a monthly basis.

## 1. Generating Payroll (Salary Slips)
- **Objective:** HR or Admin generates a salary slip for a specific employee and a specific month.
- **Endpoint:** `POST /api/v1/payroll/generate`
- **Payload:**
  ```json
  {
    "employeeId": "guid-here",
    "month": "May 2026",
    "baseSalary": 5000.00,
    "deductions": 200.00
  }
  ```
- **Business Rules:**
  - Validates that the `EmployeeId` exists.
  - Checks if a `SalarySlip` has already been generated for that exact employee in that exact `Month`. If so, it fails (prevents duplicate payments).
  - Automatically calculates the **NetPay** (`BaseSalary` minus `Deductions`).
- **Result:** Saves a `SalarySlip` entity into the database with `Status = "Generated"`.

## 2. Viewing Salary Slips
- **Objective:** Retrieve all generated salary slips.
- **Endpoint:** `GET /api/v1/payroll/slips`
- **Result:** Provides a mapped list of all salary slips alongside the Employee's Name and exact calculated Net Pay.
