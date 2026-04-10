using DataContracts;

public interface ISaleCategoryManager
{
    int AddSaleCategory(int saleId, int categoryId);
    SaleCategory GetSaleCategory(int id);
    List<SaleCategory> GetSaleCategoriesBySale(int saleId);
    List<SaleCategory> GetSaleCategoriesByCategory(int categoryId);
    void DeleteSaleCategory(int id);
    void DeleteAllSaleCategories(int saleId);
}