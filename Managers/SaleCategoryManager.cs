using DataContracts;

public class SaleCategoryManager : ISaleCategoryManager
{
    private readonly ISaleCategoryEngine _saleCategoryEngine;

    public SaleCategoryManager(ISaleCategoryEngine saleCategoryEngine)
    {
        _saleCategoryEngine = saleCategoryEngine;
    }

    public int AddSaleCategory(int saleId, int categoryId)
    {
        return _saleCategoryEngine.AddSaleCategory(saleId, categoryId);
    }

    public SaleCategory GetSaleCategory(int id)
    {
        return _saleCategoryEngine.GetSaleCategory(id);
    }

    public List<SaleCategory> GetSaleCategoriesBySale(int saleId)
    {
        return _saleCategoryEngine.GetSaleCategoriesBySale(saleId);
    }

    public List<SaleCategory> GetSaleCategoriesByCategory(int categoryId)
    {
        return _saleCategoryEngine.GetSaleCategoriesByCategory(categoryId);
    }

    public void DeleteSaleCategory(int id)
    {
        _saleCategoryEngine.DeleteSaleCategory(id);
    }

    public void DeleteAllSaleCategories(int saleId)
    {
        _saleCategoryEngine.DeleteAllSaleCategories(saleId);
    }
}