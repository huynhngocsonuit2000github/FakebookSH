using Fakebook.BuildingBlocks.Application.Abstractions.Clock;

namespace Fakebook.BuildingBlocks.Infrastructure.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}