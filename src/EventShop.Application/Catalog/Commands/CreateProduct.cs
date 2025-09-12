using OpenCqrs.Commands;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Commands;

public record CreateProduct(string Name, string Description, decimal Price) : ICommand<Guid>;

public class CreateProductHandler : ICommandHandler<CreateProduct, Guid>
{
    public Task<Result<Guid>> Handle(CreateProduct command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
