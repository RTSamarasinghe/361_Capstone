using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class CartItemAccessorTests
    {
        private readonly CartItemAccessor _accessor = new CartItemAccessor();
        private readonly CartAccessor _cartAccessor = new CartAccessor();
        private readonly ProductAccessor _productAccessor = new ProductAccessor();
        private readonly CategoryAccessor _categoryAccessor = new CategoryAccessor();
        private int _insertedId;
        private int _cartId;
        private int _productId;
        private int _categoryId;

        [TestInitialize]
        public void Setup()
        {
            _categoryId = _categoryAccessor.AddCategory("Test Category");
            _cartId = _cartAccessor.AddCart();
            _productId = _productAccessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteCartItem(_insertedId);
            }
            _productAccessor.DeleteProduct(_productId);
            _cartAccessor.DeleteCart(_cartId);
            _categoryAccessor.DeleteCategory(_categoryId);
        }

        [TestMethod]
        public void AddCartItem_ReturnsNewId()
        {
            _insertedId = _accessor.AddCartItem(_cartId, _productId, 2);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetCartItem_ReturnsCorrectCartItem()
        {
            _insertedId = _accessor.AddCartItem(_cartId, _productId, 2);
            CartItem result = _accessor.GetCartItem(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(2, result.Quantity);
        }

        [TestMethod]
        public void GetCartItem_ReturnsNull_WhenNotFound()
        {
            CartItem result = _accessor.GetCartItem(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCartItemsByCart_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddCartItem(_cartId, _productId, 2);
            var result = _accessor.GetCartItemsByCart(_cartId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(c => c.CartId == _cartId));
        }

        [TestMethod]
        public void UpdateCartItemQuantity_UpdatesQuantity()
        {
            _insertedId = _accessor.AddCartItem(_cartId, _productId, 2);
            _accessor.UpdateCartItemQuantity(_insertedId, 5);
            CartItem result = _accessor.GetCartItem(_insertedId);
            Assert.AreEqual(5, result.Quantity);
        }

        [TestMethod]
        public void DeleteCartItem_RemovesCartItem()
        {
            int id = _accessor.AddCartItem(_cartId, _productId, 2);
            _accessor.DeleteCartItem(id);
            CartItem result = _accessor.GetCartItem(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }

        [TestMethod]
        public void DeleteAllCartItems_RemovesAllItemsInCart()
        {
            _accessor.AddCartItem(_cartId, _productId, 1);
            _accessor.AddCartItem(_cartId, _productId, 2);
            _accessor.DeleteAllCartItems(_cartId);
            var result = _accessor.GetCartItemsByCart(_cartId);
            Assert.AreEqual(0, result.Count);
            _insertedId = 0;
        }
    }
}