using EventShop.Application.Catalog.Commands;
using EventShop.Application.Customers.Commands;
using EventShop.Application.Ordering.Commands;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EventShop.Tests.Component.Features.Ordering;

public class AddItemToCartTests : ComponentTestBase
{
    private Guid CustomerId;
    private Guid ProductId;
    
    public AddItemToCartTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        var customerCreationResult = Dispatcher.Send(new CreateCustomer(Name: "Test User")).Result;
        CustomerId = customerCreationResult.Value;
        
        var productCreationResult = Dispatcher.Send(new CreateProduct(Name: "Test Product", Description: "Test Description", Price: 10m)).Result;
        ProductId = productCreationResult.Value;
    }

    [Fact]
    public async Task AddItemToCart_ShouldFail_WhenCustomerDoesNotExist()
    {
        var addItemToCart = new AddItemToCart(
            CustomerId: Guid.NewGuid(), 
            ShoppingCartId: Guid.NewGuid(), 
            ProductId: Guid.NewGuid(), 
            Quantity: 1, 
            Price: 10m);

        var result = await Dispatcher.Send(addItemToCart);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();
        }
    }
    
    [Fact]
    public async Task AddItemToCart_ShouldFail_WhenProductDoesNotExist()
    {
        var addItemToCart = new AddItemToCart(
            CustomerId, 
            ShoppingCartId: Guid.NewGuid(), 
            ProductId: Guid.NewGuid(), 
            Quantity: 1, 
            Price: 10m);

        var result = await Dispatcher.Send(addItemToCart);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();
        }
    }
    
    // TODO: If quantity is less than 1, it should fail
    // TODO: If data is invalid, it should fail (price, quantity)
    // TODO: If shopping cart does not exist, it should be created
    // TODO: If valid data provided, it should succeed and item added to cart
    // TODO: If item already exists in cart, it should update the quantity
    // TODO: If multiple items added, it should add all items to cart
    // TODO: If valid data provided, it should succeed and read model created
    // TODO: If item already exists in cart, it should update the quantity and read model updated
}
