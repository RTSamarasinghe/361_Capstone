using DataContracts;
using Microsoft.Data.SqlClient;

public class OrderAccessor : IOrderAccessor
{
    private readonly string _connectionString;

    public OrderAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddOrder(int customerId, decimal totalAmount, string orderStatus, int shippingAddressId, int billingAddressId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO [Order] (CustomerId, TotalAmount, OrderStatus, ShippingAddressId, BillingAddressId)
            OUTPUT INSERTED.Id
            VALUES (@CustomerId, @TotalAmount, @OrderStatus, @ShippingAddressId, @BillingAddressId)", conn);

        cmd.Parameters.AddWithValue("@CustomerId", customerId);
        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
        cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
        cmd.Parameters.AddWithValue("@ShippingAddressId", shippingAddressId);
        cmd.Parameters.AddWithValue("@BillingAddressId", billingAddressId);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Order GetOrder(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CustomerId, OrderDate, TotalAmount, OrderStatus, ShippingAddressId, BillingAddressId
            FROM [Order]
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapOrder(reader);
        }

        return null;
    }

    public List<Order> GetOrdersByCustomer(int customerId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CustomerId, OrderDate, TotalAmount, OrderStatus, ShippingAddressId, BillingAddressId
            FROM [Order]
            WHERE CustomerId = @CustomerId", conn);

        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Order> orders = new List<Order>();
        while (reader.Read())
        {
            orders.Add(MapOrder(reader));
        }

        return orders;
    }

    public List<Order> GetOrdersByStatus(string orderStatus)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CustomerId, OrderDate, TotalAmount, OrderStatus, ShippingAddressId, BillingAddressId
            FROM [Order]
            WHERE OrderStatus = @OrderStatus", conn);

        cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Order> orders = new List<Order>();
        while (reader.Read())
        {
            orders.Add(MapOrder(reader));
        }

        return orders;
    }

    public void UpdateOrderStatus(int id, string orderStatus)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE [Order]
            SET OrderStatus = @OrderStatus
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void UpdateOrderTotalAmount(int id, decimal totalAmount)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE [Order]
            SET TotalAmount = @TotalAmount
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteOrder(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM [Order]
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Order MapOrder(SqlDataReader reader)
    {
        return new Order
        {
            Id                = (int)reader["Id"],
            CustomerId        = (int)reader["CustomerId"],
            OrderDate         = (DateTime)reader["OrderDate"],
            TotalAmount       = (decimal)reader["TotalAmount"],
            OrderStatus       = (string)reader["OrderStatus"],
            ShippingAddressId = (int)reader["ShippingAddressId"],
            BillingAddressId  = (int)reader["BillingAddressId"]
        };
    }
}