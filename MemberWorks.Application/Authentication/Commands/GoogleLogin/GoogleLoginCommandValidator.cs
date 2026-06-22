using FluentValidation;

namespace MemberWorks.Application.Authentication.Commands.GoogleLogin;

public class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}