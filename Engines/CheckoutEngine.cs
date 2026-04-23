using DataContracts;
using Engines;

	public class CheckoutEngine : ICheckoutEngine
	{
		private readonly ICartItemEngine _cartItemEngine;
		private readonly ICustomerEngine _customerEngine;
		private readonly IOrderEngine _orderEngine;
		private readonly IOrderItemEngine _orderItemEngine;
		private readonly IPaymentEngine _paymentEngine;
		private readonly IPaymentMethodEngine _paymentMethodEngine;
		private readonly IProductEngine _productEngine;
		private readonly ISaleCategoryEngine _saleCategoryEngine;
		private readonly ISaleEngine _saleEngine;
		private readonly ISaleItemEngine _saleItemEngine;

		public CheckoutEngine(ICustomerEngine customerEngine, ICartItemEngine cartItemEngine, IOrderEngine orderEngine, IOrderItemEngine orderItemEngine, IPaymentEngine paymentEngine, IPaymentMethodEngine paymentMethodEngine, IProductEngine productEngine, ISaleCategoryEngine saleCategoryEngine, ISaleEngine saleEngine, ISaleItemEngine saleItemEngine)
		{
			_cartItemEngine = cartItemEngine;
			_customerEngine = customerEngine;
			_orderEngine = orderEngine;
			_orderItemEngine = orderItemEngine;
			_paymentEngine = paymentEngine;
			_paymentMethodEngine = paymentMethodEngine;
			_productEngine = productEngine;
			_saleCategoryEngine = saleCategoryEngine;
			_saleEngine = saleEngine;
			_saleItemEngine = saleItemEngine;
		}

		public void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId, int paymentMethodId) 
		{
			Customer customer = _customerEngine.GetCustomer(customerId);
			int cartId = customer.UserCart;
			List<CartItem> cart = _cartItemEngine.GetCartItemsByCart(cartId);

			if(cart.Count == 0)
			{
				throw new Exception("Cannot checkout empty cart"); 
			}

			int orderId = _orderEngine.AddOrder(customerId, 0.0m, "Processing", shippingAddressId, billingAddressId);
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
			_orderEngine.UpdateOrderStatus(orderId, "Awaiting Payment");
		}

		public void PayForOrder(int orderId, int paymentMethodId)
		{
			Order order = _orderEngine.GetOrder(orderId);
			_paymentMethodEngine.GetPaymentMethod(paymentMethodId);

			_paymentEngine.AddPayment(orderId, order.TotalAmount, DateTime.Now, paymentMethodId);
			_orderEngine.UpdateOrderStatus(orderId, "Purchased");
		}

		private decimal ApplySales(Product product)
		{
			decimal price = product.Price;

			List<Sale> ActiveSales = _saleEngine.GetActiveSales();
			List<SaleCategory> CategorySales = _saleCategoryEngine.GetSaleCategoriesByCategory(product.CategoryId);
			List<SaleItem> ProductSales = _saleItemEngine.GetSaleItemsByProduct(product.Id);

			for(int i = 0; i < CategorySales.Count; i++)
			{
				for(int j = 0; j < ActiveSales.Count; j++)
				{
					if(CategorySales[i].SaleId == ActiveSales[j].Id)
					{
						if(ActiveSales[j].DiscountPercent != null) 
						{
							price = price * (1 - ((decimal)ActiveSales[j].DiscountPercent) / 100);
						}
						else if(ActiveSales[j].DiscountAmount != null)
						{
							price = price - (decimal)ActiveSales[j].DiscountAmount;
						}
					}
				}
			}

			for (int i = 0; i < ProductSales.Count; i++)
			{
				for (int j = 0; j < ActiveSales.Count; j++)
				{
					if (ProductSales[i].SaleId == ActiveSales[j].Id)
					{
						if (ActiveSales[j].DiscountPercent != null)
						{
							price = price * (1 - ((decimal)ActiveSales[j].DiscountPercent) / 100);
						}
						else if (ActiveSales[j].DiscountAmount != null)
						{
							price = price - (decimal)ActiveSales[j].DiscountAmount;
						}
					}
				}
			}

			return price;
		}
	}
