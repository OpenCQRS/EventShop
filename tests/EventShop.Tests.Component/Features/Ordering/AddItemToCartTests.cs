using Microsoft.AspNetCore.Mvc.Testing;

namespace EventShop.Tests.Component.Features.Ordering;

public class AddItemToCartTests(WebApplicationFactory<Program> factory) : ComponentTestBase(factory)
{
    // TODO: If shopping cart does not exist, it should be created
    // TODO: If product does not exist, it should fail
    // TODO: If quantity is less than 1, it should fail
    // TODO: If valid data provided, it should succeed and item added to cart
    // TODO: If item already exists in cart, it should update the quantity
    // TODO: If multiple items added, it should add all items to cart
    // TODO: If valid data provided, it should succeed and read model created
    // TODO: If item already exists in cart, it should update the quantity and read model updated
}
