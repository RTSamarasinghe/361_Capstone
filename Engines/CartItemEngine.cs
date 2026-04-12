using DataContracts;

public class CartItemEngine : ICartItemEngine
{
    private readonly ICartItemAccessor _cartItemAccessor;
    private readonly ICartAccessor _cartAccessor;

    public CartItemEngine(ICartItemAccessor cartItemAccessor, ICartAccessor cartAccessor)
    {
        _cartItemAccessor = cartItemAccessor;
        _cartAccessor = cartAccessor;
    }

    public int AddCartItem(int cartId, int productId, int quantity)
    {
        EnsureCartExists(cartId);

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
        EnsureCartExists(cartId);
        return _cartItemAccessor.GetCartItemsByCart(cartId);
    }

    public void UpdateCartItemQuantity(int id, int quantity)
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

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        _cartItemAccessor.UpdateCartItemQuantity(id, quantity);
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
        EnsureCartExists(cartId);
        _cartItemAccessor.DeleteAllCartItems(cartId);
    }

    private void EnsureCartExists(int cartId)
    {
        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        Cart cart = _cartAccessor.GetCart(cartId);
        if (cart == null)
        {
            throw new Exception("Cart not found.");
        }
    }
}