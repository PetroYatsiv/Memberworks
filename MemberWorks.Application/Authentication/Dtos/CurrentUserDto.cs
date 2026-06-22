using MemberWorks.Domain.Entities;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Authentication.Dtos;

public record CurrentUserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    Guid OrganizationId,
    string OrganizationName,
    ApplicationRole Role)
{
    public static CurrentUserDto FromEntity(User user, string organizationName) => new(
        user.Id, user.Email, user.FirstName, user.LastName,
        user.OrganizationId, organizationName, user.Role);
}