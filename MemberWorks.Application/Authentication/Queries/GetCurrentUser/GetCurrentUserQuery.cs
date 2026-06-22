using MediatR;
using MemberWorks.Application.Authentication.Dtos;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Authentication.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<CurrentUserDto>;

public class GetCurrentUserQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var userId = currentUser.UserId ?? throw new ForbiddenAccessException();

        var user = await db.Users
                       .Include(u => u.Organization)
                       .FirstOrDefaultAsync(u => u.Id == userId, ct)
                   ?? throw new NotFoundException(nameof(Domain.Entities.User), userId);

        return CurrentUserDto.FromEntity(user, user.Organization.Name);
    }
}