using EventShop.Domain.Customers.Aggregates;
using EventShop.Domain.Streams;
using OpenCqrs.Commands;
using OpenCqrs.EventSourcing;
using OpenCqrs.Results;

namespace EventShop.Application.Ordering.Commands;

public record AddItemToCart(Guid CustomerId, Guid ShoppingCartId, Guid ProductId, int Quantity, decimal Price) : ICommand<Guid>;

public class AddItemToCartHandler(IDomainService domainService) : ICommandHandler<AddItemToCart, Guid>
{
    public async Task<Result<Guid>> Handle(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        var customerStreamId = new CustomerStreamId(command.CustomerId);
        var customerAggregateId = new CustomerAggregateId(command.CustomerId);
        
        var customerResult = await domainService.GetAggregate(customerStreamId, customerAggregateId, cancellationToken: cancellationToken);
        if (customerResult.IsNotSuccess)
        {
            return customerResult.Failure!;
        }
        var customer = customerResult.Value;
        if (customer == null)
        {
            return new Failure(ErrorCode.NotFound, Title: "Customer not found", Description: $"Customer with ID {command.CustomerId} not found.");
        }

        return Guid.NewGuid();
    }
}
