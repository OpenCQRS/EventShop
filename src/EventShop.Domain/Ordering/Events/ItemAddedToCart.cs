using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Ordering.Events;

[EventType("ItemAddedToCart")]
public record ItemAddedToCart(Guid CartId, Guid ProductId, int Quantity);
