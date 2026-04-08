using DataContracts;

public class CartManager : ICartManager
{
    private readonly ICartAccessor _cartAccessor;

    public CartManager(ICartAccessor cartAccessor)
    {
        _cartAccessor = cartAccessor;
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

    public void DeleteCart(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        Cart existingCart = _cartAccessor.GetCart(id);
        if (existingCart == null)
        {
            throw new Exception("Cart not found.");
        }

        _cartAccessor.DeleteCart(id);
    }
}