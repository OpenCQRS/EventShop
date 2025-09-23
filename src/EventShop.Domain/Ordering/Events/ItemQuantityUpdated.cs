using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Ordering.Events;

[EventType("ItemQuantityUpdated")]
public record ItemQuantityUpdated(Guid ShoppingCartId, Guid ProductId, int NewQuantity) : IEvent;
