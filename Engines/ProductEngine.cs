using DataContracts;

public class ProductEngine : IProductEngine
{

	private readonly IProductAccessor _productAccessor;

	public ProductEngine(IProductAccessor productAccessor)
	{
		_productAccessor = productAccessor;
	}

	public int AddProduct(string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity) 
	{
		ValidateProductInput(name, description, price, categoryId, imageURL, manufacturer, rating, sku, stockQuantity);

		return _productAccessor.AddProduct(name, description, price, categoryId, imageURL,manufacturer, rating, sku, stockQuantity);
	}

	public Product GetProduct(int id) 
	{
		if (id <= 0) {
			throw new ArgumentException("Product id cannot be less than or equal to zero");
		}

		Product product = _productAccessor.GetProduct(id);
		if (product == null) {
			throw new Exception("Product does not exist");
		}

		return product;
	}

	public List<Product> GetAllProducts() 
	{
		return _productAccessor.GetAllProducts();	
	}

	public List<Product> GetProductsByCategory(int categoryId) {
		if (categoryId <= 0)
		{
			throw new ArgumentException("Category id cannot be less than or equal to zero");
		}

		return _productAccessor.GetProductsByCategory(categoryId);
	}

	public void UpdateProduct(int id, string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity) 
	{
		ValidateProductInput(name, description, price, categoryId, imageURL, manufacturer, rating, sku, stockQuantity);

		if (id <= 0) {
			throw new ArgumentException("Product id cannot be less than or equal to zero");
		}

		if(_productAccessor.GetProduct(id) != null) {
			_productAccessor.UpdateProduct(id, name, description, price, categoryId, imageURL, manufacturer, rating, sku, stockQuantity);
		} else {
			throw new ArgumentException("Product does not exist");
		}
	}

	public void UpdateStockQuantity(int id, int stockQuantity)
	{
		if (id <= 0) {
			throw new ArgumentException("Product id cannot be less than or equal to zero");
		}

		if (stockQuantity < 0) {
			throw new ArgumentException("Stock quantity cannot be less than zero");
		}

		if (_productAccessor.GetProduct(id) != null)
		{
			_productAccessor.UpdateStockQuantity(id, stockQuantity);
		} else {
			throw new ArgumentException("Product does not exist");
		}
	}

	public void DeleteProduct(int id)
	{
		if (id <= 0)
		{
			throw new ArgumentException("Product id must be greater than 0.");
		}

		if (GetProduct(id) != null)
		{
			_productAccessor.DeleteProduct(id);
		}
		else
		{
			throw new ArgumentException("Product does not exist");
		}
	}

	private static void ValidateProductInput(string name, string description, decimal price, int categoryId, string imageURL, string manufacturer, decimal? rating, string sku, int stockQuantity)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name cannot be empty.");
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentException("Description cannot be empty.");
		}

		if (price <= 0)
		{
			throw new ArgumentException("Price cannot be less than or equal to zero");
		}

		if (categoryId <= 0)
		{
			throw new ArgumentException("Category id cannot be less than or equal to zero");
		}

		if (string.IsNullOrWhiteSpace(imageURL))
		{
			throw new ArgumentException("Image url cannot be empty.");
		}

		if (string.IsNullOrWhiteSpace(manufacturer))
		{
			throw new ArgumentException("Manufacturer cannot be empty.");
		}

		if (rating < 0)
		{
			throw new ArgumentException("Rating cannot be less than zero");
		}

		if (string.IsNullOrWhiteSpace(sku))
		{
			throw new ArgumentException("Sku cannot be empty.");
		}

		if (stockQuantity < 0)
		{
			throw new ArgumentException("Stock quantity cannot be less than zero");
		}
	}
}
