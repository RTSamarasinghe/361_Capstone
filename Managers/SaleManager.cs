using DataContracts;

public class SaleManager : ISaleManager
{
    private readonly ISaleAccessor _saleAccessor;

    public SaleManager(ISaleAccessor saleAccessor)
    {
        _saleAccessor = saleAccessor;
    }

    public int AddSale(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        ValidateSaleInput(startDate, endDate, discountAmount, discountPercent);

        return _saleAccessor.AddSale(startDate, endDate, discountAmount, discountPercent);
    }

    public Sale GetSale(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Sale id must be greater than 0.");
        }

        Sale sale = _saleAccessor.GetSale(id);

        if (sale == null)
        {
            throw new Exception("Sale not found.");
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
        if (id <= 0)
        {
            throw new ArgumentException("Sale id must be greater than 0.");
        }

        ValidateSaleInput(startDate, endDate, discountAmount, discountPercent);

        Sale existingSale = _saleAccessor.GetSale(id);
        if (existingSale == null)
        {
            throw new Exception("Sale not found.");
        }

        _saleAccessor.UpdateSale(id, startDate, endDate, discountAmount, discountPercent);
    }

    public void DeleteSale(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Sale id must be greater than 0.");
        }

        Sale existingSale = _saleAccessor.GetSale(id);
        if (existingSale == null)
        {
            throw new Exception("Sale not found.");
        }

        _saleAccessor.DeleteSale(id);
    }

    private static void ValidateSaleInput(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent)
    {
        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new ArgumentException("End date cannot be earlier than start date.");
        }

        if (!discountAmount.HasValue && !discountPercent.HasValue)
        {
            throw new ArgumentException("A sale must have either a discount amount or discount percent.");
        }

        if (discountAmount.HasValue && discountAmount.Value < 0)
        {
            throw new ArgumentException("Discount amount cannot be negative.");
        }

        if (discountPercent.HasValue && (discountPercent.Value < 0 || discountPercent.Value > 100))
        {
            throw new ArgumentException("Discount percent must be between 0 and 100.");
        }
    }
}