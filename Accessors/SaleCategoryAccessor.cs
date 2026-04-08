using DataContracts;
using Microsoft.Data.SqlClient;

public class SaleCategoryAccessor : ISaleCategoryAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddSaleCategory(int saleId, int categoryId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO SaleCategory (SaleId, CategoryId)
            OUTPUT INSERTED.Id
            VALUES (@SaleId, @CategoryId)", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);
        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public SaleCategory GetSaleCategory(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, CategoryId
            FROM SaleCategory
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapSaleCategory(reader);
        }

        return null;
    }

    public List<SaleCategory> GetSaleCategoriesBySale(int saleId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, CategoryId
            FROM SaleCategory
            WHERE SaleId = @SaleId", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<SaleCategory> saleCategories = new List<SaleCategory>();
        while (reader.Read())
        {
            saleCategories.Add(MapSaleCategory(reader));
        }

        return saleCategories;
    }

    public List<SaleCategory> GetSaleCategoriesByCategory(int categoryId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, CategoryId
            FROM SaleCategory
            WHERE CategoryId = @CategoryId", conn);

        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<SaleCategory> saleCategories = new List<SaleCategory>();
        while (reader.Read())
        {
            saleCategories.Add(MapSaleCategory(reader));
        }

        return saleCategories;
    }

    public void DeleteSaleCategory(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM SaleCategory
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllSaleCategories(int saleId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM SaleCategory
            WHERE SaleId = @SaleId", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static SaleCategory MapSaleCategory(SqlDataReader reader)
    {
        return new SaleCategory
        {
            Id         = (int)reader["Id"],
            SaleId     = (int)reader["SaleId"],
            CategoryId = (int)reader["CategoryId"]
        };
    }
}