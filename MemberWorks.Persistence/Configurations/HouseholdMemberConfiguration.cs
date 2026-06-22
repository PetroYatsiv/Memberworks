using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberWorks.Persistence.Configurations;

public class HouseholdMemberConfiguration : IEntityTypeConfiguration<HouseholdMember>
{
    public void Configure(EntityTypeBuilder<HouseholdMember> builder)
    {
        builder.HasIndex(m => new { m.HouseholdId, m.UserId }).IsUnique();

        builder.HasOne(m => m.Household)
            .WithMany(h => h.Members)
            .HasForeignKey(m => m.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.User)
            .WithMany(u => u.HouseholdMemberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}