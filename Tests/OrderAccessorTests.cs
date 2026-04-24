using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class OrderAccessorTests
    {
        private readonly OrderAccessor _accessor = new OrderAccessor();
        private readonly CustomerAccessor _customerAccessor = new CustomerAccessor();
        private readonly CartAccessor _cartAccessor = new CartAccessor();
        private readonly AddressAccessor _addressAccessor = new AddressAccessor();
        private int _insertedId;
        private int _customerId;
        private int _cartId;
        private int _addressId;

        [TestInitialize]
        public void Setup()
        {
            _cartId = _cartAccessor.AddCart();
            _customerId = _customerAccessor.AddCustomer("Test User", "ordertest@example.com", "hashedpass");
            _addressId = _addressAccessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteOrder(_insertedId);
            }
            _addressAccessor.DeleteAddress(_addressId);
            _customerAccessor.DeleteCustomer(_customerId);
            _cartAccessor.DeleteCart(_cartId);
        }

        [TestMethod]
        public void AddOrder_ReturnsNewId()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetOrder_ReturnsCorrectOrder()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            Order result = _accessor.GetOrder(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(99.99m, result.TotalAmount);
        }

        [TestMethod]
        public void GetOrder_ReturnsNull_WhenNotFound()
        {
            Order result = _accessor.GetOrder(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetOrdersByCustomer_ReturnsCorrectOrders()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            var result = _accessor.GetOrdersByCustomer(_customerId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(o => o.Id == _insertedId));
        }

        [TestMethod]
        public void GetOrdersByStatus_ReturnsCorrectOrders()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            var result = _accessor.GetOrdersByStatus("Pending");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(o => o.Id == _insertedId));
        }

        [TestMethod]
        public void UpdateOrderStatus_UpdatesStatus()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            _accessor.UpdateOrderStatus(_insertedId, "Shipped");
            Order result = _accessor.GetOrder(_insertedId);
            Assert.AreEqual("Shipped", result.OrderStatus);
        }

        [TestMethod]
        public void UpdateOrderTotalAmount_UpdatesTotal()
        {
            _insertedId = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            _accessor.UpdateOrderTotalAmount(_insertedId, 149.99m);
            Order result = _accessor.GetOrder(_insertedId);
            Assert.AreEqual(149.99m, result.TotalAmount);
        }

        [TestMethod]
        public void DeleteOrder_RemovesOrder()
        {
            int id = _accessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
            _accessor.DeleteOrder(id);
            Order result = _accessor.GetOrder(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}