using DataContracts;

public interface ICheckoutEngine
{
	int ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId);
	int PayForOrder(int orderId, int paymentMethodId);
}
