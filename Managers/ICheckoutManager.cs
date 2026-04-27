using DataContracts;

public interface ICheckoutManager
{
	void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId);
	void PayForOrder(int orderId, int paymentMethodId);
}

