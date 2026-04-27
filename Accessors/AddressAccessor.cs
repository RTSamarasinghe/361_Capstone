using DataContracts;
using Microsoft.Data.SqlClient;

public class AddressAccessor : IAddressAccessor
{
    private readonly string _connectionString;
    public AddressAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddAddress(int customerId, string street, string city, string state, string postalCode, string country)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Address (CustomerId, Street, City, State, PostalCode, Country)
            OUTPUT INSERTED.Id
            VALUES (@CustomerId, @Street, @City, @State, @PostalCode, @Country)", conn);

        cmd.Parameters.AddWithValue("@CustomerId", customerId);
        cmd.Parameters.AddWithValue("@Street", street);
        cmd.Parameters.AddWithValue("@City", city);
        cmd.Parameters.AddWithValue("@State", state);
        cmd.Parameters.AddWithValue("@PostalCode", postalCode);
        cmd.Parameters.AddWithValue("@Country", country);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Address GetAddress(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CustomerId, Street, City, State, PostalCode, Country
            FROM Address
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapAddress(reader);
        }

        return null;
    }

    public List<Address> GetAddressesByCustomer(int customerId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CustomerId, Street, City, State, PostalCode, Country
            FROM Address
            WHERE CustomerId = @CustomerId", conn);

        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Address> addresses = new List<Address>();
        while (reader.Read())
        {
            addresses.Add(MapAddress(reader));
        }

        return addresses;
    }

    public void UpdateAddress(int id, string street, string city, string state, string postalCode, string country)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Address
            SET Street     = @Street,
                City       = @City,
                State      = @State,
                PostalCode = @PostalCode,
                Country    = @Country
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Street", street);
        cmd.Parameters.AddWithValue("@City", city);
        cmd.Parameters.AddWithValue("@State", state);
        cmd.Parameters.AddWithValue("@PostalCode", postalCode);
        cmd.Parameters.AddWithValue("@Country", country);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAddress(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Address
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllAddresses(int customerId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Address
            WHERE CustomerId = @CustomerId", conn);

        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Address MapAddress(SqlDataReader reader)
    {
        return new Address
        {
            Id         = (int)reader["Id"],
            CustomerId = (int)reader["CustomerId"],
            Street     = (string)reader["Street"],
            City       = (string)reader["City"],
            State      = (string)reader["State"],
            PostalCode = (string)reader["PostalCode"],
            Country    = (string)reader["Country"]
        };
    }
}