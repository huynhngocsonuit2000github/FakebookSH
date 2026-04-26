using Fakebook.Auth.Api.Validators;
using Fakebook.Auth.Application.Auth;
using FluentValidation;

namespace Fakebook.Auth.Api.Extensions
{
    public static class ValidatorRegistration
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddSingleton<IValidator<LogoutRequest>, LogoutRequestValidator>();
            services.AddSingleton<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
            services.AddSingleton<IValidator<RegisterRequest>, RegisterRequestValidator>();

            return services;
        }
    }
}