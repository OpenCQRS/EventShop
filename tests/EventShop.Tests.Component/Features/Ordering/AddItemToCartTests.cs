using System.Diagnostics;
using EventShop.Application.Catalog.Commands;
using EventShop.Application.Customers.Commands;
using EventShop.Application.Ordering.Commands;
using EventShop.Domain.Ordering.Aggregates;
using EventShop.Domain.Streams;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EventShop.Tests.Component.Features.Ordering;

public class AddItemToCartTests : ComponentTestBase
{
    private readonly Guid _customerId;
    private readonly Guid _productId;

    public AddItemToCartTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        var customerCreationResult = Dispatcher.Send(new RegisterCustomer(Name: "Test User")).Result;
        _customerId = customerCreationResult.Value;

        var productCreationResult = Dispatcher.Send(new CreateProduct(Name: "Test Product", Description: "Test Description", Price: 10m)).Result;
        _productId = productCreationResult.Value;
    }

    [Fact]
    public async Task AddItemToCart_ShouldSucceed_WhenCustomerDoesNotExistAsItWouldBeProcessedAsGuest()
    {
        var addItemToCart = new AddItemToCart(
            CustomerId: Guid.NewGuid(),
            ShoppingCartId: Guid.NewGuid(),
            _productId,
            Quantity: 1,
            Price: 10m);

        var result = await Dispatcher.Send(addItemToCart);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
        }
    }

    [Fact]
    public async Task AddItemToCart_ShouldFail_WhenProductDoesNotExist()
    {
        var addItemToCart = new AddItemToCart(
            _customerId,
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AddItemToCart_ShouldFail_WhenQuantityIsLessThanOne(int quantity)
    {
        var addItemToCart = new AddItemToCart(
            _customerId,
            ShoppingCartId: Guid.NewGuid(),
            _productId,
            Quantity: quantity,
            Price: 10m);

        var result = await Dispatcher.Send(addItemToCart, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();
        }
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-0.00000000001)]
    public async Task AddItemToCart_ShouldFail_WhenPriceIsNegative(decimal price)
    {
        var addItemToCart = new AddItemToCart(
            _customerId,
            ShoppingCartId: Guid.NewGuid(),
            _productId,
            Quantity: 1,
            price);

        var result = await Dispatcher.Send(addItemToCart, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task AddItemToCart_ShouldSucceed_WhenPriceIsNotNegative(decimal price)
    {
        var addItemToCart = new AddItemToCart(
            _customerId,
            ShoppingCartId: Guid.NewGuid(),
            _productId,
            Quantity: 1,
            price);

        var result = await Dispatcher.Send(addItemToCart, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
        }
    }

    [Fact]
    public async Task AddItemToCart_ShouldCreateNewShoppingCartIfItDoesNotExist()
    {
        var shoppingCartId = Guid.NewGuid();
        var streamId = new CustomerStreamId(_customerId);
        var aggregateId = new ShoppingCartAggregateId(shoppingCartId);

        await Dispatcher.Send(new AddItemToCart(
            _customerId,
            shoppingCartId,
            _productId,
            Quantity: 1,
            Price: 10m));

        var result = await DomainService.GetAggregate(streamId, aggregateId);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
            result.Value.Should().NotBeNull();
            result.Value.ShoppingCartId.Should().Be(shoppingCartId);
            result.Value.ShoppingCartItems.Count().Should().Be(1);
            result.Value.ShoppingCartItems.Single().ProductId.Should().Be(_productId);
            result.Value.ShoppingCartItems.Single().Quantity.Should().Be(1);
            result.Value.ShoppingCartItems.Single().UnitPrice.Should().Be(10m);
        }
    }

    // TODO: If valid data provided, it should succeed and item added to cart
    // TODO: If item already exists in cart, it should update the quantity
    // TODO: If multiple items added, it should add all items to cart
    // TODO: If valid data provided, it should succeed and read model created
    // TODO: If item already exists in cart, it should update the quantity and read model updated
}
