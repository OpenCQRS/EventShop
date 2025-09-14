using EventShop.Application.Catalog.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EventShop.Tests.Component.Features;

public class CreateProductTests(WebApplicationFactory<Program> factory) : ComponentTestBase(factory)
{
    [Fact]
    public async Task CreateProduct_ShouldSucceed_WhenValidDataProvided()
    {
        var createProduct = new CreateProduct(
            Name: "Test Product",
            Description: "This is a test product",
            Price: 19.99m
        );
        
        var result = await Dispatcher.Send(createProduct);
        
        Assert.NotNull(result);
    }
}
