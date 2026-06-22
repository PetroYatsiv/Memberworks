using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberWorks.Persistence.Configurations;

public class HouseholdRelationshipConfiguration : IEntityTypeConfiguration<HouseholdRelationship>
{
    public void Configure(EntityTypeBuilder<HouseholdRelationship> builder)
    {
        builder.Property(r => r.Type).HasConversion<string>().HasMaxLength(32);
        
        builder.HasIndex(r => new { r.FromMemberId, r.ToMemberId, r.Type }).IsUnique();

        builder.HasOne(r => r.Household)
            .WithMany(h => h.Relationships)
            .HasForeignKey(r => r.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict on both endpoints to avoid multiple cascade paths through the household.
        builder.HasOne(r => r.FromMember)
            .WithMany(m => m.RelationshipsFrom)
            .HasForeignKey(r => r.FromMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ToMember)
            .WithMany(m => m.RelationshipsTo)
            .HasForeignKey(r => r.ToMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}