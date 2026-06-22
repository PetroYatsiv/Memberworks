using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Households.Commands.AddHouseholdRelationship;

public class AddHouseholdRelationshipCommandHandler(IApplicationDbContext db)
    : IRequestHandler<AddHouseholdRelationshipCommand, Guid>
{
    public async Task<Guid> Handle(AddHouseholdRelationshipCommand request, CancellationToken ct)
    {
        var household = await db.Households
                            .Include(h => h.Members)
                            .FirstOrDefaultAsync(h => h.Id == request.HouseholdId, ct)
                        ?? throw new NotFoundException(nameof(Household), request.HouseholdId);
        
        var memberIds = household.Members.Select(m => m.Id).ToHashSet();
        if (!memberIds.Contains(request.FromMemberId))
            throw new NotFoundException(nameof(HouseholdMember), request.FromMemberId);
        if (!memberIds.Contains(request.ToMemberId))
            throw new NotFoundException(nameof(HouseholdMember), request.ToMemberId);

        var relationship = new HouseholdRelationship
        {
            HouseholdId = household.Id,
            FromMemberId = request.FromMemberId,
            ToMemberId = request.ToMemberId,
            Type = request.Type
        };

        db.HouseholdRelationships.Add(relationship);
        await db.SaveChangesAsync(ct);
        return relationship.Id;
    }
}