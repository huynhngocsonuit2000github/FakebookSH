using Fakebook.Auth.Application.Auth;
using FluentValidation;

namespace Fakebook.Auth.Api.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Email, userName and password are required.")
            .EmailAddress()
            .WithErrorCode("validation_error")
            .WithMessage("Email must be a valid email address with 320 characters or fewer.")
            .MaximumLength(320)
            .WithErrorCode("validation_error")
            .WithMessage("Email must be a valid email address with 320 characters or fewer.");

        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Email, userName and password are required.")
            .MaximumLength(64)
            .WithErrorCode("validation_error")
            .WithMessage("UserName must be 64 characters or fewer.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Email, userName and password are required.")
            .MinimumLength(8)
            .WithErrorCode("weak_password")
            .WithMessage("Password must be at least 8 characters.");
    }
}
