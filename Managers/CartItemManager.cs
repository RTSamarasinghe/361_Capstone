using DataContracts;

public class CartItemManager : ICartItemManager
{
    private readonly ICartItemEngine _cartItemEngine;

    public CartItemManager(ICartItemEngine cartItemEngine)
    {
        _cartItemEngine = cartItemEngine;
    }

    public int AddCartItem(int cartId, int productId, int quantity)
    {
        return _cartItemEngine.AddCartItem(cartId, productId, quantity);
    }

    public CartItem GetCartItem(int id)
    {
        return _cartItemEngine.GetCartItem(id);
    }

    public List<CartItem> GetCartItemsByCart(int cartId)
    {
        return _cartItemEngine.GetCartItemsByCart(cartId);
    }

    public void UpdateCartItemQuantity(int id, int quantity)
    {
        _cartItemEngine.UpdateCartItemQuantity(id, quantity);
    }

    public void DeleteCartItem(int id)
    {
        _cartItemEngine.DeleteCartItem(id);
    }

    public void DeleteAllCartItems(int cartId)
    {
        _cartItemEngine.DeleteAllCartItems(cartId);
    }
}