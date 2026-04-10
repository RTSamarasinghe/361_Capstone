using DataContracts;

public class OrderItemEngine : IOrderItemEngine
{
    private readonly IOrderItemAccessor _orderItemAccessor;

    public OrderItemEngine(IOrderItemAccessor orderItemAccessor)
    {
        _orderItemAccessor = orderItemAccessor;
    }

    public int AddOrderItem(int orderId, int productId, int quantity, decimal unitPrice)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        if (productId <= 0)
        {
            throw new ArgumentException("Product id must be greater than 0.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price cannot be negative.");
        }

        return _orderItemAccessor.AddOrderItem(orderId, productId, quantity, unitPrice);
    }

    public OrderItem GetOrderItem(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order item id must be greater than 0.");
        }

        OrderItem orderItem = _orderItemAccessor.GetOrderItem(id);

        if (orderItem == null)
        {
            throw new Exception("Order item not found.");
        }

        return orderItem;
    }

    public List<OrderItem> GetOrderItemsByOrder(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        return _orderItemAccessor.GetOrderItemsByOrder(orderId);
    }

    public void UpdateOrderItemQuantity(int id, int quantity)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order item id must be greater than 0.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }

        OrderItem existingOrderItem = _orderItemAccessor.GetOrderItem(id);
        if (existingOrderItem == null)
        {
            throw new Exception("Order item not found.");
        }

        _orderItemAccessor.UpdateOrderItemQuantity(id, quantity);
    }

    public void DeleteOrderItem(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order item id must be greater than 0.");
        }

        OrderItem existingOrderItem = _orderItemAccessor.GetOrderItem(id);
        if (existingOrderItem == null)
        {
            throw new Exception("Order item not found.");
        }

        _orderItemAccessor.DeleteOrderItem(id);
    }

    public void DeleteAllOrderItems(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        _orderItemAccessor.DeleteAllOrderItems(orderId);
    }
}