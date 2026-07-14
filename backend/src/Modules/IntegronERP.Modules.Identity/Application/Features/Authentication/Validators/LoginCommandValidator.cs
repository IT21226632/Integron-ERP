using FluentValidation;
using IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty();
    }
}