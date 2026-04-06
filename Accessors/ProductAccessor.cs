using DataContracts;
using Microsoft.Data.SqlClient;

public class ProductAccessor : IProductAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddProduct(string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Description, @Price, @CategoryId, @ImageURL, @Manufacturer, @Rating, @Sku, @StockQuantity)", conn);

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Price", price);
        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
        cmd.Parameters.AddWithValue("@ImageURL", (object)imageURL ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Manufacturer", (object)manufacturer ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Rating", (object)rating ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Sku", (object)sku ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Product GetProduct(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity
            FROM Product
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapProduct(reader);
        }

        return null;
    }

    public List<Product> GetAllProducts()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity
            FROM Product", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            products.Add(MapProduct(reader));
        }

        return products;
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity
            FROM Product
            WHERE CategoryId = @CategoryId", conn);

        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Product> products = new List<Product>();
        while (reader.Read())
        {
            products.Add(MapProduct(reader));
        }

        return products;
    }

    public void UpdateProduct(int id, string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Product
            SET Name          = @Name,
                Description   = @Description,
                Price         = @Price,
                CategoryId    = @CategoryId,
                ImageURL      = @ImageURL,
                Manufacturer  = @Manufacturer,
                Rating        = @Rating,
                Sku           = @Sku,
                StockQuantity = @StockQuantity
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Price", price);
        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
        cmd.Parameters.AddWithValue("@ImageURL", (object)imageURL ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Manufacturer", (object)manufacturer ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Rating", (object)rating ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Sku", (object)sku ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void UpdateStockQuantity(int id, int stockQuantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Product
            SET StockQuantity = @StockQuantity
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteProduct(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Product
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Product MapProduct(SqlDataReader reader)
    {
        return new Product
        {
            Id            = (int)reader["Id"],
            Name          = (string)reader["Name"],
            Description   = reader["Description"] == DBNull.Value ? null : (string)reader["Description"],
            Price         = (decimal)reader["Price"],
            CategoryId    = (int)reader["CategoryId"],
            ImageURL      = reader["ImageURL"] == DBNull.Value ? null : (string)reader["ImageURL"],
            Manufacturer  = reader["Manufacturer"] == DBNull.Value ? null : (string)reader["Manufacturer"],
            Rating        = reader["Rating"] == DBNull.Value ? null : (decimal?)reader["Rating"],
            Sku           = reader["Sku"] == DBNull.Value ? null : (string)reader["Sku"],
            StockQuantity = (int)reader["StockQuantity"]
        };
    }
}