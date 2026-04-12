using DataContracts;

public interface ISaleItemManager
{
    int AddSaleItem(int saleId, int productId);
    SaleItem GetSaleItem(int id);
    List<SaleItem> GetSaleItemsBySale(int saleId);
    List<SaleItem> GetSaleItemsByProduct(int productId);
    void DeleteSaleItem(int id);
    void DeleteAllSaleItems(int saleId);
}