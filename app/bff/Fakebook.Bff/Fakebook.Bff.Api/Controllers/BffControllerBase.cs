using Fakebook.Bff.Api.Downstreams.Cores;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.Bff.Api.Controllers;

public abstract class BffControllerBase : ControllerBase
{
    protected async Task<IActionResult> ExecuteDownstreamAsync<TResponse>(Func<Task<TResponse>> downstreamCall)
    {
        try
        {
            var response = await downstreamCall();
            return Ok(response);
        }
        catch (DownstreamHttpException exception)
        {
            return ToDownstreamErrorResult(exception);
        }
    }

    protected async Task<IActionResult> ExecuteDownstreamAsync(Func<Task<IActionResult>> downstreamCall)
    {
        try
        {
            return await downstreamCall();
        }
        catch (DownstreamHttpException exception)
        {
            return ToDownstreamErrorResult(exception);
        }
    }

    private ContentResult ToDownstreamErrorResult(DownstreamHttpException exception)
    {
        return new ContentResult
        {
            Content = string.IsNullOrWhiteSpace(exception.ResponseBody)
                ? "Downstream service returned an error."
                : exception.ResponseBody,
            ContentType = exception.ContentType ?? "application/json",
            StatusCode = exception.StatusCode
        };
    }
}
