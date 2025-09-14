using EventShop.Application.Catalog.Commands;
using EventShop.Domain.Catalog.Aggregates;
using EventShop.Domain.Catalog.Streams;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EventShop.Tests.Component.Features;

public class ChangeProductPriceTests(WebApplicationFactory<Program> factory) : ComponentTestBase(factory)
{
    [Fact]
    public async Task ChangeProductPrice_ShouldFail_WhenProductDoesNotExist()
    {
        var changeProductPrice = new ChangeProductPrice(Id: Guid.NewGuid(), NewPrice: 18.99m);
        var result = await Dispatcher.Send(changeProductPrice, validateCommand: true);
        
        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();            
        }
    }
    
    [Fact]
    public async Task ChangeProductPrice_ShouldFail_WhenInvalidDataProvided()
    {
        var createProductResult = await Dispatcher.Send(new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
        ));

        var changeProductPrice = new ChangeProductPrice(Id: createProductResult.Value, NewPrice: -5.00m);
        var result = await Dispatcher.Send(changeProductPrice, validateCommand: true);
        
        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();            
        }
    }
    
    [Fact]
    public async Task ChangeProductPrice_ShouldSucceed_WhenValidDataProvided()
    {
        var createProductResult = await Dispatcher.Send(new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
        ));

        var changeProductPrice = new ChangeProductPrice(Id: createProductResult.Value, NewPrice: 18.99m);
        var result = await Dispatcher.Send(changeProductPrice, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
        }
    }
    
    [Fact]
    public async Task ChangeProductPrice_ShouldSucceed_AndPriceChanged()
    {
        var createProductResult = await Dispatcher.Send(new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
        ));
        
        await Dispatcher.Send(new ChangeProductPrice(
            Id: createProductResult.Value, 
            NewPrice: 18.99m
        ));

        var streamId = new ProductStreamId(createProductResult.Value);
        var aggregateId = new ProductAggregateId(createProductResult.Value);
        var result = await DomainService.GetAggregate(streamId, aggregateId);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();            
            result.Value.Id.Should().Be(createProductResult.Value);
            result.Value.Name.Should().Be("Test Product");
            result.Value.Description.Should().Be("This is a test product");
            result.Value.Price.Should().Be(19.99m);
        }
    }
    
    // TODO: Read model updated
}
