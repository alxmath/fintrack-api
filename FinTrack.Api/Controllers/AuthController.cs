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
        Guid userId = Guid.Parse("b6e6a299-96a4-4de8-aa51-9e5041ade373");

        var token = tokenService.GenerateToken(userId);

        return Ok(new
        {
            AccessToken = token
        });
    }
}
