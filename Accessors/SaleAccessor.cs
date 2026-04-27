using DataContracts;
using Microsoft.Data.SqlClient;

public class SaleAccessor : ISaleAccessor
{
    private readonly string _connectionString;

    public SaleAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddSale(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Sale (StartDate, EndDate, DiscountAmount, DiscountPercent)
            OUTPUT INSERTED.Id
            VALUES (@StartDate, @EndDate, @DiscountAmount, @DiscountPercent)", conn);

        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DiscountAmount", (object)discountAmount ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DiscountPercent", (object)discountPercent ?? DBNull.Value);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public Sale GetSale(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, StartDate, EndDate, DiscountAmount, DiscountPercent
            FROM Sale
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapSale(reader);
        }

        return null;
    }

    public List<Sale> GetAllSales()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, StartDate, EndDate, DiscountAmount, DiscountPercent
            FROM Sale", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Sale> sales = new List<Sale>();
        while (reader.Read())
        {
            sales.Add(MapSale(reader));
        }

        return sales;
    }

    public List<Sale> GetActiveSales()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, StartDate, EndDate, DiscountAmount, DiscountPercent
            FROM Sale
            WHERE StartDate <= GETDATE()
            AND (EndDate IS NULL OR EndDate >= GETDATE())", conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<Sale> sales = new List<Sale>();
        while (reader.Read())
        {
            sales.Add(MapSale(reader));
        }

        return sales;
    }

    public void UpdateSale(int id, DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE Sale
            SET StartDate       = @StartDate,
                EndDate         = @EndDate,
                DiscountAmount  = @DiscountAmount,
                DiscountPercent = @DiscountPercent
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DiscountAmount", (object)discountAmount ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DiscountPercent", (object)discountPercent ?? DBNull.Value);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteSale(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM Sale
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static Sale MapSale(SqlDataReader reader)
    {
        return new Sale
        {
            Id              = (int)reader["Id"],
            StartDate       = (DateTime)reader["StartDate"],
            EndDate         = reader["EndDate"] == DBNull.Value ? null : (DateTime?)reader["EndDate"],
            DiscountAmount  = reader["DiscountAmount"] == DBNull.Value ? null : (decimal?)reader["DiscountAmount"],
            DiscountPercent = reader["DiscountPercent"] == DBNull.Value ? null : (decimal?)reader["DiscountPercent"]
        };
    }
}