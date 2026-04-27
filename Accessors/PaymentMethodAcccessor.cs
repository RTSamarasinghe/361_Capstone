using DataContracts;
using Microsoft.Data.SqlClient;

public class PaymentMethodAccessor : IPaymentMethodAccessor
{
    private readonly string _connectionString;

    public PaymentMethodAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddPaymentMethod(string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO PaymentMethod (CardNumberHash, ExpirationDate, CardholderName, PinHash)
            OUTPUT INSERTED.Id
            VALUES (@CardNumberHash, @ExpirationDate, @CardholderName, @PinHash)", conn);

        cmd.Parameters.AddWithValue("@CardNumberHash", cardNumberHash);
        cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
        cmd.Parameters.AddWithValue("@CardholderName", cardholderName);
        cmd.Parameters.AddWithValue("@PinHash", pinHash);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public PaymentMethod GetPaymentMethod(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CardNumberHash, ExpirationDate, CardholderName, PinHash
            FROM PaymentMethod
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapPaymentMethod(reader);
        }

        return null;
    }

    public List<PaymentMethod> GetAllPaymentMethods()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CardNumberHash, ExpirationDate, CardholderName, PinHash
            FROM PaymentMethod", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<PaymentMethod> paymentMethods = new List<PaymentMethod>();
        while (reader.Read())
        {
            paymentMethods.Add(MapPaymentMethod(reader));
        }

        return paymentMethods;
    }

    public void UpdatePaymentMethod(int id, string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE PaymentMethod
            SET CardNumberHash = @CardNumberHash,
                ExpirationDate = @ExpirationDate,
                CardholderName = @CardholderName,
                PinHash = @PinHash
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@CardNumberHash", cardNumberHash);
        cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
        cmd.Parameters.AddWithValue("@CardholderName", cardholderName);
        cmd.Parameters.AddWithValue("@PinHash", pinHash);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeletePaymentMethod(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM PaymentMethod
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static PaymentMethod MapPaymentMethod(SqlDataReader reader)
    {
        return new PaymentMethod
        {
            Id             = (int)reader["Id"],
            CardNumberHash = (string)reader["CardNumberHash"],
            ExpirationDate = (DateTime)reader["ExpirationDate"],
            CardholderName = (string)reader["CardholderName"],
            PinHash        = (string)reader["PinHash"]
        };
    }
}