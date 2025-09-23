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
    private readonly Guid _productId1;
    private readonly Guid _productId2;

    public AddItemToCartTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        var customerCreationResult = Dispatcher.Send(new RegisterCustomer(Name: "Test User")).Result;
        _customerId = customerCreationResult.Value;

        var productCreationResult1 = Dispatcher.Send(new CreateProduct(Name: "Test Product 1", Description: "Test Description 1", Price: 10m)).Result;
        _productId1 = productCreationResult1.Value;

        var productCreationResult2 = Dispatcher.Send(new CreateProduct(Name: "Test Product 2", Description: "Test Description 2", Price: 20m)).Result;
        _productId2 = productCreationResult2.Value;
    }

    [Fact]
    public async Task AddItemToCart_ShouldSucceed_WhenCustomerDoesNotExistAsItWouldBeProcessedAsGuest()
    {
        var addItemToCart = new AddItemToCart(
            CustomerId: Guid.NewGuid(),
            ShoppingCartId: Guid.NewGuid(),
            _productId1,
            Quantity: 1);

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
            Quantity: 1);

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
            _productId1,
            Quantity: quantity);

        var result = await Dispatcher.Send(addItemToCart, validateCommand: true);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Failure.Should().NotBeNull();
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
            _productId1,
            Quantity: 1));

        var result = await DomainService.GetAggregate(streamId, aggregateId);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
            result.Value.Should().NotBeNull();
            result.Value.ShoppingCartId.Should().Be(shoppingCartId);
            result.Value.ShoppingCartItems.Count().Should().Be(1);
            result.Value.ShoppingCartItems.Single().ProductId.Should().Be(_productId1);
            result.Value.ShoppingCartItems.Single().Quantity.Should().Be(1);
            result.Value.ShoppingCartItems.Single().UnitPrice.Should().Be(10m);
        }
    }

    [Fact]
    public async Task AddItemToCart_ShouldAddMultipleProducts()
    {
        var shoppingCartId = Guid.NewGuid();
        var streamId = new CustomerStreamId(_customerId);
        var aggregateId = new ShoppingCartAggregateId(shoppingCartId);

        await Dispatcher.Send(new AddItemToCart(
            _customerId,
            shoppingCartId,
            _productId1,
            Quantity: 1));

        await Dispatcher.Send(new AddItemToCart(
            _customerId,
            shoppingCartId,
            _productId2,
            Quantity: 2));

        var result = await DomainService.GetAggregate(streamId, aggregateId);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
            result.Value.Should().NotBeNull();
            result.Value.ShoppingCartId.Should().Be(shoppingCartId);
            result.Value.ShoppingCartItems.Count().Should().Be(2);
            result.Value.ShoppingCartItems.First().ProductId.Should().Be(_productId1);
            result.Value.ShoppingCartItems.First().Quantity.Should().Be(1);
            result.Value.ShoppingCartItems.First().UnitPrice.Should().Be(10m);
            result.Value.ShoppingCartItems.Last().ProductId.Should().Be(_productId2);
            result.Value.ShoppingCartItems.Last().Quantity.Should().Be(2);
            result.Value.ShoppingCartItems.Last().UnitPrice.Should().Be(20m);
        }
    }

    [Fact]
    public async Task AddItemToCart_ShouldUpdateQuantityIfProductAlreadyAdded()
    {
        var shoppingCartId = Guid.NewGuid();
        var streamId = new CustomerStreamId(_customerId);
        var aggregateId = new ShoppingCartAggregateId(shoppingCartId);

        await Dispatcher.Send(new AddItemToCart(
            _customerId,
            shoppingCartId,
            _productId1,
            Quantity: 1));

        await Dispatcher.Send(new AddItemToCart(
            _customerId,
            shoppingCartId,
            _productId1,
            Quantity: 3));
        
        var result = await DomainService.GetAggregate(streamId, aggregateId);

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Failure.Should().BeNull();
            result.Value.Should().NotBeNull();
            result.Value.ShoppingCartId.Should().Be(shoppingCartId);
            result.Value.ShoppingCartItems.Count().Should().Be(1);
            result.Value.ShoppingCartItems.Single().ProductId.Should().Be(_productId1);
            result.Value.ShoppingCartItems.Single().Quantity.Should().Be(4);
            result.Value.ShoppingCartItems.Single().UnitPrice.Should().Be(10m);
        }
    }
    
    // TODO: If multiple items added, it should add all items to cart
    // TODO: If valid data provided, it should succeed and read model created
    // TODO: If item already exists in cart, it should update the quantity and read model updated
}
