using DataContracts;

public interface ICartItemEngine
{
    int AddCartItem(int cartId, int productId, int quantity);
    CartItem GetCartItem(int id);
    List<CartItem> GetCartItemsByCart(int cartId);
    void UpdateCartItemQuantity(int id, int quantity);
    void DeleteCartItem(int id);
    void DeleteAllCartItems(int cartId);
}