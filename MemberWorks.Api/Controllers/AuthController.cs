using MemberWorks.Application.Authentication.Commands.GoogleLogin;
using MemberWorks.Application.Authentication.Dtos;
using MemberWorks.Application.Authentication.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberWorks.Api.Controllers;

public class AuthController : ApiControllerBase
{
    /// <summary>Exchange a Google ID token for an application JWT (creates the user/org on first sign-in).</summary>
    [AllowAnonymous]
    [HttpPost("google")]
    public async Task<ActionResult<AuthResponse>> Google([FromBody] GoogleLoginRequest request)
        => Ok(await Mediator.Send(new GoogleLoginCommand(request.IdToken)));

    /// <summary>Returns the currently authenticated user.</summary>
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> Me()
        => Ok(await Mediator.Send(new GetCurrentUserQuery()));

    public record GoogleLoginRequest(string IdToken);
}