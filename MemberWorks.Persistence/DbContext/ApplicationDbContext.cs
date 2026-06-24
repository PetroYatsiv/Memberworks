using System.Reflection;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Common;
using MemberWorks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Persistence.Migrations;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUser;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }
    
    public Guid? TenantId => _currentUser.OrganizationId;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Household> Households => Set<Household>();
    public DbSet<HouseholdMember> HouseholdMembers => Set<HouseholdMember>();
    public DbSet<HouseholdRelationship> HouseholdRelationships => Set<HouseholdRelationship>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Multi-tenancy: every ITenantScoped entity is automatically filtered to the caller's org.
        // The household's children carry the same filter (via their household) so direct DbSet
        // queries on them can't leak across tenants and EF stays consistent about cascading filters.
        modelBuilder.Entity<User>().HasQueryFilter(e => e.OrganizationId == TenantId);
        modelBuilder.Entity<Household>().HasQueryFilter(e => e.OrganizationId == TenantId);
        modelBuilder.Entity<HouseholdMember>().HasQueryFilter(e => e.Household.OrganizationId == TenantId);
        modelBuilder.Entity<HouseholdRelationship>().HasQueryFilter(e => e.Household.OrganizationId == TenantId);

        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampAudit();
        StampTenant();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void StampAudit()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAtUtc = now;
            else if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedAtUtc = now;
        }
    }
    
    private void StampTenant()
    {
        if (TenantId is null) return;

        foreach (var entry in ChangeTracker.Entries<ITenantScoped>())
        {
            if (entry.State == EntityState.Added && entry.Entity.OrganizationId == Guid.Empty)
                entry.Entity.OrganizationId = TenantId.Value;
        }
    }
}