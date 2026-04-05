using FinTrack.Api.Common.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(JwtTokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        // fake user (por enquanto)
        var userId = Guid.NewGuid();

        var token = tokenService.GenerateToken(userId);

        return Ok(new
        {
            access_token = token
        });
    }
}
