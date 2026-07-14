using FluentValidation;
using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Validators;

public class RegisterCompanyCommandValidator
    : AbstractValidator<RegisterCompanyCommand>
{
    public RegisterCompanyCommandValidator()
    {
        RuleFor(x => x.Request.CompanyName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Request.CompanyEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.CompanyPhone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");

        RuleFor(x => x.Request.ConfirmPassword)
            .Equal(x => x.Request.Password)
            .WithMessage("Passwords do not match.");
    }
}