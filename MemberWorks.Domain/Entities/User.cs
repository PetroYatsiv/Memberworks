using MemberWorks.Domain.Common;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Domain.Entities;

public class User : AuditableEntity, ITenantScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? GoogleSubjectId { get; set; }

    public ApplicationRole Role { get; set; } = ApplicationRole.Member;

    public ICollection<HouseholdMember> HouseholdMemberships { get; set; } = new List<HouseholdMember>();

    public string FullName => $"{FirstName} {LastName}".Trim();
}
    
