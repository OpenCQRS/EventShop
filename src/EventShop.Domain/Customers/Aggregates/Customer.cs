using EventShop.Domain.Customers.Events;
using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Customers.Aggregates;

[AggregateType("Customer")]
public class Customer : AggregateRoot
{
    public override Type[] EventTypeFilter =>
    [
        typeof(CustomerCreated)
    ];

    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Customer()
    {
    }

    public Customer(Guid customerId, string name)
    {
        Add(new CustomerCreated(customerId, name));
    }

    protected override bool Apply<T>(T domainEvent)
    {
        return domainEvent switch
        {
            CustomerCreated @event => Apply(@event),
            _ => false
        };
    }

    private bool Apply(CustomerCreated @event)
    {
        CustomerId = @event.CustomerId;
        Name = @event.Name;

        return true;
    }
}
