using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser): IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var organizationId = currentUser.OrganizationId ?? throw new ForbiddenAccessException();

        var emailTaken = await db.Users.AnyAsync(u => u.Email == request.Email, ct);
        if (emailTaken)
            throw new ForbiddenAccessException($"A user with email '{request.Email}' already exists.");

        var user = new User
        {
            OrganizationId = organizationId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user.Id;
    }
}