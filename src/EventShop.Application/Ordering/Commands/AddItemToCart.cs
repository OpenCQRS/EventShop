using EventShop.Domain.Catalog.Aggregates;
using EventShop.Domain.Ordering.Aggregates;
using EventShop.Domain.Streams;
using FluentValidation;
using OpenCqrs.Commands;
using OpenCqrs.EventSourcing;
using OpenCqrs.Results;

namespace EventShop.Application.Ordering.Commands;

public record AddItemToCart(Guid CustomerId, Guid ShoppingCartId, Guid ProductId, int Quantity, decimal Price) : ICommand;

public class AddItemToCartValidator : AbstractValidator<AddItemToCart>
{
    public AddItemToCartValidator()
    {
        RuleFor(c => c.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(c => c.Price).GreaterThan(-1).WithMessage("Price must be zero or greater.");
    }
}

public class AddItemToCartHandler(IDomainService domainService) : ICommandHandler<AddItemToCart>
{
    public async Task<Result> Handle(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        var productResult = await ValidateAndRetrieveProduct(command.ProductId, cancellationToken);
        if (productResult.IsNotSuccess)
        {
            return productResult.Failure!;
        }
        
        var shoppingCart = new ShoppingCart(command.ShoppingCartId, command.ProductId, command.Quantity, command.Price);
        
        var streamId = new CustomerStreamId(command.CustomerId);
        var aggregateId = new ShoppingCartAggregateId(command.ShoppingCartId);
        return await domainService.SaveAggregate(streamId, aggregateId, shoppingCart, expectedEventSequence: 0, cancellationToken: cancellationToken);
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
