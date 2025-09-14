namespace EventShop.Infrastructure.Projections;

public record Order(Guid OrderId, decimal TotalCost, DateTime PlacedOn, OrderItem[] Items);
public record OrderItem(Guid ProductId, int Quantity, decimal UnitPrice);
