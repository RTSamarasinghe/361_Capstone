using DataContracts;

public class CustomerEngine : ICustomerEngine
{
    private readonly ICustomerAccessor _customerAccessor;

    public CustomerEngine(ICustomerAccessor customerAccessor)
    {
        _customerAccessor = customerAccessor;
    }

    public int AddCustomer(string name, string email, string passHash, int cartId, int paymentMethodId)
    {
        ValidateCustomerInput(name, email, passHash);

        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        if (paymentMethodId <= 0)
        {
            throw new ArgumentException("Payment method id must be greater than 0.");
        }

        Customer existingCustomer = _customerAccessor.GetCustomerByEmail(email.Trim());
        if (existingCustomer != null)
        {
            throw new ArgumentException("A customer with this email already exists.");
        }

        return _customerAccessor.AddCustomer(
            name.Trim(),
            email.Trim(),
            passHash,
            cartId,
            paymentMethodId);
    }

    public Customer GetCustomer(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        Customer customer = _customerAccessor.GetCustomer(id);

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        return customer;
    }

    public Customer GetCustomerByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        Customer customer = _customerAccessor.GetCustomerByEmail(email.Trim());

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        return customer;
    }

    public List<Customer> GetAllCustomers()
    {
        return _customerAccessor.GetAllCustomers();
    }

    public void UpdateCustomer(int id, string name, string email, string passHash)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        ValidateCustomerInput(name, email, passHash);

        Customer existingCustomer = _customerAccessor.GetCustomer(id);
        if (existingCustomer == null)
        {
            throw new Exception("Customer not found.");
        }

        Customer emailOwner = _customerAccessor.GetCustomerByEmail(email.Trim());
        if (emailOwner != null && emailOwner.Id != id)
        {
            throw new ArgumentException("Another customer already uses this email.");
        }

        _customerAccessor.UpdateCustomer(id, name.Trim(), email.Trim(), passHash);
    }

    public void UpdateCustomerCart(int id, int cartId)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        if (cartId <= 0)
        {
            throw new ArgumentException("Cart id must be greater than 0.");
        }

        Customer existingCustomer = _customerAccessor.GetCustomer(id);
        if (existingCustomer == null)
        {
            throw new Exception("Customer not found.");
        }

        _customerAccessor.UpdateCustomerCart(id, cartId);
    }

    public void UpdateCustomerPaymentMethod(int id, int paymentMethodId)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        if (paymentMethodId <= 0)
        {
            throw new ArgumentException("Payment method id must be greater than 0.");
        }

        Customer existingCustomer = _customerAccessor.GetCustomer(id);
        if (existingCustomer == null)
        {
            throw new Exception("Customer not found.");
        }

        _customerAccessor.UpdateCustomerPaymentMethod(id, paymentMethodId);
    }

    public void DeleteCustomer(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        Customer existingCustomer = _customerAccessor.GetCustomer(id);
        if (existingCustomer == null)
        {
            throw new Exception("Customer not found.");
        }

        _customerAccessor.DeleteCustomer(id);
    }

    private static void ValidateCustomerInput(string name, string email, string passHash)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(passHash))
        {
            throw new ArgumentException("Password hash cannot be empty.");
        }
    }
}