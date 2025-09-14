namespace EventShop.Infrastructure.Projections;

public record Product(Guid Id, string Name, string Description, decimal Price);
