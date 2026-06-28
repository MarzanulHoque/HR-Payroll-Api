using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace HRMS.API.SignalR;

public class EmailUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Use email as the SignalR user identifier so we can target clients by email
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}
