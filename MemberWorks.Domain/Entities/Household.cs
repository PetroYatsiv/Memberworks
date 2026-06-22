using MemberWorks.Domain.Common;

namespace MemberWorks.Domain.Entities;

public class Household : AuditableEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    
    public Guid PrimaryUserId { get; set; }
    public User PrimaryUser { get; set; } = null!;
    
    public ICollection<HouseholdMember> Members { get; set; } = new List<HouseholdMember>();
    public ICollection<HouseholdRelationship> Relationships { get; set; } = new List<HouseholdRelationship>();
}