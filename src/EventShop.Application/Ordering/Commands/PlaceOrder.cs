using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Ordering.Commands;

public record OrderItem(Guid ProductId, int Quantity, decimal Price);

public record PlaceOrder(Guid CustomerId, List<OrderItem> Items) : ICommand<Guid>;

public class PlaceOrderHandler : ICommandHandler<PlaceOrder, Guid>
{
    public Task<Result<Guid>> Handle(PlaceOrder command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}