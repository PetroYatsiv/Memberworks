using MediatR;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string FirstName, string LastName, ApplicationRole Role)
    : IRequest<Guid>;