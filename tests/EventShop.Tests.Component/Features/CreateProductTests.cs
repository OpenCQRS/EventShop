using EventShop.Application.Catalog.Commands;
using FluentAssertions;
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

        result.IsSuccess.Should().BeFalse();
        result.Failure.Should().NotBeNull();
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

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
