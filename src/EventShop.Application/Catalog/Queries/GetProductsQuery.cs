using EventShop.Infrastructure;
using EventShop.Infrastructure.Projections;
using OpenCqrs.Queries;
using OpenCqrs.Results;

namespace EventShop.Application.Catalog.Queries;

public record GetProductsQuery : IQuery<ProductReadModel[]>;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, ProductReadModel[]>
{
    public Task<Result<ProductReadModel[]>> Handle(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
