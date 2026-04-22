using DataContracts;

public interface ICheckoutManager
{
	void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId, int paymentMethodId);
	void PayForOrder(int orderId, int paymentMethodId);
}

