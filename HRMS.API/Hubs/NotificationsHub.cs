using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HRMS.API.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
    // Hub left intentionally minimal. Clients should listen to "ReceiveNotification" messages.
}
