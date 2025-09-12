using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Commands;

public record ChangeProductPrice(Guid ProductId, decimal Price) : ICommand;

public class ChangeProductPriceHandler : ICommandHandler<ChangeProductPrice>
{
    public Task<Result> Handle(ChangeProductPrice command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
