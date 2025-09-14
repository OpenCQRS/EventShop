using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Ordering.Commands;

public record AddItemToCart(Guid CustomerId, Guid ShoppingCartId, Guid ProductId, int Quantity, decimal Price) : ICommand<Guid>;

public class AddItemToCartHandler : ICommandHandler<AddItemToCart, Guid>
{
    public Task<Result<Guid>> Handle(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
