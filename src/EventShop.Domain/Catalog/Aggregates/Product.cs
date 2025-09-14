using EventShop.Domain.Catalog.Events;
using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Catalog.Aggregates;

[AggregateType("Product")]
public class Product : AggregateRoot
{
    public override Type[] EventTypeFilter =>
    [
        typeof(ProductCreated),
        typeof(ProductPriceChanged)
    ];

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public Product()
    {
    }
    
    public Product(Guid id, string name, string description, decimal price)
    {
        Add(new ProductCreated(id, name, description, price));
    }

    protected override bool Apply<T>(T domainEvent)
    {
        return domainEvent switch
        {
            ProductCreated @event => Apply(@event),
            ProductPriceChanged @event => Apply(@event),
            _ => false
        };
    }

    private bool Apply(ProductCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;

        return true;
    }
    
    private bool Apply(ProductPriceChanged @event)
    {
        return true;
    }
}
