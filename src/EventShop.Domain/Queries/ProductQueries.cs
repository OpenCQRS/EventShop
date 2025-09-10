using OpenCqrs.Queries;

namespace EventShop.Domain.Queries;

public class GetAllProductsQuery : IQuery<List<ProductDto>>
{
}

public class GetProductByIdQuery : IQuery<ProductDto?>
{
    public Guid Id { get; set; }
}

public class GetProductsByCategoryQuery : IQuery<List<ProductDto>>
{
    public string Category { get; set; } = string.Empty;
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}