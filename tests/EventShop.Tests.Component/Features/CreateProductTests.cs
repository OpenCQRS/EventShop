using EventShop.Application.Catalog.Commands;
using EventShop.Domain.Catalog.Aggregates;
using EventShop.Domain.Catalog.Streams;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EventShop.Tests.Component.Features;

public class CreateProductTests(WebApplicationFactory<Program> factory) : ComponentTestBase(factory)
{
    [Theory]
    [InlineData("", "This is a test product", 19.99)]
    [InlineData("Test Product", "", 19.99)]
    [InlineData("Test Product", "This is a test product", -5.00)]
    public async Task CreateProduct_ShouldFail_WhenInvalidDataProvided(string name, string description, decimal price)
    {
        var createProduct = new CreateProduct(name, description, price);
        var result = await Dispatcher.Send(createProduct, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();            
        }
    }
    
    [Fact]
    public async Task CreateProduct_ShouldSucceed_WhenValidDataProvided()
    {
        var createProduct = new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
        );
        
        var result = await Dispatcher.Send(createProduct);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();            
        }        
    }
    
    [Fact]
    public async Task CreateProduct_ShouldSucceed_AndStoreProduct()
    {
        var createProductResult = await Dispatcher.Send(new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
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
    
    // TODO: Read model created
}
