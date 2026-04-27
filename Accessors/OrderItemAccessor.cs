using DataContracts;
using Microsoft.Data.SqlClient;

public class OrderItemAccessor : IOrderItemAccessor
{
    private readonly string _connectionString;

    public OrderItemAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddOrderItem(int orderId, int productId, int quantity, decimal unitPrice)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO OrderItem (OrderId, ProductId, Quantity, UnitPrice)
            OUTPUT INSERTED.Id
            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        cmd.Parameters.AddWithValue("@Quantity", quantity);
        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public OrderItem GetOrderItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, OrderId, ProductId, Quantity, UnitPrice
            FROM OrderItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapOrderItem(reader);
        }

        return null;
    }

    public List<OrderItem> GetOrderItemsByOrder(int orderId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, OrderId, ProductId, Quantity, UnitPrice
            FROM OrderItem
            WHERE OrderId = @OrderId", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<OrderItem> orderItems = new List<OrderItem>();
        while (reader.Read())
        {
            orderItems.Add(MapOrderItem(reader));
        }

        return orderItems;
    }

    public void UpdateOrderItemQuantity(int id, int quantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE OrderItem
            SET Quantity = @Quantity
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Quantity", quantity);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteOrderItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM OrderItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllOrderItems(int orderId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM OrderItem
            WHERE OrderId = @OrderId", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static OrderItem MapOrderItem(SqlDataReader reader)
    {
        return new OrderItem
        {
            Id        = (int)reader["Id"],
            OrderId   = (int)reader["OrderId"],
            ProductId = (int)reader["ProductId"],
            Quantity  = (int)reader["Quantity"],
            UnitPrice = (decimal)reader["UnitPrice"]
        };
    }
}