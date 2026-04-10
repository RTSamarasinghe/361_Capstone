using DataContracts;

public class PaymentManager : IPaymentManager
{
    private readonly IPaymentEngine _paymentEngine;

    public PaymentManager(IPaymentEngine paymentEngine)
    {
        _paymentEngine = paymentEngine;
    }

    public int AddPayment(int orderId, decimal amount, DateTime paymentDate, int paymentMethod)
    {
        return _paymentEngine.AddPayment(orderId, amount, paymentDate, paymentMethod);
    }

    public Payment GetPayment(int id)
    {
        return _paymentEngine.GetPayment(id);
    }

    public Payment GetPaymentByOrder(int orderId)
    {
        return _paymentEngine.GetPaymentByOrder(orderId);
    }

    public List<Payment> GetAllPayments()
    {
        return _paymentEngine.GetAllPayments();
    }

    public void DeletePayment(int id)
    {
        _paymentEngine.DeletePayment(id);
    }
}