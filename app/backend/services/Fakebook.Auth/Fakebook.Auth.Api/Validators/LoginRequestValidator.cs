using Fakebook.Auth.Application.Auth;
using FluentValidation;

namespace Fakebook.Auth.Api.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.EmailOrUserName)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Email/userName and password are required.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Email/userName and password are required.");
    }
}