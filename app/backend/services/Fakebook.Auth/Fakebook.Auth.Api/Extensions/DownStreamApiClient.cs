namespace Fakebook.Auth.Api.Extensions
{
    public static class DownStreamApiClient
    {
        public static IServiceCollection AddDownStreamApiClientWithCorrelationIdHandler(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Add downstream API clients with CorrelationIdDelegatingHandler
            //services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration["Services:User"]!);
            //})
            //.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

            //services.AddHttpClient<IPostApiClient, PostApiClient>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration["Services:Post"]!);
            //})
            //.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

            return services;
        }
    }
}