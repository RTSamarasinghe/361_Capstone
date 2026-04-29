using DataContracts;

public class CheckoutManager : ICheckoutManager
{
	private readonly ICheckoutEngine _checkoutEngine;

	public CheckoutManager(ICheckoutEngine checkoutEngine)
	{
		_checkoutEngine = checkoutEngine;
	}

	public int ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId)
	{
		return _checkoutEngine.ConvertCartToOrder(customerId, shippingAddressId, billingAddressId);
	}

	public int PayForOrder(int orderId, int paymentMethodId)
	{
		return _checkoutEngine.PayForOrder(orderId, paymentMethodId);
	}
}

