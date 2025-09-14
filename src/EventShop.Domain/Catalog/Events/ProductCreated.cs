using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Catalog.Events;

[EventType("ProductCreated")]
public record ProductCreated(Guid Id, string Name, string Description, decimal Price) : IEvent;
