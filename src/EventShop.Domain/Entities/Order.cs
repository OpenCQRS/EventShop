using OpenCqrs.Domain;

namespace EventShop.Domain.Entities;

public class Order : AggregateRoot
{
    public string CustomerName { get; private set; } = string.Empty;
    public string CustomerEmail { get; private set; } = string.Empty;
    public List<OrderItem> Items { get; private set; } = new();
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime OrderDate { get; private set; }

    public Order() { } // Required for persistence

    public Order(Guid id, string customerName, string customerEmail)
    {
        Id = id;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Items = new List<OrderItem>();
        Status = OrderStatus.Pending;
        OrderDate = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, string productName, decimal price, int quantity)
    {
        var item = new OrderItem(productId, productName, price, quantity);
        Items.Add(item);
        RecalculateTotal();
    }

    public void RemoveItem(Guid productId)
    {
        Items.RemoveAll(i => i.ProductId == productId);
        RecalculateTotal();
    }

    public void ConfirmOrder()
    {
        if (Status == OrderStatus.Pending)
        {
            Status = OrderStatus.Confirmed;
        }
    }

    public void CompleteOrder()
    {
        if (Status == OrderStatus.Confirmed)
        {
            Status = OrderStatus.Completed;
        }
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Pending || Status == OrderStatus.Confirmed)
        {
            Status = OrderStatus.Cancelled;
        }
    }

    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.Price * i.Quantity);
    }
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public OrderItem() { } // Required for persistence

    public OrderItem(Guid productId, string productName, decimal price, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}