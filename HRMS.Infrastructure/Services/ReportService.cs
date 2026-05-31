using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;

namespace HRMS.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly Data.ApplicationDbContext _db;

    public ReportService(Data.ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<byte[]> GenerateEmployeesCsvAsync()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,FirstName,LastName,Email,Department,Designation,JoiningDate,IsActive");

        var list = _db.Employees.ToList();
        foreach (var e in list)
        {
            sb.AppendLine($"{e.Id},{e.FirstName},{e.LastName},{e.Email},{e.Department},{e.Designation},{e.JoiningDate.ToString("o", CultureInfo.InvariantCulture)},{e.IsActive}");
        }

        return await Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    public async Task<byte[]> GenerateAttendanceCsvAsync(string month)
    {
        var sb = new StringBuilder();
        sb.AppendLine("EmployeeId,Date,ClockIn,ClockOut,Status");

        var list = _db.AttendanceRecords.Where(a => a.Date.ToString("MMMM yyyy") == month).ToList();
        foreach (var a in list)
        {
            sb.AppendLine($"{a.EmployeeId},{a.Date:yyyy-MM-dd},{a.ClockInTime:HH:mm},{(a.ClockOutTime.HasValue ? a.ClockOutTime.Value.ToString("HH:mm") : string.Empty)},{a.Status}");
        }

        return await Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
    }
}
