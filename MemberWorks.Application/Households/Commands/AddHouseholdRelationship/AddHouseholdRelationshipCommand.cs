using MediatR;
using MemberWorks.Domain.Enums;

namespace MemberWorks.Application.Households.Commands.AddHouseholdRelationship;

public record AddHouseholdRelationshipCommand(
    Guid HouseholdId,
    Guid FromMemberId,
    Guid ToMemberId,
    RelationshipType Type) : IRequest<Guid>;