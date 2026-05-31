using System;
using System.Threading.Tasks;

namespace HRMS.Application.Common.Interfaces;

public interface INotificationService
{
    Task CreateAsync(string title, string body, string recipient);
}
