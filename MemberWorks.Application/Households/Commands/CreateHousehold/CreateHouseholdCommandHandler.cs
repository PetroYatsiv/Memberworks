using MediatR;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Households.Commands.CreateHousehold;

public class CreateHouseholdCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    : IRequestHandler<CreateHouseholdCommand, Guid>
{
    public async Task<Guid> Handle(CreateHouseholdCommand request, CancellationToken ct)
    {
        var organizationId = currentUser.OrganizationId ?? throw new ForbiddenAccessException();

        // The primary user must exist in the caller's organization (tenant filter guarantees scope).
        var primaryUserExists = await db.Users.AnyAsync(u => u.Id == request.PrimaryUserId, ct);
        if (!primaryUserExists)
            throw new NotFoundException(nameof(User), request.PrimaryUserId);

        var household = new Household
        {
            OrganizationId = organizationId,
            Name = request.Name,
            PrimaryUserId = request.PrimaryUserId
        };

        // Invariant: the primary user is always a member of their own household.
        household.Members.Add(new HouseholdMember
        {
            HouseholdId = household.Id,
            UserId = request.PrimaryUserId
        });

        db.Households.Add(household);
        await db.SaveChangesAsync(ct);
        return household.Id;
    }
}