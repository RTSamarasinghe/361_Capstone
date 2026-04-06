using DataContracts;
using Microsoft.Data.SqlClient;

public class SaleItemAccessor : ISaleItemAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddSaleItem(int saleId, int productId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO SaleItem (SaleId, ProductId)
            OUTPUT INSERTED.Id
            VALUES (@SaleId, @ProductId)", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);
        cmd.Parameters.AddWithValue("@ProductId", productId);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public SaleItem GetSaleItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, ProductId
            FROM SaleItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapSaleItem(reader);
        }

        return null;
    }

    public List<SaleItem> GetSaleItemsBySale(int saleId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, ProductId
            FROM SaleItem
            WHERE SaleId = @SaleId", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<SaleItem> saleItems = new List<SaleItem>();
        while (reader.Read())
        {
            saleItems.Add(MapSaleItem(reader));
        }

        return saleItems;
    }

    public List<SaleItem> GetSaleItemsByProduct(int productId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, SaleId, ProductId
            FROM SaleItem
            WHERE ProductId = @ProductId", conn);

        cmd.Parameters.AddWithValue("@ProductId", productId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<SaleItem> saleItems = new List<SaleItem>();
        while (reader.Read())
        {
            saleItems.Add(MapSaleItem(reader));
        }

        return saleItems;
    }

    public void DeleteSaleItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM SaleItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllSaleItems(int saleId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM SaleItem
            WHERE SaleId = @SaleId", conn);

        cmd.Parameters.AddWithValue("@SaleId", saleId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static SaleItem MapSaleItem(SqlDataReader reader)
    {
        return new SaleItem
        {
            Id        = (int)reader["Id"],
            SaleId    = (int)reader["SaleId"],
            ProductId = (int)reader["ProductId"]
        };
    }
}