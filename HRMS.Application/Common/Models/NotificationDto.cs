using System;

namespace HRMS.Application.Common.Models;

public record NotificationDto(Guid Id, string Title, string Body, string Recipient, bool IsRead, DateTime CreatedAt);
