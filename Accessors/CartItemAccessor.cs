using DataContracts;
using Microsoft.Data.SqlClient;

public class CartItemAccessor : ICartItemAccessor
{
    private readonly string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;";

    public int AddCartItem(int cartId, int productId, int quantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            INSERT INTO CartItem (CartId, ProductId, Quantity)
            OUTPUT INSERTED.Id
            VALUES (@CartId, @ProductId, @Quantity)", conn);

        cmd.Parameters.AddWithValue("@CartId", cartId);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        cmd.Parameters.AddWithValue("@Quantity", quantity);

        conn.Open();
        return (int)cmd.ExecuteScalar();
    }

    public CartItem GetCartItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CartId, ProductId, Quantity
            FROM CartItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapCartItem(reader);
        }

        return null;
    }

    public List<CartItem> GetCartItemsByCart(int cartId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            SELECT Id, CartId, ProductId, Quantity
            FROM CartItem
            WHERE CartId = @CartId", conn);

        cmd.Parameters.AddWithValue("@CartId", cartId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        List<CartItem> cartItems = new List<CartItem>();
        while (reader.Read())
        {
            cartItems.Add(MapCartItem(reader));
        }

        return cartItems;
    }

    public void UpdateCartItemQuantity(int id, int quantity)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            UPDATE CartItem
            SET Quantity = @Quantity
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Quantity", quantity);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteCartItem(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM CartItem
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteAllCartItems(int cartId)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(@"
            DELETE FROM CartItem
            WHERE CartId = @CartId", conn);

        cmd.Parameters.AddWithValue("@CartId", cartId);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    private static CartItem MapCartItem(SqlDataReader reader)
    {
        return new CartItem
        {
            Id        = (int)reader["Id"],
            CartId    = (int)reader["CartId"],
            ProductId = (int)reader["ProductId"],
            Quantity  = (int)reader["Quantity"]
        };
    }
}