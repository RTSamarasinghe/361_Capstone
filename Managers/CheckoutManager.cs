using DataContracts;

public class CheckoutManager : ICheckoutManager
{
	private readonly ICheckoutEngine _checkoutEngine;

	public CheckoutManager(ICheckoutEngine checkoutEngine)
	{
		_checkoutEngine = checkoutEngine;
	}

	public void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId, int paymentMethodId)
	{
		_checkoutEngine.ConvertCartToOrder(customerId, shippingAddressId, billingAddressId, paymentMethodId);
	}

	public void PayForOrder(int orderId, int paymentMethodId)
	{
		_checkoutEngine.PayForOrder(orderId, paymentMethodId);
	}
}

