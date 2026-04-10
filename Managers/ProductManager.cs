using DataContracts;

public class ProductManager : IProductManager
{
    private readonly IProductEngine _productEngine;

    public ProductManager(IProductEngine productEngine)
    {
        _productEngine = productEngine;
    }

    public int AddProduct(string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity)
    {
        return _productEngine.AddProduct(name, description, price, categoryId, imageURL, manufacturer, rating, sku, stockQuantity);
    }

    public Product GetProduct(int id)
    {
        return _productEngine.GetProduct(id);
    }

    public List<Product> GetAllProducts()
    {
        return _productEngine.GetAllProducts();
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        return _productEngine.GetProductsByCategory(categoryId);
    }

    public void UpdateProduct(int id, string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity)
    {
        _productEngine.UpdateProduct(id, name, description, price, categoryId, imageURL, manufacturer, rating, sku, stockQuantity);
    }

    public void UpdateStockQuantity(int id, int stockQuantity)
    {
        _productEngine.UpdateStockQuantity(id, stockQuantity);
    }

    public void DeleteProduct(int id)
    {
        _productEngine.DeleteProduct(id);
    }
}