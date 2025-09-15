using EventShop.Domain.Catalog.Aggregates;
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
        var customerResult = await ValidateAndRetrieveCustomer(command.CustomerId, cancellationToken);
        if (customerResult.IsNotSuccess)
        {
            return customerResult.Failure!;
        }

        var productResult = await ValidateAndRetrieveProduct(command.ProductId, cancellationToken);
        if (productResult.IsNotSuccess)
        {
            return productResult.Failure!;
        }
        
        return Guid.NewGuid();
    }

    private async Task<Result<Customer>> ValidateAndRetrieveCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var customerStreamId = new CustomerStreamId(customerId);
        var customerAggregateId = new CustomerAggregateId(customerId);
        
        var customerResult = await domainService.GetAggregate(customerStreamId, customerAggregateId, cancellationToken: cancellationToken);
        if (customerResult.IsNotSuccess)
        {
            return customerResult.Failure!;
        }
        if (customerResult.Value == null)
        {
            return new Failure(ErrorCode.NotFound, Title: "Customer not found", Description: $"Customer with ID {customerId} not found.");
        }

        return customerResult.Value;
    }

    private async Task<Result<Product>> ValidateAndRetrieveProduct(Guid productId, CancellationToken cancellationToken)
    {
        var productStreamId = new ProductStreamId(productId);
        var productAggregateId = new ProductAggregateId(productId);
        
        var productResult = await domainService.GetAggregate(productStreamId, productAggregateId, cancellationToken: cancellationToken);
        if (productResult.IsNotSuccess)
        {
            return productResult.Failure!;
        }
        if (productResult.Value == null)
        {
            return new Failure(ErrorCode.NotFound, Title: "Product not found", Description: $"Product with ID {productId} not found.");
        }
        
        return productResult.Value;
    }
}
