using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("employees/csv")]
    public async Task<IActionResult> EmployeesCsv()
    {
        var csv = await _reportService.GenerateEmployeesCsvAsync();
        return File(csv, "text/csv", "employees.csv");
    }

    [HttpGet("attendance/csv")]
    public async Task<IActionResult> AttendanceCsv([FromQuery] string month)
    {
        var csv = await _reportService.GenerateAttendanceCsvAsync(month);
        var fileName = $"attendance_{month.Replace(' ', '_')}.csv";
        return File(csv, "text/csv", fileName);
    }
}
