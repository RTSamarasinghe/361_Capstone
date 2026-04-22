using DataContracts;

public interface IAddressEngine
{
    int AddAddress(int customerId, string street, string city, string state, string postalCode, string country);
    Address GetAddress(int id);
    List<Address> GetAddressesByCustomer(int customerId);
    void UpdateAddress(int id, string street, string city, string state, string postalCode, string country);
    void DeleteAddress(int id);
    void DeleteAllAddresses(int customerId);
}