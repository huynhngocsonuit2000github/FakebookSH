namespace Fakebook.BuildingBlocks.Application.Common.Errors;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string message)
    {
        return new Error("NotFound", message);
    }

    public static Error Validation(string message)
    {
        return new Error("Validation", message);
    }

    public static Error Unauthorized(string message)
    {
        return new Error("Unauthorized", message);
    }

    public static Error Conflict(string message)
    {
        return new Error("Conflict", message);
    }

    public static Error Failure(string message)
    {
        return new Error("Failure", message);
    }
}