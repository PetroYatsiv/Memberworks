using MediatR;

namespace MemberWorks.Application.Households.Commands.CreateHousehold;

public record CreateHouseholdCommand(string Name, Guid PrimaryUserId) : IRequest<Guid>;