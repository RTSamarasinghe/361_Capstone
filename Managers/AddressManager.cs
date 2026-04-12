using DataContracts;

public class AddressManager : IAddressManager
{
    private readonly IAddressEngine _addressEngine;

    public AddressManager(IAddressEngine addressEngine)
    {
        _addressEngine = addressEngine;
    }

    public int AddAddress(int customerId, string street, string city, string state, string postalCode, string country)
    {
        return _addressEngine.AddAddress(customerId, street, city, state, postalCode, country);
    }

    public Address GetAddress(int id)
    {
        return _addressEngine.GetAddress(id);
    }

    public List<Address> GetAddressesByCustomer(int customerId)
    {
        return _addressEngine.GetAddressesByCustomer(customerId);
    }

    public void UpdateAddress(int id, string street, string city, string state, string postalCode, string country)
    {
        _addressEngine.UpdateAddress(id, street, city, state, postalCode, country);
    }

    public void DeleteAddress(int id)
    {
        _addressEngine.DeleteAddress(id);
    }

    public void DeleteAllAddresses(int customerId)
    {
        _addressEngine.DeleteAllAddresses(customerId);
    }
}