using DataContracts;

public class OrderEngine : IOrderEngine
{
    private readonly IOrderAccessor _orderAccessor;

    public OrderEngine(IOrderAccessor orderAccessor)
    {
        _orderAccessor = orderAccessor;
    }

    public int AddOrder(int customerId, decimal totalAmount, string orderStatus, int shippingAddressId, int billingAddressId)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        if (totalAmount < 0)
        {
            throw new ArgumentException("Total amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(orderStatus))
        {
            throw new ArgumentException("Order status cannot be empty.");
        }

        if (shippingAddressId <= 0 || billingAddressId <= 0)
        {
            throw new ArgumentException("Address ids must be greater than 0.");
        }

        return _orderAccessor.AddOrder(customerId, totalAmount, orderStatus.Trim(), shippingAddressId, billingAddressId);
    }

    public Order GetOrder(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        Order order = _orderAccessor.GetOrder(id);

        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        return order;
    }

    public List<Order> GetOrdersByCustomer(int customerId)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        return _orderAccessor.GetOrdersByCustomer(customerId);
    }

    public List<Order> GetOrdersByStatus(string orderStatus)
    {
        if (string.IsNullOrWhiteSpace(orderStatus))
        {
            throw new ArgumentException("Order status cannot be empty.");
        }

        return _orderAccessor.GetOrdersByStatus(orderStatus.Trim());
    }

    public void UpdateOrderStatus(int id, string orderStatus)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(orderStatus))
        {
            throw new ArgumentException("Order status cannot be empty.");
        }

        Order existingOrder = _orderAccessor.GetOrder(id);
        if (existingOrder == null)
        {
            throw new Exception("Order not found.");
        }

        _orderAccessor.UpdateOrderStatus(id, orderStatus.Trim());
    }

    public void UpdateOrderTotalAmount(int id, decimal totalAmount)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        if (totalAmount < 0)
        {
            throw new ArgumentException("Total amount cannot be negative.");
        }

        Order existingOrder = _orderAccessor.GetOrder(id);
        if (existingOrder == null)
        {
            throw new Exception("Order not found.");
        }

        _orderAccessor.UpdateOrderTotalAmount(id, totalAmount);
    }

    public void DeleteOrder(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order id must be greater than 0.");
        }

        Order existingOrder = _orderAccessor.GetOrder(id);
        if (existingOrder == null)
        {
            throw new Exception("Order not found.");
        }

        _orderAccessor.DeleteOrder(id);
    }
}