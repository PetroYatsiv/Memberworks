using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Enums;
using MemberWorks.Infrastructure.Authentication;

namespace MemberWorks.Api.Common;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId => ParseGuid(User?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                                     ?? User?.FindFirstValue(ClaimTypes.NameIdentifier));

    public Guid? OrganizationId => ParseGuid(User?.FindFirstValue(JwtTokenService.OrganizationClaim));

    public ApplicationRole? Role =>
        Enum.TryParse<ApplicationRole>(User?.FindFirstValue(ClaimTypes.Role), out var role) ? role : null;

    private static Guid? ParseGuid(string? value) =>
        Guid.TryParse(value, out var guid) ? guid : null;
}