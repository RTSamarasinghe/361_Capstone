using DataContracts;

public interface ICartItemManager
{
    int AddCartItem(int cartId, int productId, int quantity);
    CartItem GetCartItem(int id);
    List<CartItem> GetCartItemsByCart(int cartId);
    void DeleteCartItem(int id);
    void DeleteAllCartItems(int cartId);
}