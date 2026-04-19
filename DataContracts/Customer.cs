namespace DataContracts;
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public string PassHash { get; set; }
    public int UserCart { get; set; }
    public int? PaymentMethodId { get; set; }
}
