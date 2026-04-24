using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class OrderItemAccessorTests
    {
        private readonly OrderItemAccessor _accessor = new OrderItemAccessor();
        private readonly OrderAccessor _orderAccessor = new OrderAccessor();
        private readonly CustomerAccessor _customerAccessor = new CustomerAccessor();
        private readonly CartAccessor _cartAccessor = new CartAccessor();
        private readonly AddressAccessor _addressAccessor = new AddressAccessor();
        private readonly ProductAccessor _productAccessor = new ProductAccessor();
        private readonly CategoryAccessor _categoryAccessor = new CategoryAccessor();
        private int _insertedId;
        private int _orderId;
        private int _customerId;
        private int _cartId;
        private int _addressId;
        private int _productId;
        private int _categoryId;

        [TestInitialize]
        public void Setup()
        {
            _categoryId = _categoryAccessor.AddCategory("Test Category");
            _productId = _productAccessor.AddProduct("Test Product", "Desc", 9.99m, _categoryId, null, null, null, null, 10);
            _cartId = _cartAccessor.AddCart();
            _customerId = _customerAccessor.AddCustomer("Test User", "orderitemtest@example.com", "hashedpass");
            _addressId = _addressAccessor.AddAddress(_customerId, "123 Main St", "Lincoln", "NE", "68501", "USA");
            _orderId = _orderAccessor.AddOrder(_customerId, 99.99m, "Pending", _addressId, _addressId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteOrderItem(_insertedId);
            }
            _orderAccessor.DeleteOrder(_orderId);
            _addressAccessor.DeleteAddress(_addressId);
            _customerAccessor.DeleteCustomer(_customerId);
            _cartAccessor.DeleteCart(_cartId);
            _productAccessor.DeleteProduct(_productId);
            _categoryAccessor.DeleteCategory(_categoryId);
        }

        [TestMethod]
        public void AddOrderItem_ReturnsNewId()
        {
            _insertedId = _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetOrderItem_ReturnsCorrectOrderItem()
        {
            _insertedId = _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            OrderItem result = _accessor.GetOrderItem(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(2, result.Quantity);
        }

        [TestMethod]
        public void GetOrderItem_ReturnsNull_WhenNotFound()
        {
            OrderItem result = _accessor.GetOrderItem(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetOrderItemsByOrder_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            var result = _accessor.GetOrderItemsByOrder(_orderId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(o => o.Id == _insertedId));
        }

        [TestMethod]
        public void UpdateOrderItemQuantity_UpdatesQuantity()
        {
            _insertedId = _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            _accessor.UpdateOrderItemQuantity(_insertedId, 5);
            OrderItem result = _accessor.GetOrderItem(_insertedId);
            Assert.AreEqual(5, result.Quantity);
        }

        [TestMethod]
        public void DeleteOrderItem_RemovesOrderItem()
        {
            int id = _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            _accessor.DeleteOrderItem(id);
            OrderItem result = _accessor.GetOrderItem(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }

        [TestMethod]
        public void DeleteAllOrderItems_RemovesAllItemsInOrder()
        {
            _accessor.AddOrderItem(_orderId, _productId, 1, 9.99m);
            _accessor.AddOrderItem(_orderId, _productId, 2, 9.99m);
            _accessor.DeleteAllOrderItems(_orderId);
            var result = _accessor.GetOrderItemsByOrder(_orderId);
            Assert.AreEqual(0, result.Count);
            _insertedId = 0;
        }
    }
}