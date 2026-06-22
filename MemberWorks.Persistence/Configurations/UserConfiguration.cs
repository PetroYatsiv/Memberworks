using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberWorks.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.Property(u => u.GoogleSubjectId).HasMaxLength(256);
        
        builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(32);
        
        builder.HasIndex(u => new { u.OrganizationId, u.Email }).IsUnique();
        builder.HasIndex(u => u.GoogleSubjectId).IsUnique();
        
        builder.HasOne(u => u.Organization)
            .WithMany(o => o.Users)
            .HasForeignKey(u => u.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}