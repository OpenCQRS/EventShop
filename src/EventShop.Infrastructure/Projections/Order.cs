namespace EventShop.Infrastructure.Projections;

public record Order(Guid OrderId, decimal TotalCost, DateTime PlacedOn);
