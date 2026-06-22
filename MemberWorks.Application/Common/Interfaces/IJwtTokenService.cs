using MemberWorks.Domain.Entities;

namespace MemberWorks.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user);
}