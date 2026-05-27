# Dashboard & Reporting API Flow

This module serves as the primary data aggregator for the frontend dashboard application. It provides real-time reporting metrics necessary for Management and HR overviews.

## 1. Fetching Top-Level Metrics
- **Objective:** Give the frontend an immediate overview of system activity upon login.
- **Endpoint:** `GET /api/v1/dashboard/metrics`
- **What happens under the hood:**
  - The `GetDashboardMetricsQueryHandler` asynchronously triggers multiple Count operations via Entity Framework Core against our SQLite tables.
  - **TotalEmployees:** Counts all existing employees registered.
  - **PendingLeaveRequests:** Computes all leaves strictly marked with a "Pending" status across the company.
  - **TotalAttendanceToday:** Filters the `AttendanceRecords` based on today's localized UTC date bounds to see how many people have clocked in.
- **Result:** It returns a cohesive `DashboardMetricsDto` holding the integer values for use in reporting charts or cards on the frontend.
