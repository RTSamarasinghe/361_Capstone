using DataContracts;

public interface ICartManager
{
    int AddCart();
    Cart GetCart(int id);
    List<CartItem> GetCartItems(int cartId);
    int AddCartItem(int cartId, int productId, int quantity);
    void UpdateCartItemQuantity(int cartItemId, int quantity);
    void RemoveCartItem(int cartItemId);
    void ClearCart(int cartId);
    void DeleteCart(int id);
}