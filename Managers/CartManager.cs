using DataContracts;

public class CartManager : ICartManager
{
    private readonly ICartEngine _cartEngine;

    public CartManager(ICartEngine cartEngine)
    {
        _cartEngine = cartEngine;
    }

    public int AddCart()
    {
        return _cartEngine.AddCart();
    }

    public Cart GetCart(int id)
    {
        return _cartEngine.GetCart(id);
    }

    public List<CartItem> GetCartItems(int cartId)
    {
        return _cartEngine.GetCartItems(cartId);
    }

    public int AddCartItem(int cartId, int productId, int quantity)
    {
        return _cartEngine.AddCartItem(cartId, productId, quantity);
    }

    public void UpdateCartItemQuantity(int cartItemId, int quantity)
    {
        _cartEngine.UpdateCartItemQuantity(cartItemId, quantity);
    }

    public void RemoveCartItem(int cartItemId)
    {
        _cartEngine.RemoveCartItem(cartItemId);
    }

    public void ClearCart(int cartId)
    {
        _cartEngine.ClearCart(cartId);
    }

    public void DeleteCart(int id)
    {
        _cartEngine.DeleteCart(id);
    }
}