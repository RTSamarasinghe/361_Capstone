using DataContracts;

public class CartEngine : ICartEngine
{
    private readonly ICartAccessor _cartAccessor;
    private readonly ICartItemAccessor _cartItemAccessor;

    public CartEngine(ICartAccessor cartAccessor, ICartItemAccessor cartItemAccessor)
    {
        _cartAccessor = cartAccessor;
        _cartItemAccessor = cartItemAccessor;
    }

    public int AddCart()
    {
        return _cartAccessor.AddCart();
    }

    public Cart GetCart(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        Cart cart = _cartAccessor.GetCart(id);

        if (cart == null)
        {
            throw new Exception("Cart not found.");
        }

        return cart;
    }

    public List<CartItem> GetCartItems(int cartId)
    {
        EnsureCartExists(cartId);
        return _cartItemAccessor.GetCartItemsByCart(cartId);
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

        List<CartItem> existingItems = _cartItemAccessor.GetCartItemsByCart(cartId);

        foreach (CartItem item in existingItems)
        {
            if (item.ProductId == productId)
            {
                int newQuantity = item.Quantity + quantity;
                _cartItemAccessor.UpdateCartItemQuantity(item.Id, newQuantity);
                return item.Id;
            }
        }

        return _cartItemAccessor.AddCartItem(cartId, productId, quantity);
    }

    public void UpdateCartItemQuantity(int cartItemId, int quantity)
    {
        if (cartItemId <= 0)
        {
            throw new ArgumentException("Cart item id must be greater than 0.");
        }

        CartItem existingItem = _cartItemAccessor.GetCartItem(cartItemId);
        if (existingItem == null)
        {
            throw new Exception("Cart item not found.");
        }

        if (quantity <= 0)
        {
            _cartItemAccessor.DeleteCartItem(cartItemId);
            return;
        }

        _cartItemAccessor.UpdateCartItemQuantity(cartItemId, quantity);
    }

    public void RemoveCartItem(int cartItemId)
    {
        if (cartItemId <= 0)
        {
            throw new ArgumentException("Cart item id must be greater than 0.");
        }

        CartItem existingItem = _cartItemAccessor.GetCartItem(cartItemId);
        if (existingItem == null)
        {
            throw new Exception("Cart item not found.");
        }

        _cartItemAccessor.DeleteCartItem(cartItemId);
    }

    public void ClearCart(int cartId)
    {
        EnsureCartExists(cartId);
        _cartItemAccessor.DeleteAllCartItems(cartId);
    }

    public void DeleteCart(int id)
    {
        EnsureCartExists(id);
        _cartItemAccessor.DeleteAllCartItems(id);
        _cartAccessor.DeleteCart(id);
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