using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gudena.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var userId = User.FindFirst("uid")?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var userType = User.FindFirst("userType")?.Value;

        return Ok(new { userId, email, userType });
    }
}