using DataContracts;

public interface IPaymentMethodAccessor
{
    int AddPaymentMethod(string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash);
    PaymentMethod GetPaymentMethod(int id);
    List<PaymentMethod> GetAllPaymentMethods();
    void UpdatePaymentMethod(int id, string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash);
    void DeletePaymentMethod(int id);
}