namespace Fakebook.BuildingBlocks.Application.Abstractions.Cores
{
    public interface ICorrelationIdProvider
    {
        Guid GetOrCreate();

        string GetOrCreateAsString();
    }
}