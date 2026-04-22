using DataContracts;

public class SaleItemEngine : ISaleItemEngine
{

	private readonly ISaleItemAccessor _saleItemAccessor;

	public SaleItemEngine(ISaleItemAccessor saleItemAccessor)
	{
		_saleItemAccessor = saleItemAccessor;
	}

	public int AddSaleItem(int saleId, int productId)
	{
		if (saleId <= 0)
		{
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		if (productId <= 0)
		{
			throw new ArgumentException("Product id cannot be less than or equal to zero");
		}
		return _saleItemAccessor.AddSaleItem(saleId, productId);
	}

	public SaleItem GetSaleItem(int id)
	{
		if (id <= 0)
		{
			throw new ArgumentException("Sale item id cannot be less than or equal to zero");
		}

		SaleItem saleItem = _saleItemAccessor.GetSaleItem(id);

		if (saleItem == null)
		{
			throw new Exception("Sale item does not exist");
		}
		return saleItem;
	}

	public List<SaleItem> GetSaleItemsBySale(int saleId)
	{
		if (saleId <= 0)
		{
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		return _saleItemAccessor.GetSaleItemsBySale(saleId);
	}

	public List<SaleItem> GetSaleItemsByProduct(int productId)
	{
		if (productId <= 0)
		{
			throw new ArgumentException("Product id cannot be less than or equal to zero");
		}
		return _saleItemAccessor.GetSaleItemsByProduct(productId);
	}

	public void DeleteSaleItem(int id)
	{
		if (id <= 0)
		{
			throw new ArgumentException("Sale item id cannot be less than or equal to zero");
		}

		if (GetSaleItem(id) != null)
		{
			_saleItemAccessor.DeleteSaleItem(id);
		}
		else
		{
			throw new Exception("Sale item does not exist");
		}
	}

	public void DeleteAllSaleItems(int saleId)
	{
		if (saleId <= 0)
		{
			throw new ArgumentException("Sale id cannot be less than or equal to zero");
		}
		_saleItemAccessor.DeleteAllSaleItems(saleId);
	}
}
