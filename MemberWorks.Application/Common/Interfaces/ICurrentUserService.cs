using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? OrganizationId { get; }
    ApplicationRole? Role { get; }
    bool IsAuthenticated { get; }
}