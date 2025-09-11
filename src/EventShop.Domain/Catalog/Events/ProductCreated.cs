using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Catalog.Events;

[EventType("ProductCreated")]
public record ProductCreated(Guid ProductId, string Name, decimal Price);
