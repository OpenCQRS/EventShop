using OpenCqrs.Queries;
using EventShop.Domain.Entities;

namespace EventShop.Domain.Queries;

public class GetOrderByIdQuery : IQuery<OrderDto?>
{
    public Guid Id { get; set; }
}

public class GetOrdersByCustomerQuery : IQuery<List<OrderDto>>
{
    public string CustomerEmail { get; set; } = string.Empty;
}

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}