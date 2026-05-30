using HRMS.API.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminTestController : ControllerBase
{
    [HttpGet("test-protected")]
    [PermissionAuthorize("payroll.generate")]
    public IActionResult TestProtected()
    {
        return Ok(new { message = "protected" });
    }
}
