public interface ICartManager
{
    Task<Cart> GetCart(int customerId);
    Task<CartItem> AddCartItem(int customerId, int cartItemId, int quantity);
    Task<CartItem> UpdateCartItem(int cartItemId, int quantity);
    Task RemoveItem(int cartItemId);
    Task ClearCart(int cartId);
}