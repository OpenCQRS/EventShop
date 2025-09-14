namespace EventShop.Infrastructure.Projections;

public record ShoppingCart(Guid ShoppingCartId, List<ShoppingCartItem> Items);
public record ShoppingCartItem(Guid ProductId, int Quantity);
