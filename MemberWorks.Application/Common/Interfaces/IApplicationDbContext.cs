using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Organization> Organizations { get; }
    DbSet<User> Users { get; }
    DbSet<Household> Households { get; }
    DbSet<HouseholdMember> HouseholdMembers { get; }
    DbSet<HouseholdRelationship> HouseholdRelationships { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}