public class CartManager : ICartManager
{
	private readonly ICartEngine _cartEngine;
	private readonly IProductEngine _productEngine;

	public CartManager(ICartEngine cartEngine, IProductEngine productEngine)
	{
		_cartEngine = cartEngine;
		_productEngine = productEngine;
	}

	public CartDto GetCart(int customerId)
	{
		return _cartEngine.GetCart(customerId);
	}

	public void AddCartItem(int customerId, int cartItemId, int quantity)
	{
		// Orchestration logic

		var product = _productEngine.GetProduct(cartItemId);

		if (product == null)
			throw new Exception("Product not found");

		if (product.Stock < quantity)
			throw new Exception("Not enough stock");

		_cartEngine.AddItem(customerId, cartItemId, quantity);
	}

	public void RemoveItem(int customerId, int cartItemId)
	{
		_cartEngine.RemoveItem(customerId, cartItemId);
	}

	public void UpdateCartItem(int customerId, int cartItemId, int quantity)
	{
		if (quantity <= 0)
		{
			_cartEngine.RemoveItem(customerId, cartItemId);
			return;
		}

		var product = _productEngine.GetProduct(cartItemId);

		if (product.Stock < quantity)
			throw new Exception("Not enough stock");

		_cartEngine.UpdateQuantity(customerId, cartItemId, quantity);
	}

	public void ClearCart(int customerId)
	{
		_cartEngine.ClearCart(customerId);
	}
}