using OpenCqrs.Domain;

namespace EventShop.Domain.Entities;

public class Product : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public Product() { } // Required for persistence

    public Product(Guid id, string name, string description, decimal price, int stock, string category, string imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        Category = category;
        ImageUrl = imageUrl;
        IsActive = true;
    }

    public void UpdateStock(int newStock)
    {
        Stock = newStock;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}