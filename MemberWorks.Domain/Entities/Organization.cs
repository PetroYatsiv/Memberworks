using MemberWorks.Domain.Common;

namespace MemberWorks.Domain.Entities;

public class Organization : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Household> Households { get; set; } = new List<Household>();
}