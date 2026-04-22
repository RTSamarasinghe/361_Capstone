using DataContracts;

public interface IOrderEngine
{
    int AddOrder(int customerId, decimal totalAmount, string orderStatus, int shippingAddressId, int billingAddressId);
    Order GetOrder(int id);
    List<Order> GetOrdersByCustomer(int customerId);
    List<Order> GetOrdersByStatus(string orderStatus);
    void UpdateOrderStatus(int id, string orderStatus);
    void UpdateOrderTotalAmount(int id, decimal totalAmount);
    void DeleteOrder(int id);
}