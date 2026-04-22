using DataContracts;

public interface ICheckoutEngine
{
	void ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId, int paymentMethodId);
	void PayForOrder(int customerId, int orderId, int paymentMethodId);
}
