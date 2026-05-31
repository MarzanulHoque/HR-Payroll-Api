using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.Application.UnitTests.Notifications;

public class NotificationsControllerTests
{
    [Fact]
    public async Task GetForRecipient_ReturnsNotifications_ForAuthenticatedUser()
    {
        var email = "user@example.com";
        var mockService = new Mock<INotificationService>();
        mockService.Setup(s => s.GetForRecipientAsync(email, 1, 25))
            .ReturnsAsync(new List<NotificationDto> { new NotificationDto(Guid.NewGuid(), "T", "B", email, false, DateTime.UtcNow) });

        var controller = new NotificationsController(mockService.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "Test"));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var result = await controller.GetForRecipient();
        var ok = Assert.IsType<ActionResult<List<NotificationDto>>>(result);
        var value = Assert.IsType<OkObjectResult>(ok.Result);
        var list = Assert.IsType<List<NotificationDto>>(value.Value);
        Assert.Single(list);
    }

    [Fact]
    public async Task MarkAsRead_ReturnsNoContent_WhenServiceSucceeds()
    {
        var email = "user@example.com";
        var id = Guid.NewGuid();
        var mockService = new Mock<INotificationService>();
        mockService.Setup(s => s.MarkAsReadAsync(id, email)).ReturnsAsync(true);

        var controller = new NotificationsController(mockService.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "Test"));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var result = await controller.MarkAsRead(id);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Create_CallsService_WithProvidedRecipient()
    {
        var mockService = new Mock<INotificationService>();
        mockService.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var controller = new NotificationsController(mockService.Object);
        var req = new CreateNotificationRequest("Hello", "Body", "recipient@example.com");

        var result = await controller.Create(req);

        Assert.IsType<NoContentResult>(result);
        mockService.Verify(s => s.CreateAsync("Hello", "Body", "recipient@example.com"), Times.Once);
    }
}
