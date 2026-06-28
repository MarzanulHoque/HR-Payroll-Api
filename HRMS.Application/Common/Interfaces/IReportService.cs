using System.Threading.Tasks;

namespace HRMS.Application.Common.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateEmployeesCsvAsync();
    Task<byte[]> GenerateAttendanceCsvAsync(string month);
}
