using DataContracts;

public interface IOrderItemAccessor
{
    int AddOrderItem(int orderId, int productId, int quantity, decimal unitPrice);
    OrderItem GetOrderItem(int id);
    List<OrderItem> GetOrderItemsByOrder(int orderId);
    void UpdateOrderItemQuantity(int id, int quantity);
    void DeleteOrderItem(int id);
    void DeleteAllOrderItems(int orderId);
}