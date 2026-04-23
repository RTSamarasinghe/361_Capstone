using DataContracts;
using Engines;
namespace Managers;

    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerEngine _customerEngine;

        public CustomerManager(ICustomerEngine customerEngine)
        {
            _customerEngine = customerEngine;
        }

        public int AddCustomer(string name, string email, string passHash)
        {
            
            return _customerEngine.AddCustomer(name, email, passHash);
        }

        public string Login(string email, string password)
        {
            return _customerEngine.Login(email, password);
        }

    public Customer GetCustomer(int id)
        {
            return _customerEngine.GetCustomer(id);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _customerEngine.GetCustomerByEmail(email);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerEngine.GetAllCustomers();
        }

        public void UpdateCustomer(int id, string name, string email, string passHash)
        {
            _customerEngine.UpdateCustomer(id, name, email, passHash);
        }

        public void UpdateCustomerCart(int id, int cartId)
        {
            _customerEngine.UpdateCustomerCart(id, cartId);
        }

        public void UpdateCustomerPaymentMethod(int id, int paymentMethodId)
        {
            _customerEngine.UpdateCustomerPaymentMethod(id, paymentMethodId);
        }

        public void DeleteCustomer(int id)
        {
            _customerEngine.DeleteCustomer(id);
        }
    }
