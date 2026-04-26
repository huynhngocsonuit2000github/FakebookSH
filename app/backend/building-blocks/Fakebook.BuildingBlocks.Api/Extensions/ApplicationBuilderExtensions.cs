using Fakebook.BuildingBlocks.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Fakebook.BuildingBlocks.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFakebookExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}