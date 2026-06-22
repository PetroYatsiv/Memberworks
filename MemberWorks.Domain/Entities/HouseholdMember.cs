using MemberWorks.Domain.Common;

namespace MemberWorks.Domain.Entities;

public class HouseholdMember : AuditableEntity
{
    public Guid HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<HouseholdRelationship> RelationshipsFrom { get; set; } = new List<HouseholdRelationship>();

    public ICollection<HouseholdRelationship> RelationshipsTo { get; set; } = new List<HouseholdRelationship>();
}