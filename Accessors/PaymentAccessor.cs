using DataContracts;
using Microsoft.Data.SqlClient;

public class PaymentAccessor : IPaymentAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddPayment(int orderId, decimal amount, DateTime paymentDate, int paymentMethod)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Payment (OrderId, Amount, PaymentDate, PaymentMethod)
            OUTPUT INSERTED.Id
            VALUES (@OrderId, @Amount, @PaymentDate, @PaymentMethod)", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Payment GetPayment(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, OrderId, Amount, PaymentDate, PaymentMethod
            FROM Payment
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapPayment(reader);
        }

        return null;
    }

    public Payment GetPaymentByOrder(int orderId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, OrderId, Amount, PaymentDate, PaymentMethod
            FROM Payment
            WHERE OrderId = @OrderId", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapPayment(reader);
        }

        return null;
    }

    public List<Payment> GetAllPayments()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, OrderId, Amount, PaymentDate, PaymentMethod
            FROM Payment", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Payment> payments = new List<Payment>();
        while (reader.Read())
        {
            payments.Add(MapPayment(reader));
        }

        return payments;
    }

    public void DeletePayment(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Payment
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Payment MapPayment(SqlDataReader reader)
    {
        return new Payment
        {
            Id            = (int)reader["Id"],
            OrderId       = (int)reader["OrderId"],
            Amount        = (decimal)reader["Amount"],
            PaymentDate   = (DateTime)reader["PaymentDate"],
            PaymentMethod = (int)reader["PaymentMethod"]
        };
    }
}