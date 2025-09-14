using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Catalog.Streams;

public record ProductStreamId(Guid ProductId) : IStreamId
{
    public string Id => $"product:{ProductId}";
}
