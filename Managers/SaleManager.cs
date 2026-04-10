using DataContracts;

public class SaleManager : ISaleManager
{
    private readonly ISaleEngine _saleEngine;

    public SaleManager(ISaleEngine saleEngine)
    {
        _saleEngine = saleEngine;
    }

    public int AddSale(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        return _saleEngine.AddSale(startDate, endDate, discountAmount, discountPercent);
    }

    public Sale GetSale(int id)
    {
        return _saleEngine.GetSale(id);
    }

    public List<Sale> GetAllSales()
    {
        return _saleEngine.GetAllSales();
    }

    public List<Sale> GetActiveSales()
    {
        return _saleEngine.GetActiveSales();
    }

    public void UpdateSale(int id, DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        _saleEngine.UpdateSale(id, startDate, endDate, discountAmount, discountPercent);
    }

    public void DeleteSale(int id)
    {
        _saleEngine.DeleteSale(id);
    }
}