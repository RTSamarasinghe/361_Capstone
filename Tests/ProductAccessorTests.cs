using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class ProductAccessorTests
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly ProductAccessor _accessor = new ProductAccessor(ConnectionString);
        private readonly CategoryAccessor _categoryAccessor = new CategoryAccessor(ConnectionString);
        private int _insertedId;
        private int _categoryId;

        [TestInitialize]
        public void Setup()
        {
            _categoryId = _categoryAccessor.AddCategory("Test Category");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteProduct(_insertedId);
            }
            _categoryAccessor.DeleteCategory(_categoryId);
        }

        [TestMethod]
        public void AddProduct_ReturnsNewId()
        {
            _insertedId = _accessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetProduct_ReturnsCorrectProduct()
        {
            _insertedId = _accessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            Product result = _accessor.GetProduct(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual("Test Product", result.Name);
        }

        [TestMethod]
        public void GetProduct_ReturnsNull_WhenNotFound()
        {
            Product result = _accessor.GetProduct(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllProducts_ReturnsList()
        {
            _insertedId = _accessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            var result = _accessor.GetAllProducts();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetProductsByCategory_ReturnsCorrectProducts()
        {
            _insertedId = _accessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            var result = _accessor.GetProductsByCategory(_categoryId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(p => p.CategoryId == _categoryId));
        }

        [TestMethod]
        public void UpdateProduct_UpdatesFields()
        {
            _insertedId = _accessor.AddProduct("Old Name", "Old Desc", 9.99m, _categoryId, null, null, null, null, 10);
            _accessor.UpdateProduct(_insertedId, "New Name", "New Desc", 19.99m, _categoryId, null, null, null, null, 20);
            Product result = _accessor.GetProduct(_insertedId);
            Assert.AreEqual("New Name", result.Name);
            Assert.AreEqual(19.99m, result.Price);
        }

        [TestMethod]
        public void UpdateStockQuantity_UpdatesStock()
        {
            _insertedId = _accessor.AddProduct("Test Product", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            _accessor.UpdateStockQuantity(_insertedId, 50);
            Product result = _accessor.GetProduct(_insertedId);
            Assert.AreEqual(50, result.StockQuantity);
        }

        [TestMethod]
        public void DeleteProduct_RemovesProduct()
        {
            int id = _accessor.AddProduct("To Delete", "Description", 9.99m, _categoryId, null, null, null, null, 10);
            _accessor.DeleteProduct(id);
            Product result = _accessor.GetProduct(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}
