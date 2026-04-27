using DataContracts;

public interface ICheckoutEngine
{
	void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId);
	void PayForOrder(int orderId, int paymentMethodId);
}
