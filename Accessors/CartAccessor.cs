using DataContracts;
using Microsoft.Data.SqlClient;

public class CartAccessor : ICartAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public int AddCart()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Cart DEFAULT VALUES;
            SELECT SCOPE_IDENTITY();", conn);

        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public Cart GetCart(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id
            FROM Cart
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Cart { Id = (int)reader["Id"] };
        }

        return null;
    }

    public void DeleteCart(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Cart
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }
}