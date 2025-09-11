using EventShop.Domain.Ordering.Events;
using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Ordering.Aggregates;

[AggregateType("ShoppingCart")]
public class ShoppingCart : AggregateRoot
{
    public override Type[] EventTypeFilter =>
    [
        typeof(ItemAddedToCart)
    ];

    protected override bool Apply<T>(T @event)
    {
        throw new NotImplementedException();
    }
}
