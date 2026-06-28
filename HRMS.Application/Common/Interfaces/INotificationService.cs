using System;
using System.Threading.Tasks;

namespace HRMS.Application.Common.Interfaces;

using HRMS.Application.Common.Models;

 public interface INotificationService
{
    Task CreateAsync(string title, string body, string recipient);
    Task<List<NotificationDto>> GetForRecipientAsync(string recipient, int page = 1, int pageSize = 25);
    Task<bool> MarkAsReadAsync(Guid id, string recipient);
}
