using DataContracts;

public class OrderManager : IOrderManager
{
    private readonly IOrderEngine _orderEngine;

    public OrderManager(IOrderEngine orderEngine)
    {
        _orderEngine = orderEngine;
    }

    public int AddOrder(int customerId, decimal totalAmount, string orderStatus, int shippingAddressId, int billingAddressId)
    {
        return _orderEngine.AddOrder(customerId, totalAmount, orderStatus, shippingAddressId, billingAddressId);
    }

    public Order GetOrder(int id)
    {
        return _orderEngine.GetOrder(id);
    }

    public List<Order> GetOrdersByCustomer(int customerId)
    {
        return _orderEngine.GetOrdersByCustomer(customerId);
    }

    public List<Order> GetOrdersByStatus(string orderStatus)
    {
        return _orderEngine.GetOrdersByStatus(orderStatus);
    }

    public void UpdateOrderStatus(int id, string orderStatus)
    {
        _orderEngine.UpdateOrderStatus(id, orderStatus);
    }

    public void UpdateOrderTotalAmount(int id, decimal totalAmount)
    {
        _orderEngine.UpdateOrderTotalAmount(id, totalAmount);
    }

    public void DeleteOrder(int id)
    {
        _orderEngine.DeleteOrder(id);
    }
}