using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price) : ICommand<Guid>;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    public Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
