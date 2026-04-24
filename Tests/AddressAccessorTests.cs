using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class AddressAccessorTests
    {
        private readonly AddressAccessor _accessor = new AddressAccessor();
        private readonly CustomerAccessor _customerAccessor = new CustomerAccessor();
        private readonly CartAccessor _cartAccessor = new CartAccessor();
        private int _insertedId;
        private int _customerId;
        private int _cartId;

        [TestInitialize]
        public void Setup()
        {
            _cartId = _cartAccessor.AddCart();
            _customerId = _customerAccessor.AddCustomer("Test User", "addrtest@example.com", "hashedpass");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteAddress(_insertedId);
            }
            _customerAccessor.DeleteCustomer(_customerId);
            _cartAccessor.DeleteCart(_cartId);
        }

        [TestMethod]
        public void AddAddress_ReturnsNewId()
        {
            _insertedId = _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetAddress_ReturnsCorrectAddress()
        {
            _insertedId = _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            Address result = _accessor.GetAddress(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual("123 Main St", result.Street);
        }

        [TestMethod]
        public void GetAddress_ReturnsNull_WhenNotFound()
        {
            Address result = _accessor.GetAddress(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAddressesByCustomer_ReturnsCorrectAddresses()
        {
            _insertedId = _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            var result = _accessor.GetAddressesByCustomer(_customerId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(a => a.Id == _insertedId));
        }

        [TestMethod]
        public void UpdateAddress_UpdatesFields()
        {
            _insertedId = _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            _accessor.UpdateAddress(_insertedId, "456 Oak Ave", "Omaha", "NE", "68102", "USA");
            Address result = _accessor.GetAddress(_insertedId);
            Assert.AreEqual("456 Oak Ave", result.Street);
            Assert.AreEqual("Omaha", result.City);
        }

        [TestMethod]
        public void DeleteAddress_RemovesAddress()
        {
            int id = _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            _accessor.DeleteAddress(id);
            Address result = _accessor.GetAddress(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }

        [TestMethod]
        public void DeleteAllAddresses_RemovesAllAddressesForCustomer()
        {
            _accessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            _accessor.AddAddress(_customerId, "456 Oak Ave", "Omaha", "NE", "68102", "USA");
            _accessor.DeleteAllAddresses(_customerId);
            var result = _accessor.GetAddressesByCustomer(_customerId);
            Assert.AreEqual(0, result.Count);
            _insertedId = 0;
        }
    }
}