namespace Fakebook.Bff.Api.Downstreams.Cores;

public sealed class DownstreamHttpException : Exception
{
    public DownstreamHttpException(
        int statusCode,
        string? responseBody,
        string? contentType)
        : base("Downstream service returned an error.")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        ContentType = contentType;
    }

    public int StatusCode { get; }

    public string? ResponseBody { get; }

    public string? ContentType { get; }
}
