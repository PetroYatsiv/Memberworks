using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberWorks.Persistence.Configurations;

public class HouseholdConfiguration : IEntityTypeConfiguration<Household>
{
    public void Configure(EntityTypeBuilder<Household> builder)
    {
        builder.Property(h => h.Name).IsRequired().HasMaxLength(200);

        builder.HasOne(h => h.Organization)
            .WithMany(o => o.Households)
            .HasForeignKey(h => h.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict so the primary user FK doesn't create a second cascade path into Users.
        builder.HasOne(h => h.PrimaryUser)
            .WithMany()
            .HasForeignKey(h => h.PrimaryUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}