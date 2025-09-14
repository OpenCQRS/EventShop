using EventShop.Domain.Customers.Aggregates;
using EventShop.Domain.Streams;
using FluentValidation;
using OpenCqrs.Commands;
using OpenCqrs.EventSourcing;
using OpenCqrs.Results;

namespace EventShop.Application.Customers.Commands;

public record CreateCustomer(string Name) : ICommand<Guid>;

public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.");
    }
}

public class CreateCustomerHandler(IDomainService domainService) : ICommandHandler<CreateCustomer, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCustomer command, CancellationToken cancellationToken = default)
    {
        var customerId = Guid.NewGuid();
        var streamId = new CustomerStreamId(customerId);
        var aggregateId = new CustomerAggregateId(customerId);
        var aggregate = new Customer(customerId, command.Name);
        await domainService.SaveAggregate(streamId, aggregateId, aggregate, expectedEventSequence: 0, cancellationToken);
        return customerId;
    }
}
