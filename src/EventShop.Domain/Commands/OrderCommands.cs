using OpenCqrs.Commands;

namespace EventShop.Domain.Commands;

public class CreateOrderCommand : Command
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
}

public class AddItemToOrderCommand : Command
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class ConfirmOrderCommand : Command
{
}