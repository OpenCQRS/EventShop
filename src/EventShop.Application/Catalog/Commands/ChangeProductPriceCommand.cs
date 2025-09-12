using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Commands;

public record ChangeProductPriceCommand(Guid ProductId, decimal Price) : ICommand;

public class ChangeProductPriceCommandHandler : ICommandHandler<ChangeProductPriceCommand>
{
    public Task<Result> Handle(ChangeProductPriceCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
