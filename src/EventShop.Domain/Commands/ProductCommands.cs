using OpenCqrs.Commands;

namespace EventShop.Domain.Commands;

public class CreateProductCommand : Command
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class UpdateProductStockCommand : Command
{
    public int NewStock { get; set; }
}

public class UpdateProductPriceCommand : Command
{
    public decimal NewPrice { get; set; }
}