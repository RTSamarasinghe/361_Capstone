using DataContracts;
using Microsoft.Data.SqlClient;

public class CategoryAccessor : ICategoryAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddCategory(string name)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Category (Name)
            OUTPUT INSERTED.Id
            VALUES (@Name)", conn);

        cmd.Parameters.AddWithValue("@Name", name);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Category GetCategory(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name
            FROM Category
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapCategory(reader);
        }

        return null;
    }

    public List<Category> GetAllCategories()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name
            FROM Category", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Category> categories = new List<Category>();
        while (reader.Read())
        {
            categories.Add(MapCategory(reader));
        }

        return categories;
    }

    public void UpdateCategory(int id, string name)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Category
            SET Name = @Name
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", name);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteCategory(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Category
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Category MapCategory(SqlDataReader reader)
    {
        return new Category
        {
            Id   = (int)reader["Id"],
            Name = (string)reader["Name"]
        };
    }
}
