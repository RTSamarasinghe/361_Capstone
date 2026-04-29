using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class SaleCategoryAccessorTests
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly SaleCategoryAccessor _accessor = new SaleCategoryAccessor(ConnectionString);
        private readonly SaleAccessor _saleAccessor = new SaleAccessor(ConnectionString);
        private readonly CategoryAccessor _categoryAccessor = new CategoryAccessor(ConnectionString);
        private int _insertedId;
        private int _saleId;
        private int _categoryId;

        [TestInitialize]
        public void Setup()
        {
            _categoryId = _categoryAccessor.AddCategory("Test Category");
            _saleId = _saleAccessor.AddSale(DateTime.Now, null, 10.00m, null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteSaleCategory(_insertedId);
            }
            _saleAccessor.DeleteSale(_saleId);
            _categoryAccessor.DeleteCategory(_categoryId);
        }

        [TestMethod]
        public void AddSaleCategory_ReturnsNewId()
        {
            _insertedId = _accessor.AddSaleCategory(_saleId, _categoryId);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetSaleCategory_ReturnsCorrectSaleCategory()
        {
            _insertedId = _accessor.AddSaleCategory(_saleId, _categoryId);
            SaleCategory result = _accessor.GetSaleCategory(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(_saleId, result.SaleId);
        }

        [TestMethod]
        public void GetSaleCategory_ReturnsNull_WhenNotFound()
        {
            SaleCategory result = _accessor.GetSaleCategory(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSaleCategoriesBySale_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddSaleCategory(_saleId, _categoryId);
            var result = _accessor.GetSaleCategoriesBySale(_saleId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(s => s.Id == _insertedId));
        }

        [TestMethod]
        public void GetSaleCategoriesByCategory_ReturnsCorrectItems()
        {
            _insertedId = _accessor.AddSaleCategory(_saleId, _categoryId);
            var result = _accessor.GetSaleCategoriesByCategory(_categoryId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(s => s.CategoryId == _categoryId));
        }

        [TestMethod]
        public void DeleteSaleCategory_RemovesSaleCategory()
        {
            int id = _accessor.AddSaleCategory(_saleId, _categoryId);
            _accessor.DeleteSaleCategory(id);
            SaleCategory result = _accessor.GetSaleCategory(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }

        [TestMethod]
        public void DeleteAllSaleCategories_RemovesAllCategoriesInSale()
        {
            _accessor.AddSaleCategory(_saleId, _categoryId);
            _accessor.DeleteAllSaleCategories(_saleId);
            var result = _accessor.GetSaleCategoriesBySale(_saleId);
            Assert.AreEqual(0, result.Count);
            _insertedId = 0;
        }
    }
}
