using Fakebook.Auth.Application.Auth;
using FluentValidation;

namespace Fakebook.Auth.Api.Validators;

public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithErrorCode("validation_error")
            .WithMessage("Refresh token is required.");
    }
}
