using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class PaymentAccessorTests
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly PaymentAccessor _accessor = new PaymentAccessor(ConnectionString);
        private readonly OrderAccessor _orderAccessor = new OrderAccessor(ConnectionString);
        private readonly CustomerAccessor _customerAccessor = new CustomerAccessor(ConnectionString);
        private readonly CartAccessor _cartAccessor = new CartAccessor(ConnectionString);
        private readonly AddressAccessor _addressAccessor = new AddressAccessor(ConnectionString);
        private readonly PaymentMethodAccessor _paymentMethodAccessor = new PaymentMethodAccessor(ConnectionString);
        private int _insertedId;
        private int _orderId;
        private int _customerId;
        private int _cartId;
        private int _addressId;
        private int _paymentMethodId;

        [TestInitialize]
        public void Setup()
        {
            _cartId = _cartAccessor.AddCart();
            _customerId = _customerAccessor.AddCustomer("Test User", "paymenttest@example.com", "hashedpass");
            _addressId = _addressAccessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            _orderId = _orderAccessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            _paymentMethodId = _paymentMethodAccessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "Test User", "hashedpin");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeletePayment(_insertedId);
            }
            _orderAccessor.DeleteOrder(_orderId);
            _addressAccessor.DeleteAddress(_addressId);
            _customerAccessor.DeleteCustomer(_customerId);
            _cartAccessor.DeleteCart(_cartId);
            _paymentMethodAccessor.DeletePaymentMethod(_paymentMethodId);
        }

        [TestMethod]
        public void AddPayment_ReturnsNewId()
        {
            _insertedId = _accessor.AddPayment(_orderId, 99.99m, DateTime.Now, _paymentMethodId);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetPayment_ReturnsCorrectPayment()
        {
            _insertedId = _accessor.AddPayment(_orderId, 99.99m, DateTime.Now, _paymentMethodId);
            Payment result = _accessor.GetPayment(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(99.99m, result.Amount);
        }

        [TestMethod]
        public void GetPayment_ReturnsNull_WhenNotFound()
        {
            Payment result = _accessor.GetPayment(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPaymentByOrder_ReturnsCorrectPayment()
        {
            _insertedId = _accessor.AddPayment(_orderId, 99.99m, DateTime.Now, _paymentMethodId);
            Payment result = _accessor.GetPaymentByOrder(_orderId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_orderId, result.OrderId);
        }

        [TestMethod]
        public void GetPaymentByOrder_ReturnsNull_WhenNotFound()
        {
            Payment result = _accessor.GetPaymentByOrder(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllPayments_ReturnsList()
        {
            _insertedId = _accessor.AddPayment(_orderId, 99.99m, DateTime.Now, _paymentMethodId);
            var result = _accessor.GetAllPayments();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void DeletePayment_RemovesPayment()
        {
            int id = _accessor.AddPayment(_orderId, 99.99m, DateTime.Now, _paymentMethodId);
            _accessor.DeletePayment(id);
            Payment result = _accessor.GetPayment(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}
