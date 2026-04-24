using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class CustomerAccessorTests
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly CustomerAccessor _accessor = new CustomerAccessor(ConnectionString);
        private readonly CartAccessor _cartAccessor = new CartAccessor(ConnectionString);
        private int _insertedId;
        private int _cartId;

        [TestInitialize]
        public void Setup()
        {
            _cartId = _cartAccessor.AddCart();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteCustomer(_insertedId);
            }
            _cartAccessor.DeleteCart(_cartId);
        }

        [TestMethod]
        public void AddCustomer_ReturnsNewId()
        {
            _insertedId = _accessor.AddCustomer("Test User", "test@example.com", "hashedpass");
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetCustomer_ReturnsCorrectCustomer()
        {
            _insertedId = _accessor.AddCustomer("Test User", "test2@example.com", "hashedpass");
            Customer result = _accessor.GetCustomer(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual("Test User", result.Name);
        }

        [TestMethod]
        public void GetCustomer_ReturnsNull_WhenNotFound()
        {
            Customer result = _accessor.GetCustomer(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomerByEmail_ReturnsCorrectCustomer()
        {
            _insertedId = _accessor.AddCustomer("Test User", "unique@example.com", "hashedpass");
            Customer result = _accessor.GetCustomerByEmail("unique@example.com");
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
        }

        [TestMethod]
        public void GetCustomerByEmail_ReturnsNull_WhenNotFound()
        {
            Customer result = _accessor.GetCustomerByEmail("nonexistent@example.com");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllCustomers_ReturnsList()
        {
            _insertedId = _accessor.AddCustomer("Test User", "test3@example.com", "hashedpass");
            var result = _accessor.GetAllCustomers();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void UpdateCustomer_UpdatesFields()
        {
            _insertedId = _accessor.AddCustomer("Old Name", "test4@example.com", "hashedpass");
            _accessor.UpdateCustomer(_insertedId, "New Name", "test4@example.com", "newhashedpass");
            Customer result = _accessor.GetCustomer(_insertedId);
            Assert.AreEqual("New Name", result.Name);
        }

        [TestMethod]
        public void UpdateCustomerCart_UpdatesCart()
        {
            _insertedId = _accessor.AddCustomer("Test User", "test5@example.com", "hashedpass");
            int newCartId = _cartAccessor.AddCart();
            _accessor.UpdateCustomerCart(_insertedId, newCartId);
            Customer result = _accessor.GetCustomer(_insertedId);
            Assert.AreEqual(newCartId, result.UserCart);
            _cartAccessor.DeleteCart(newCartId);
        }

        [TestMethod]
        public void UpdateCustomerPaymentMethod_UpdatesPaymentMethod()
        {
            _insertedId = _accessor.AddCustomer("Test User", "test6@example.com", "hashedpass");
            var pmAccessor = new PaymentMethodAccessor(ConnectionString);
            int pmId = pmAccessor.AddPaymentMethod("hash", DateTime.Now.AddYears(2), "Test User", "pinhash");
            _accessor.UpdateCustomerPaymentMethod(_insertedId, pmId);
            Customer result = _accessor.GetCustomer(_insertedId);
            Assert.AreEqual(pmId, result.PaymentMethodId);
            pmAccessor.DeletePaymentMethod(pmId);
        }

        [TestMethod]
        public void DeleteCustomer_RemovesCustomer()
        {
            int id = _accessor.AddCustomer("To Delete", "delete@example.com", "hashedpass");
            _accessor.DeleteCustomer(id);
            Customer result = _accessor.GetCustomer(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}