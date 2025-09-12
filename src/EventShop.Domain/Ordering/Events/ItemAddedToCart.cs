using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Ordering.Events;

[EventType("ItemAddedToCart")]
public record ItemAddedToCart(Guid ShoppingCartId, Guid ProductId, int Quantity);
