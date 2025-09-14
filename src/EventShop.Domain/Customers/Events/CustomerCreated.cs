using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Customers.Events;

[EventType("CustomerCreated")]
public record CustomerCreated(Guid CustomerId, string Name) : IEvent;
