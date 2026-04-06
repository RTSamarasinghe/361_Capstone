namespace DataContracts;
public class PaymentMethod
{
    public int Id { get; set; }
    public string CardNumberHash { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CardholderName { get; set; }
    public string PinHash { get; set; }
}