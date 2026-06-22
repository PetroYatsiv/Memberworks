using MediatR;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Application.Households.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Households.Queries.GetHouseHolds;

public record GetHouseholdsQuery : IRequest<IReadOnlyList<HouseholdSummaryDto>>;

public class GetHouseholdsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetHouseholdsQuery, IReadOnlyList<HouseholdSummaryDto>>
{
    public async Task<IReadOnlyList<HouseholdSummaryDto>> Handle(GetHouseholdsQuery request, CancellationToken ct)
    {
        return await db.Households
            .OrderBy(h => h.Name)
            .Select(h => new HouseholdSummaryDto(
                h.Id,
                h.Name,
                h.PrimaryUserId,
                h.PrimaryUser.FirstName + " " + h.PrimaryUser.LastName,
                h.Members.Count))
            .ToListAsync(ct);
    }
}