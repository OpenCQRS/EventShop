using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Ordering.Commands;

public record AddItemToCart(Guid ShoppingCartId, Guid ProductId, int Quantity, decimal Price) : ICommand;

public class AddItemToCartHandler : ICommandHandler<AddItemToCart>
{
    public Task<Result> Handle(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}