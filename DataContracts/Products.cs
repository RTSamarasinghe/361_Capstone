namespace DataContracts;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string ImageURL { get; set; }
    public string Manufacturer { get; set; }
    public decimal? Rating { get; set; }
    public string Sku { get; set; }
    public int StockQuantity { get; set; }
}