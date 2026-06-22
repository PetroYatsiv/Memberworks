using MemberWorks.Domain.Common;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Domain.Entities;

public class HouseholdRelationship : AuditableEntity
{
    public Guid HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
    
    public Guid FromMemberId { get; set; }
    public HouseholdMember FromMember { get; set; } = null!;
    
    public Guid ToMemberId { get; set; }
    public HouseholdMember ToMember { get; set; } = null!;
    
    public RelationshipType Type { get; set; }
    
}