using FinTrack.Application.Common.Interfaces;
using System.Security.Claims;

namespace FinTrack.Api.Common.Auth;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            var claim = (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value) 
                ?? throw new UnauthorizedAccessException("Usuário não autenticado.");

            return Guid.Parse(claim);
        }
    }
}
