using DataContracts;

public class PaymentMethodManager : IPaymentMethodManager
{
    private readonly IPaymentMethodEngine _paymentMethodEngine;

    public PaymentMethodManager(IPaymentMethodEngine paymentMethodEngine)
    {
        _paymentMethodEngine = paymentMethodEngine;
    }

    public int AddPaymentMethod(string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash)
    {
        return _paymentMethodEngine.AddPaymentMethod(cardNumberHash, expirationDate, cardholderName, pinHash);
    }

    public PaymentMethod GetPaymentMethod(int id)
    {
        return _paymentMethodEngine.GetPaymentMethod(id);
    }

    public List<PaymentMethod> GetAllPaymentMethods()
    {
        return _paymentMethodEngine.GetAllPaymentMethods();
    }

    public void UpdatePaymentMethod(int id, string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash)
    {
        _paymentMethodEngine.UpdatePaymentMethod(id, cardNumberHash, expirationDate, cardholderName, pinHash);
    }

    public void DeletePaymentMethod(int id)
    {
        _paymentMethodEngine.DeletePaymentMethod(id);
    }
}