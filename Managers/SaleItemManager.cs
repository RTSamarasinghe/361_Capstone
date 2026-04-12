using DataContracts;

public class SaleItemManager : ISaleItemManager
{
    private readonly ISaleItemEngine _saleItemEngine;

    public SaleItemManager(ISaleItemEngine saleItemEngine)
    {
        _saleItemEngine = saleItemEngine;
    }

    public int AddSaleItem(int saleId, int productId)
    {
        return _saleItemEngine.AddSaleItem(saleId, productId);
    }

    public SaleItem GetSaleItem(int id)
    {
        return _saleItemEngine.GetSaleItem(id);
    }

    public List<SaleItem> GetSaleItemsBySale(int saleId)
    {
        return _saleItemEngine.GetSaleItemsBySale(saleId);
    }

    public List<SaleItem> GetSaleItemsByProduct(int productId)
    {
        return _saleItemEngine.GetSaleItemsByProduct(productId);
    }

    public void DeleteSaleItem(int id)
    {
        _saleItemEngine.DeleteSaleItem(id);
    }

    public void DeleteAllSaleItems(int saleId)
    {
        _saleItemEngine.DeleteAllSaleItems(saleId);
    }
}