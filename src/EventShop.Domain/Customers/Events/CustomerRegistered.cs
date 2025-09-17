using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Customers.Events;

[EventType("CustomerRegistered")]
public record CustomerRegistered(Guid CustomerId, string Name) : IEvent;
