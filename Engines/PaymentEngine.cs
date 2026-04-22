using DataContracts;

public class PaymentEngine : IPaymentEngine
{
	private readonly IPaymentAccessor _paymentAccessor;

	public PaymentEngine(IPaymentAccessor paymentAccessor)
	{
		_paymentAccessor = paymentAccessor;
	}

	public int AddPayment(int orderId, decimal amount, DateTime paymentDate, int paymentMethod) {

		if (orderId <= 0) {
			throw new ArgumentException("Order id must be greater than 0.");
		}

		if (amount <= 0) {
			throw new ArgumentException("Amount must be greater than 0");
		}

		if(paymentMethod <= 0) {
			throw new ArgumentException("Payment method id must be greater than 0");
		}

		Payment ExisingPayment = GetPaymentByOrder(orderId);
		if (ExisingPayment != null) {
			throw new ArgumentException("A payment already exists for this order");
		}

		return _paymentAccessor.AddPayment(orderId, amount, paymentDate, paymentMethod);
	}

	public Payment GetPayment(int id) {

		if (id <= 0)
		{
			throw new ArgumentException("Payment id must be greater than 0.");
		}

		Payment payment = _paymentAccessor.GetPayment(id);

		if (payment == null)
		{
			throw new Exception("Payment not found.");
		}

		return payment;
	}

	public Payment GetPaymentByOrder(int orderId) {

		if (orderId <= 0)
		{
			throw new ArgumentException("Order id must be greater than 0.");
		}

		Payment payment = _paymentAccessor.GetPayment(orderId);

		if (payment == null)
		{
			throw new Exception("Payment not found.");
		}

		return payment;
	}

	public List<Payment> GetAllPayments() {

		return _paymentAccessor.GetAllPayments();

	}

	public void DeletePayment(int id) {
		if(_paymentAccessor.GetPayment(id) != null) {

			_paymentAccessor.DeletePayment(id);

		}
	}
}
