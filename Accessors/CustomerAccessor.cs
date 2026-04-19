using DataContracts;
using Microsoft.Data.SqlClient;

public class CustomerAccessor : ICustomerAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddCustomer(string name, string email, string passHash, int cartId, int paymentMethodId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Customer (Name, Email, PassHash, UserCart, PaymentMethodId)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Email, @PassHash, @CartId, @PaymentMethodId)", conn);

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@PassHash", passHash);
        cmd.Parameters.AddWithValue("@CartId", cartId);
        cmd.Parameters.AddWithValue("@PaymentMethodId", (object)paymentMethodId ?? DBNull.Value);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Customer GetCustomer(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Email, Created, PassHash, UserCart, PaymentMethodId
            FROM Customer
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapCustomer(reader);
        }

        return null;
    }

    public Customer GetCustomerByEmail(string email)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Email, Created, PassHash, UserCart, PaymentMethodId
            FROM Customer
            WHERE Email = @Email", conn);

        cmd.Parameters.AddWithValue("@Email", email);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapCustomer(reader);
        }

        return null;
    }

    public List<Customer> GetAllCustomers()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Email, Created, PassHash, UserCart, PaymentMethodId
            FROM Customer", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Customer> customers = new List<Customer>();
        while (reader.Read())
        {
            customers.Add(MapCustomer(reader));
        }

        return customers;
    }

    public void UpdateCustomer(int id, string name, string email, string passHash)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Customer
            SET Name     = @Name,
                Email    = @Email,
                PassHash = @PassHash
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@PassHash", passHash);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void UpdateCustomerCart(int id, int cartId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Customer
            SET UserCart = @CartId
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@CartId", cartId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void UpdateCustomerPaymentMethod(int id, int paymentMethodId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Customer
            SET PaymentMethodId = @PaymentMethodId
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@PaymentMethodId", paymentMethodId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteCustomer(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Customer
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Customer MapCustomer(SqlDataReader reader)
    {
        return new Customer
        {
            Id              = (int)reader["Id"],
            Name            = (string)reader["Name"],
            Email           = (string)reader["Email"],
            Created         = (DateTime)reader["Created"],
            PassHash        = (string)reader["PassHash"],
            UserCart        = (int)reader["UserCart"],
            PaymentMethodId = (int)reader["PaymentMethodId"]
        };
    }
}