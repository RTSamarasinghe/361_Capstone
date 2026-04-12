using DataContracts;

public class SaleEngine : ISaleEngine {

	private readonly ISaleAccessor _saleAccessor;

	public SaleEngine(ISaleAccessor saleAccessor)
	{
		_saleAccessor = saleAccessor;
	}

	public int AddSale(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
	{
		if(discountAmount < 0) {
			throw new ArgumentException("Discount amount cannot be less than zero");
		}
		if (discountPercent < 0) {
			throw new ArgumentException("Discount percent cannot be less than zero");
		}
		return _saleAccessor.AddSale(startDate, endDate, discountAmount, discountPercent);
	}

	public Sale GetSale(int id)
	{
		if(id <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		
		Sale sale = _saleAccessor.GetSale(id);
		if (sale == null) {
			throw new Exception("Sale does not exist");
		}
		return sale;
	}

	public List<Sale> GetAllSales()
	{
		return _saleAccessor.GetAllSales();
	}

	public List<Sale> GetActiveSales()
	{
		return _saleAccessor.GetActiveSales();
	}

	public void UpdateSale(int id, DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
	{
		if (id <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		if (discountAmount < 0) {
			throw new ArgumentException("Discount amount cannot be less than zero");
		}
		if (discountPercent < 0) {
			throw new ArgumentException("Discount percent cannot be less than zero");
		}
		if(GetSale(id) != null) {
			_saleAccessor.UpdateSale(id, startDate, endDate, discountAmount, discountPercent);
		} else {
			throw new Exception("Sale does not exist");
		}
	}

	public void DeleteSale(int id)
	{
		if (id <= 0) {
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		if (GetSale(id) != null) {
			_saleAccessor.DeleteSale(id);
		} else {
			throw new Exception("Sale does not exist");
		}
	}
}
