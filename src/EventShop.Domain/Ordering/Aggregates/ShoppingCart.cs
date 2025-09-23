using EventShop.Domain.Ordering.Events;
using Newtonsoft.Json;
using OpenCqrs.EventSourcing.Domain;

namespace EventShop.Domain.Ordering.Aggregates;

[AggregateType("ShoppingCart")]
public class ShoppingCart : AggregateRoot
{
    public override Type[] EventTypeFilter =>
    [
        typeof(ItemAddedToCart)
    ];

    public Guid ShoppingCartId { get; private set; }

    [JsonProperty(nameof(ShoppingCartItems))]
    private readonly List<ShoppingCartItem> _shoppingCartItems = [];
    [JsonIgnore]
    public IEnumerable<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems.AsReadOnly();

    public ShoppingCart()
    {
    }

    public ShoppingCart(Guid shoppingCartId, Guid productId, int quantity, decimal unitPrice)
    {
        Add(new ItemAddedToCart(shoppingCartId, productId, quantity, unitPrice));
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        var existingItem = _shoppingCartItems.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            Add(new ItemQuantityUpdated(ShoppingCartId, productId, newQuantity));
        }
        else
        {
            Add(new ItemAddedToCart(ShoppingCartId, productId, quantity, unitPrice));
        }
    }

    protected override bool Apply<T>(T domainEvent)
    {
        return domainEvent switch
        {
            ItemAddedToCart @event => Apply(@event),
            ItemQuantityUpdated @event => Apply(@event),
            _ => false
        };
    }

    private bool Apply(ItemAddedToCart @event)
    {
        ShoppingCartId = @event.ShoppingCartId;
        _shoppingCartItems.Add(new ShoppingCartItem
        {
            ProductId = @event.ProductId,
            Quantity = @event.Quantity,
            UnitPrice = @event.UnitPrice
        });

        return true;
    }
    
    private bool Apply(ItemQuantityUpdated @event)
    {
        var existingItem = _shoppingCartItems.FirstOrDefault(i => i.ProductId == @event.ProductId);
        if (existingItem == null)
        {
            return false;
        }

        existingItem.Quantity = @event.NewQuantity;

        return true;
    }
}

public class ShoppingCartItem
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
}
