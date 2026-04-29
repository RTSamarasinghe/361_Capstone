using DataContracts;

public interface ICheckoutManager
{
	int ConvertCartToOrder(int customerId, int shippingAddressId, int billingAddressId);
	int PayForOrder(int orderId, int paymentMethodId);
}

