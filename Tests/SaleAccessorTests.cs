using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class SaleAccessorTests
    {
        private readonly SaleAccessor _accessor = new SaleAccessor();
        private int _insertedId;

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteSale(_insertedId);
            }
        }

        [TestMethod]
        public void AddSale_ReturnsNewId()
        {
            _insertedId = _accessor.AddSale(DateTime.Now, null, 10.00m, null);
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetSale_ReturnsCorrectSale()
        {
            _insertedId = _accessor.AddSale(DateTime.Now, null, 10.00m, null);
            Sale result = _accessor.GetSale(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual(10.00m, result.DiscountAmount);
        }

        [TestMethod]
        public void GetSale_ReturnsNull_WhenNotFound()
        {
            Sale result = _accessor.GetSale(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllSales_ReturnsList()
        {
            _insertedId = _accessor.AddSale(DateTime.Now, null, 10.00m, null);
            var result = _accessor.GetAllSales();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetActiveSales_ReturnsOnlyActiveSales()
        {
            _insertedId = _accessor.AddSale(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 10.00m, null);
            var result = _accessor.GetActiveSales();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(s => s.Id == _insertedId));
        }

        [TestMethod]
        public void GetActiveSales_DoesNotReturnExpiredSales()
        {
            int expiredId = _accessor.AddSale(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-1), 10.00m, null);
            var result = _accessor.GetActiveSales();
            Assert.IsFalse(result.Any(s => s.Id == expiredId));
            _accessor.DeleteSale(expiredId);
        }

        [TestMethod]
        public void UpdateSale_UpdatesFields()
        {
            _insertedId = _accessor.AddSale(DateTime.Now, null, 10.00m, null);
            _accessor.UpdateSale(_insertedId, DateTime.Now, null, 20.00m, null);
            Sale result = _accessor.GetSale(_insertedId);
            Assert.AreEqual(20.00m, result.DiscountAmount);
        }

        [TestMethod]
        public void DeleteSale_RemovesSale()
        {
            int id = _accessor.AddSale(DateTime.Now, null, 10.00m, null);
            _accessor.DeleteSale(id);
            Sale result = _accessor.GetSale(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}