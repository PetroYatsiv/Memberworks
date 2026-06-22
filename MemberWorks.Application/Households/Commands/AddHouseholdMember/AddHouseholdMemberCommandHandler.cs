using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Households.Commands.AddHouseholdMember;

public class AddHouseholdMemberCommandHandler(IApplicationDbContext db)
    : IRequestHandler<AddHouseholdMemberCommand, Guid>
{
    public async Task<Guid> Handle(AddHouseholdMemberCommand request, CancellationToken ct)
    {
        var household = await db.Households
                            .Include(h => h.Members)
                            .FirstOrDefaultAsync(h => h.Id == request.HouseholdId, ct)
                        ?? throw new NotFoundException(nameof(Household), request.HouseholdId);

        var userExists = await db.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            throw new NotFoundException(nameof(User), request.UserId);

        if (household.Members.Any(m => m.UserId == request.UserId))
            throw new ForbiddenAccessException("That user is already a member of this household.");

        var member = new HouseholdMember
        {
            HouseholdId = household.Id,
            UserId = request.UserId
        };

        db.HouseholdMembers.Add(member);
        await db.SaveChangesAsync(ct);
        return member.Id;
    }
}