using OpenCqrs.Queries;
using OpenCqrs.Domain;
using EventShop.Domain.Queries;
using EventShop.Domain.Entities;

namespace EventShop.Infrastructure.QueryHandlers;

public class ProductQueryHandlers : 
    IQueryHandler<GetAllProductsQuery, List<ProductDto>>,
    IQueryHandler<GetProductByIdQuery, ProductDto?>,
    IQueryHandler<GetProductsByCategoryQuery, List<ProductDto>>
{
    private readonly IRepository<Product> _repository;
    // Simple in-memory cache for demonstration - in production use proper read models
    private static readonly List<ProductDto> _productCache = new();

    public ProductQueryHandlers(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery query)
    {
        // Return cached products for demonstration
        // In a real implementation, this would query a read model or projection
        return await Task.FromResult(_productCache.ToList());
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery query)
    {
        // For demonstration, search in cache
        var product = _productCache.FirstOrDefault(p => p.Id == query.Id);
        return await Task.FromResult(product);
    }

    public async Task<List<ProductDto>> Handle(GetProductsByCategoryQuery query)
    {
        var products = _productCache
            .Where(p => p.Category.Equals(query.Category, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return await Task.FromResult(products);
    }

    // Helper method to add products to cache (called from command handlers)
    public static void AddToCache(ProductDto product)
    {
        var existing = _productCache.FirstOrDefault(p => p.Id == product.Id);
        if (existing != null)
        {
            _productCache.Remove(existing);
        }
        _productCache.Add(product);
    }

    public static void RemoveFromCache(Guid productId)
    {
        var existing = _productCache.FirstOrDefault(p => p.Id == productId);
        if (existing != null)
        {
            _productCache.Remove(existing);
        }
    }
}