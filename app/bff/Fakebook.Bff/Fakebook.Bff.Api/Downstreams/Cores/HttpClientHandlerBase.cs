using System.Net.Http.Json;

namespace Fakebook.Bff.Api.Downstreams.Cores;

public abstract class HttpClientHandlerBase
{
    private readonly HttpClient _httpClient;

    protected HttpClientHandlerBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<TResponse> GetForJsonAsync<TResponse>(string requestUri, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);

        return await ReadRequiredJsonAsync<TResponse>(response, cancellationToken);
    }

    protected async Task<TResponse> PostForJsonAsync<TRequest, TResponse>(string requestUri, TRequest request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);

        return await ReadRequiredJsonAsync<TResponse>(response, cancellationToken);
    }

    protected async Task PostAsync<TRequest>(string requestUri, TRequest request, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var contentType = response.Content.Headers.ContentType?.ToString();

        throw new DownstreamHttpException((int)response.StatusCode, body, contentType);
    }

    private static async Task<TResponse> ReadRequiredJsonAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var value = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

        return value ?? throw new InvalidOperationException($"Expected a non-empty JSON response body for {typeof(TResponse).Name}.");
    }
}
