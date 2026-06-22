namespace MemberWorks.Application.Organizations.Dtos;

public record OrganizationDto(Guid Id, string Name, int UserCount, int HouseholdCount);