using Fakebook.BuildingBlocks.Api.Cores;
using Fakebook.BuildingBlocks.Api.Handlers;
using Fakebook.BuildingBlocks.Api.Middleware;
using Fakebook.BuildingBlocks.Application.Abstractions.Cores;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.BuildingBlocks.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    #region Add service

    public static IServiceCollection AddFakebookCorrelationIdProvider(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();
        return services;
    }

    public static IServiceCollection AddFakebookCorrelationIdDelegatingHandler(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<CorrelationIdDelegatingHandler>();

        return services;
    }

    #endregion Add service

    #region Use service

    public static IApplicationBuilder UseFakebookCorrelationId(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseFakebookExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }

    #endregion Use service
}