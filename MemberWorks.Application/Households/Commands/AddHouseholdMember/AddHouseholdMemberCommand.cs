using MediatR;

namespace MemberWorks.Application.Households.Commands.AddHouseholdMember;

public record AddHouseholdMemberCommand(Guid HouseholdId, Guid UserId) : IRequest<Guid>;
