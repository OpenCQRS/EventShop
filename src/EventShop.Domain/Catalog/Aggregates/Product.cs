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

    protected override bool Apply<T>(T @event)
    {
        throw new NotImplementedException();
    }
}
