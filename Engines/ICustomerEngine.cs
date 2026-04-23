using DataContracts;
namespace Engines;
public interface ICustomerEngine
{
    int AddCustomer(string name, string email, string passHash);
    Customer GetCustomer(int id);
    Customer GetCustomerByEmail(string email);
    List<Customer> GetAllCustomers();
    void UpdateCustomer(int id, string name, string email, string passHash);
    void UpdateCustomerCart(int id, int cartId);
    void UpdateCustomerPaymentMethod(int id, int paymentMethodId);
    void DeleteCustomer(int id);

    string HashPassword(string password);

    string Login(string username, string password);
}