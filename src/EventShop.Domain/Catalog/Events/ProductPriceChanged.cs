using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Catalog.Events;

[EventType("ProductPriceChanged")]
public record ProductPriceChanged(Guid Id, decimal NewPrice) : IEvent;
