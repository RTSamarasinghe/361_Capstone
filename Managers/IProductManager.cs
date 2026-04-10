using DataContracts;

public interface IProductManager
{
    int AddProduct(string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity);
    Product GetProduct(int id);
    List<Product> GetAllProducts();
    List<Product> GetProductsByCategory(int categoryId);
    void UpdateProduct(int id, string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity);
    void UpdateStockQuantity(int id, int stockQuantity);
    void DeleteProduct(int id);
}