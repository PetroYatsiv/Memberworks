using MemberWorks.Domain.Entities;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Users.Dtos;

public record UserDto(Guid Id, string Email, string FirstName, string LastName, ApplicationRole Role)
{
    public static UserDto FromEntity(User u) => new(u.Id, u.Email, u.FirstName, u.LastName, u.Role);
}