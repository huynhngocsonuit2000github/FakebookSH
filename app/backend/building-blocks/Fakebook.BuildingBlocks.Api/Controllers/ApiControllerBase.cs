using Fakebook.BuildingBlocks.Application.Common.Results;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.BuildingBlocks.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    protected ApiControllerBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected async Task<IActionResult> ExecuteAndValidatorAsync<TRequest, TResponse>(TRequest request, Func<Task<Result<TResponse>>> action, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return ToValidationActionResult(validationResult);
            }
        }

        var result = await action();
        return ToActionResult(result);
    }

    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        var body = new
        {
            code = result.Error.Code,
            message = result.Error.Message
        };

        return result.Error.Code switch
        {
            "validation_error" => BadRequest(body),
            "weak_password" => BadRequest(body),
            "user_already_exists" => Conflict(body),
            "invalid_credentials" => Unauthorized(body),
            "invalid_refresh_token" => Unauthorized(body),
            "user_not_found" => NotFound(body),
            _ => Problem(result.Error.Message)
        };
    }

    protected IActionResult ToValidationActionResult(ValidationResult result)
    {
        var error = result.Errors.FirstOrDefault();

        var body = new
        {
            code = error?.ErrorCode ?? "validation_error",
            message = error?.ErrorMessage ?? "Validation failed."
        };

        return BadRequest(body);
    }
}
