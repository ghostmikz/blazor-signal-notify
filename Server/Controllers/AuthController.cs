using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = UserService.Authenticate(request.Username, request.Password);
        if (user == null) return Unauthorized();

        return Ok(user);
    }
}

public record LoginRequest(string Username, string Password);