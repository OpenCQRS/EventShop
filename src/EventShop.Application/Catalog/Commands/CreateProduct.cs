using EventShop.Domain.Catalog.Aggregates;
using EventShop.Domain.Catalog.Streams;
using OpenCqrs.Commands;
using OpenCqrs.EventSourcing;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Commands;

public record CreateProduct(string Name, string Description, decimal Price) : ICommand<Guid>;

public class CreateProductHandler(IDomainService domainService) : ICommandHandler<CreateProduct, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProduct command, CancellationToken cancellationToken = default)
    {
        var productId = Guid.NewGuid();
        var streamId = new ProductStreamId(productId);
        var aggregateId = new ProductAggregateId(productId);
        var aggregate = new Product(productId, command.Name, command.Description, command.Price);
        await domainService.SaveAggregate(streamId, aggregateId, aggregate, expectedEventSequence: 0, cancellationToken);
        return productId;
    }
}
