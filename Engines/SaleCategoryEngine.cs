using DataContracts;

public class SaleCategoryEngine : ISaleCategoryEngine {

	private readonly ISaleCategoryAccessor _saleCategoryAccessor;

	public SaleCategoryEngine(ISaleCategoryAccessor saleCategoryAccessor)
	{
		_saleCategoryAccessor = saleCategoryAccessor;
	}

	public int AddSaleCategory(int saleId, int categoryId)
	{
		if (saleId <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		if (categoryId <= 0) {
			throw new ArgumentException("Category id cannot be less than or equal to zero");
		}
		return _saleCategoryAccessor.AddSaleCategory(saleId, categoryId);
	}

	public SaleCategory GetSaleCategory(int id)
	{
		if (id <= 0) {
			throw new ArgumentException("Sale category id cannot be less than or equal to zero");
		}
		
		SaleCategory saleCategory = _saleCategoryAccessor.GetSaleCategory(id);

		if (saleCategory == null) {
			throw new Exception("Sale category does not exist");
		}
		return saleCategory;
	}

	public List<SaleCategory> GetSaleCategoriesBySale(int saleId)
	{
		if (saleId <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		return _saleCategoryAccessor.GetSaleCategoriesBySale(saleId);
	}

	public List<SaleCategory> GetSaleCategoriesByCategory(int categoryId)
	{
		if (categoryId <= 0) {
			throw new ArgumentException("Category id cannot be less than or equal to zero");
		}
		return _saleCategoryAccessor.GetSaleCategoriesByCategory(categoryId);
	}

	public void DeleteSaleCategory(int id)
	{
		if (id <= 0) {
			throw new ArgumentException("Sale category id cannot be less than or equal to zero");
		}

		if (GetSaleCategory(id) != null) {
			_saleCategoryAccessor.DeleteSaleCategory(id);
		} else {
			throw new Exception("Sale category does not exist");
		}
	}

	public void DeleteAllSaleCategories(int saleId)
	{
		if (saleId <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		_saleCategoryAccessor.DeleteAllSaleCategories(saleId);
	}
}
