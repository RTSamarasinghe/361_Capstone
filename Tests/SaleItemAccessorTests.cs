using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class SaleItemAccessorTests
    {
        private readonly SaleItemAccessor _accessor = new SaleItemAccessor();
        private readonly SaleAccessor _saleAccessor = new SaleAccessor();
        private readonly ProductAccessor _productAccessor = new ProductAccessor();
        private readonly CategoryAccessor _categoryAccessor = new CategoryAccessor();
        private int _insertedId;
        private int _saleId;
        private int _productId;
        private int _categoryId;

        [TestInitialize]
        public void Setup()
        {
            _categoryId = _categoryAccessor.AddCategory("Test Category");
            _productId = _productAccessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            _saleId = _saleAccessor.AddSale(DateTime.Now, null, 10.00m, null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteSaleItem(_insertedId);
            }
            _saleAccessor.DeleteSale(_saleId);
            _productAccessor.DeleteProduct(_productId);
            _categoryAccessor.DeleteCategory(_categoryId);
        }

        [TestMethod]
        public void AddSaleItem_ReturnsNewId()
        {
            _insertedId = _accessor.AddSaleItem(_saleId, _productId);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetSaleItem_ReturnsCorrectSaleItem()
        {
            _insertedId = _accessor.AddSaleItem(_saleId, _productId);
            SaleItem result = _accessor.GetSaleItem(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(_saleId, result.SaleId);
        }

        [TestMethod]
        public void GetSaleItem_ReturnsNull_WhenNotFound()
        {
            SaleItem result = _accessor.GetSaleItem(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSaleItemsBySale_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddSaleItem(_saleId, _productId);
            var result = _accessor.GetSaleItemsBySale(_saleId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(s => s.Id == _insertedId));
        }

        [TestMethod]
        public void GetSaleItemsByProduct_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddSaleItem(_saleId, _productId);
            var result = _accessor.GetSaleItemsByProduct(_productId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(s => s.ProductId == _productId));
        }

        [TestMethod]
        public void DeleteSaleItem_RemovesSaleItem()
        {
            int id = _accessor.AddSaleItem(_saleId, _productId);
            _accessor.DeleteSaleItem(id);
            SaleItem result = _accessor.GetSaleItem(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }

        [TestMethod]
        public void DeleteAllSaleItems_RemovesAllItemsInSale()
        {
            _accessor.AddSaleItem(_saleId, _productId);
            _accessor.DeleteAllSaleItems(_saleId);
            var result = _accessor.GetSaleItemsBySale(_saleId);
            Assert.AreEqual(0, result.Count);
            _insertedId = 0;
        }
    }
}