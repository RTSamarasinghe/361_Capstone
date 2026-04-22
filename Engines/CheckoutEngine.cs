using DataContracts;
namespace Engines;
	public class CheckoutEngine : ICheckoutEngine
	{
		private readonly ICartItemEngine _cartItemEngine;
		private readonly ICustomerEngine _customerEngine;
		private readonly IOrderEngine _orderEngine;
		private readonly IOrderItemEngine _orderItemEngine;
		private readonly IProductEngine _productEngine;

		public CheckoutEngine(ICustomerEngine customerEngine, ICartItemEngine cartItemEngine, IOrderEngine orderEngine, IOrderItemEngine orderItemEngine, IProductEngine productEngine)
		{
			_cartItemEngine = cartItemEngine;
			_customerEngine = customerEngine;
			_orderEngine = orderEngine;
			_orderItemEngine = orderItemEngine;
			_productEngine = productEngine;
		}

		public void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId, int paymentMethodId) 
		{
			Customer customer = _customerEngine.GetCustomer(customerId);
			int cartId = customer.UserCart;
			int orderId = _orderEngine.AddOrder(customerId, 0.0m, "Processing", shippingAddressId, billingAddressId);
			List<CartItem> cart = _cartItemEngine.GetCartItemsByCart(cartId);
			decimal totalPrice = 0.0m;

			for(int i = 0; i < cart.Count; i++)
			{
				Product product = _productEngine.GetProduct(cart[i].ProductId);
				decimal price = ApplySales(product);
				_orderItemEngine.AddOrderItem(orderId, product.Id, cart[i].Quantity, price);
				_cartItemEngine.DeleteCartItem(cart[i].Id);
				totalPrice += price * cart[i].Quantity;				
			}

			_orderEngine.UpdateOrderTotalAmount(orderId, totalPrice);
		}

		public void PayForOrder(int customerId, int orderId, int paymentMethodId)
		{
			throw new NotImplementedException("Not yet implemented");
		}

		private static decimal ApplySales(Product product)
		{
			decimal price = product.Price;
			//not yet implemented
			return price;
		}

	}

