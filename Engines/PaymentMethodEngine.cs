using DataContracts;

public class PaymentMethodEngine : IPaymentMethodEngine {

	private readonly IPaymentMethodAccessor _paymentMethodAccessor;
	public PaymentMethodEngine(IPaymentMethodAccessor paymentMethodAccessor) 
	{
		_paymentMethodAccessor = paymentMethodAccessor;
	}

	public int AddPaymentMethod(string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash) 
	{
		ValidatePaymentMethodInput(cardNumberHash, cardholderName, pinHash);

		return _paymentMethodAccessor.AddPaymentMethod(cardNumberHash, expirationDate, cardholderName, pinHash);
	}

	public PaymentMethod GetPaymentMethod(int id)
	{
		if (id <= 0)
		{
			throw new ArgumentException("Payment method id must be greater than 0.");
		}

		PaymentMethod paymentMethod = _paymentMethodAccessor.GetPaymentMethod(id);

		if (paymentMethod == null)
		{
			throw new Exception("Payment method not found.");
		}

		return paymentMethod;
	}

	public List<PaymentMethod> GetAllPaymentMethods() 
	{
		return _paymentMethodAccessor.GetAllPaymentMethods();
	}

	public void UpdatePaymentMethod(int id, string cardNumberHash, DateTime expirationDate, string cardholderName, string pinHash)
	{
		ValidatePaymentMethodInput(cardNumberHash, cardholderName, pinHash);

		if(GetPaymentMethod(id) != null) {
			_paymentMethodAccessor.UpdatePaymentMethod(id, cardNumberHash, expirationDate, cardholderName, pinHash);
		}
	}

	public void DeletePaymentMethod(int id) 
	{
		if(GetPaymentMethod(id) != null) {
			_paymentMethodAccessor.DeletePaymentMethod(id);
		}
	}

	private static void ValidatePaymentMethodInput(string cardNumberHash, string cardholderName, string pinHash)
	{
		if (string.IsNullOrWhiteSpace(cardNumberHash))
		{
			throw new ArgumentException("Name cannot be empty.");
		}

		if (string.IsNullOrWhiteSpace(cardholderName))
		{
			throw new ArgumentException("Email cannot be empty.");
		}

		if (string.IsNullOrWhiteSpace(pinHash))
		{
			throw new ArgumentException("Password hash cannot be empty.");
		}
	}
}
