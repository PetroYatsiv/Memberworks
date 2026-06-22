using MediatR;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Application.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;

public class GetUsersQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken ct)
    {
        return await db.Users
            .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
            .Select(u => new UserDto(u.Id, u.Email, u.FirstName, u.LastName, u.Role))
            .ToListAsync(ct);
    }
}