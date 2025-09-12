namespace EventShop.Infrastructure.Projections;

public record ProductReadModel(Guid Id, string Name, decimal Price);
