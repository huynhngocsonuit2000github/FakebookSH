using Fakebook.Auth.Application.Auth;
using FluentValidation;

namespace Fakebook.Auth.Api.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.FirstName)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("First name, last name, email, userName and password are required.")
            .MaximumLength(100)
            .WithErrorCode("validation_error")
            .WithMessage("First name must be 100 characters or fewer.");

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("First name, last name, email, userName and password are required.")
            .MaximumLength(100)
            .WithErrorCode("validation_error")
            .WithMessage("Last name must be 100 characters or fewer.");

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("First name, last name, email, userName and password are required.")
            .EmailAddress()
            .WithErrorCode("validation_error")
            .WithMessage("Email must be a valid email address with 320 characters or fewer.")
            .MaximumLength(320)
            .WithErrorCode("validation_error")
            .WithMessage("Email must be a valid email address with 320 characters or fewer.");

        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("First name, last name, email, userName and password are required.")
            .MaximumLength(64)
            .WithErrorCode("validation_error")
            .WithMessage("UserName must be 64 characters or fewer.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("First name, last name, email, userName and password are required.")
            .MinimumLength(8)
            .WithErrorCode("weak_password")
            .WithMessage("Password must be at least 8 characters.");
    }
}
