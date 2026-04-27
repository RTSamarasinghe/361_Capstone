using DataContracts;

public class AddressEngine : IAddressEngine
{
    private readonly IAddressAccessor _addressAccessor;

    public AddressEngine(IAddressAccessor addressAccessor)
    {
        _addressAccessor = addressAccessor;
    }

    public int AddAddress(int customerId, string street, string city, string state, string postalCode, string country)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        ValidateAddressInput(street, city, state, postalCode, country);

        return _addressAccessor.AddAddress(
            customerId,
            street.Trim(),
            city.Trim(),
            state.Trim(),
            postalCode.Trim(),
            country.Trim());
    }



    public Address GetAddress(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Address id must be greater than 0.");
        }

        Address address = _addressAccessor.GetAddress(id);

        if (address == null)
        {
            throw new Exception("Address not found.");
        }

        return address;
    }

    public List<Address> GetAddressesByCustomer(int customerId)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        return _addressAccessor.GetAddressesByCustomer(customerId);
    }

    public void UpdateAddress(int id, string street, string city, string state, string postalCode, string country)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Address id must be greater than 0.");
        }

        ValidateAddressInput(street, city, state, postalCode, country);

        Address existingAddress = _addressAccessor.GetAddress(id);
        if (existingAddress == null)
        {
            throw new Exception("Address not found.");
        }

        _addressAccessor.UpdateAddress(
            id,
            street.Trim(),
            city.Trim(),
            state.Trim(),
            postalCode.Trim(),
            country.Trim());
    }

    public void DeleteAddress(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Address id must be greater than 0.");
        }

        Address existingAddress = _addressAccessor.GetAddress(id);
        if (existingAddress == null)
        {
            throw new Exception("Address not found.");
        }

        _addressAccessor.DeleteAddress(id);
    }

    public void DeleteAllAddresses(int customerId)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be greater than 0.");
        }

        _addressAccessor.DeleteAllAddresses(customerId);
    }

    private static void ValidateAddressInput(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            throw new ArgumentException("State cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("Postal code cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be empty.");
        }
    }
}