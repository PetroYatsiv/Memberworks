using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Application.Households.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Households.Queries.GetHouseHoldById;

public record GetHouseholdByIdQuery(Guid Id) : IRequest<HouseholdDetailDto>;

public class GetHouseholdByIdQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetHouseholdByIdQuery, HouseholdDetailDto>
{
    public async Task<HouseholdDetailDto> Handle(GetHouseholdByIdQuery request, CancellationToken ct)
    {
        var household = await db.Households
                            .Where(h => h.Id == request.Id)
                            .Select(h => new
                            {
                                h.Id,
                                h.Name,
                                h.PrimaryUserId,
                                Members = h.Members.Select(m => new HouseholdMemberDto(
                                    m.Id,
                                    m.UserId,
                                    m.User.FirstName + " " + m.User.LastName,
                                    m.User.Email,
                                    m.UserId == h.PrimaryUserId)).ToList(),
                                Relationships = h.Relationships.Select(r => new RelationshipDto(
                                    r.Id,
                                    r.FromMemberId,
                                    r.FromMember.User.FirstName + " " + r.FromMember.User.LastName,
                                    r.ToMemberId,
                                    r.ToMember.User.FirstName + " " + r.ToMember.User.LastName,
                                    r.Type,
                                    r.Type.ToString())).ToList()
                            })
                            .FirstOrDefaultAsync(ct)
                        ?? throw new NotFoundException(nameof(Domain.Entities.Household), request.Id);

        return new HouseholdDetailDto(
            household.Id, household.Name, household.PrimaryUserId,
            household.Members, household.Relationships);
    }
}