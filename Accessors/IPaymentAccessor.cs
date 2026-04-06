using DataContracts;

public interface IPaymentAccessor
{
    int AddPayment(int orderId, decimal amount, DateTime paymentDate, int paymentMethod);
    Payment GetPayment(int id);
    Payment GetPaymentByOrder(int orderId);
    List<Payment> GetAllPayments();
    void DeletePayment(int id);
}