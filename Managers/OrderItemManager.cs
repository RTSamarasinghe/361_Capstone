using DataContracts;

public class OrderItemManager : IOrderItemManager
{
    private readonly IOrderItemEngine _orderItemEngine;

    public OrderItemManager(IOrderItemEngine orderItemEngine)
    {
        _orderItemEngine = orderItemEngine;
    }

    public int AddOrderItem(int orderId, int productId, int quantity, decimal unitPrice)
    {
        return _orderItemEngine.AddOrderItem(orderId, productId, quantity, unitPrice);
    }

    public OrderItem GetOrderItem(int id)
    {
        return _orderItemEngine.GetOrderItem(id);
    }

    public List<OrderItem> GetOrderItemsByOrder(int orderId)
    {
        return _orderItemEngine.GetOrderItemsByOrder(orderId);
    }

    public void UpdateOrderItemQuantity(int id, int quantity)
    {
        _orderItemEngine.UpdateOrderItemQuantity(id, quantity);
    }

    public void DeleteOrderItem(int id)
    {
        _orderItemEngine.DeleteOrderItem(id);
    }

    public void DeleteAllOrderItems(int orderId)
    {
        _orderItemEngine.DeleteAllOrderItems(orderId);
    }
}