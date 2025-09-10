using OpenCqrs.Commands;
using OpenCqrs.Domain;
using EventShop.Domain.Commands;
using EventShop.Domain.Entities;
using EventShop.Domain.Queries;
using EventShop.Infrastructure.QueryHandlers;

namespace EventShop.Infrastructure.CommandHandlers;

public class ProductCommandHandlers : 
    ICommandHandler<CreateProductCommand>,
    ICommandHandler<UpdateProductStockCommand>,
    ICommandHandler<UpdateProductPriceCommand>
{
    private readonly IRepository<Product> _repository;

    public ProductCommandHandlers(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateProductCommand command)
    {
        var product = new Product(
            command.Id,
            command.Name,
            command.Description,
            command.Price,
            command.Stock,
            command.Category,
            command.ImageUrl);

        await _repository.SaveAsync(product);
        
        // Update the query cache for demonstration
        ProductQueryHandlers.AddToCache(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive
        });
    }

    public async Task Handle(UpdateProductStockCommand command)
    {
        var product = await _repository.GetByIdAsync(command.Id);
        if (product != null)
        {
            product.UpdateStock(command.NewStock);
            await _repository.SaveAsync(product);
            
            // Update the query cache
            ProductQueryHandlers.AddToCache(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            });
        }
    }

    public async Task Handle(UpdateProductPriceCommand command)
    {
        var product = await _repository.GetByIdAsync(command.Id);
        if (product != null)
        {
            product.UpdatePrice(command.NewPrice);
            await _repository.SaveAsync(product);
            
            // Update the query cache
            ProductQueryHandlers.AddToCache(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            });
        }
    }
}