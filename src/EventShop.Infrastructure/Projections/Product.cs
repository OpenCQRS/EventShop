namespace EventShop.Infrastructure.Projections;

public record Product(Guid ProductId, string Name, string Description, decimal Price);
