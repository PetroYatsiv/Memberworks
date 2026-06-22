using FluentValidation;

namespace MemberWorks.Application.Households.Commands.AddHouseholdRelationship;

public class AddHouseholdRelationshipCommandValidator : AbstractValidator<AddHouseholdRelationshipCommand>
{
    public AddHouseholdRelationshipCommandValidator()
    {
        RuleFor(x => x.HouseholdId).NotEmpty();
        RuleFor(x => x.FromMemberId).NotEmpty();
        RuleFor(x => x.ToMemberId).NotEmpty()
            .NotEqual(x => x.FromMemberId).WithMessage("A member cannot have a relationship with themselves.");
        RuleFor(x => x.Type).IsInEnum();
    }
}