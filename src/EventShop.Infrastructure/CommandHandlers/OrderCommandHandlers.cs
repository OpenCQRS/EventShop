using OpenCqrs.Commands;
using OpenCqrs.Domain;
using EventShop.Domain.Commands;
using EventShop.Domain.Entities;

namespace EventShop.Infrastructure.CommandHandlers;

public class OrderCommandHandlers : 
    ICommandHandler<CreateOrderCommand>,
    ICommandHandler<AddItemToOrderCommand>,
    ICommandHandler<ConfirmOrderCommand>
{
    private readonly IRepository<Order> _repository;

    public OrderCommandHandlers(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateOrderCommand command)
    {
        var order = new Order(
            command.Id,
            command.CustomerName,
            command.CustomerEmail);

        await _repository.SaveAsync(order);
    }

    public async Task Handle(AddItemToOrderCommand command)
    {
        var order = await _repository.GetByIdAsync(command.Id);
        if (order != null)
        {
            order.AddItem(command.ProductId, command.ProductName, command.Price, command.Quantity);
            await _repository.SaveAsync(order);
        }
    }

    public async Task Handle(ConfirmOrderCommand command)
    {
        var order = await _repository.GetByIdAsync(command.Id);
        if (order != null)
        {
            order.ConfirmOrder();
            await _repository.SaveAsync(order);
        }
    }
}