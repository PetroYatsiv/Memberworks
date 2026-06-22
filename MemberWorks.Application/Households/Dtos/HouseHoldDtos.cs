using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Households.Dtos;

public record HouseholdSummaryDto(Guid Id, string Name, Guid PrimaryUserId, string PrimaryUserName, int MemberCount);

public record HouseholdMemberDto(Guid MemberId, Guid UserId, string FullName, string Email, bool IsPrimary);

public record RelationshipDto(
    Guid Id,
    Guid FromMemberId,
    string FromName,
    Guid ToMemberId,
    string ToName,
    RelationshipType Type,
    string TypeLabel);

public record HouseholdDetailDto(
    Guid Id,
    string Name,
    Guid PrimaryUserId,
    IReadOnlyList<HouseholdMemberDto> Members,
    IReadOnlyList<RelationshipDto> Relationships);