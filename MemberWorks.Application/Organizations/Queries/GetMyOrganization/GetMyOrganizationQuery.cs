using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Application.Organizations.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Organizations.Queries.GetMyOrganization;

public record GetMyOrganizationQuery : IRequest<OrganizationDto>;

public class GetMyOrganizationQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<GetMyOrganizationQuery, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(GetMyOrganizationQuery request, CancellationToken ct)
    {
        var orgId = currentUser.OrganizationId ?? throw new ForbiddenAccessException();

        var dto = await db.Organizations
                      .Where(o => o.Id == orgId)
                      .Select(o => new OrganizationDto(o.Id, o.Name, o.Users.Count, o.Households.Count))
                      .FirstOrDefaultAsync(ct)
                  ?? throw new NotFoundException(nameof(Domain.Entities.Organization), orgId);

        return dto;
    }
}