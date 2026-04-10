using DataContracts;

public interface ISaleEngine
{
    int AddSale(DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent);
    Sale GetSale(int id);
    List<Sale> GetAllSales();
    List<Sale> GetActiveSales();
    void UpdateSale(int id, DateTime startDate, DateTime? endDate, decimal? discountAmount, decimal? discountPercent);
    void DeleteSale(int id);
}