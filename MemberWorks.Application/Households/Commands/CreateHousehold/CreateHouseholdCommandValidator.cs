using FluentValidation;

namespace MemberWorks.Application.Households.Commands.CreateHousehold;

public class CreateHouseholdCommandValidator : AbstractValidator<CreateHouseholdCommand>
{
    public CreateHouseholdCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PrimaryUserId).NotEmpty();
    }
}