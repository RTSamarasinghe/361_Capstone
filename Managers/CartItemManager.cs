using DataContracts;

public class CartItemManager : ICartItemManager
{
    private readonly ICartItemAccessor _cartItemAccessor;

    public CartItemManager(ICartItemAccessor cartItemAccessor)
    {
        _cartItemAccessor = cartItemAccessor;
    }

    public int AddCartItem(int cartId, int productId, int quantity)
    {
        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        if (productId <= 0)
        {
            throw new ArgumentException("Product id must be greater than 0.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        return _cartItemAccessor.AddCartItem(cartId, productId, quantity);
    }

    public CartItem GetCartItem(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Cart item id must be greater than 0.");
        }

        CartItem item = _cartItemAccessor.GetCartItem(id);

        if (item == null)
        {
            throw new Exception("Cart item not found.");
        }

        return item;
    }

    public List<CartItem> GetCartItemsByCart(int cartId)
    {
        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        return _cartItemAccessor.GetCartItemsByCart(cartId);
    }

    public void DeleteCartItem(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Cart item id must be greater than 0.");
        }

        CartItem existingItem = _cartItemAccessor.GetCartItem(id);
        if (existingItem == null)
        {
            throw new Exception("Cart item not found.");
        }

        _cartItemAccessor.DeleteCartItem(id);
    }

    public void DeleteAllCartItems(int cartId)
    {
        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        _cartItemAccessor.DeleteAllCartItems(cartId);
    }
}