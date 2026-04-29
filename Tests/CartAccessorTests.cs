using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class CartAccessorTests
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly CartAccessor _accessor = new CartAccessor(ConnectionString);
        private int _insertedId;

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteCart(_insertedId);
            }
        }

        [TestMethod]
        public void AddCart_ReturnsNewId()
        {
            _insertedId = _accessor.AddCart();
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetCart_ReturnsCorrectCart()
        {
            _insertedId = _accessor.AddCart();
            Cart result = _accessor.GetCart(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
        }

        [TestMethod]
        public void GetCart_ReturnsNull_WhenNotFound()
        {
            Cart result = _accessor.GetCart(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteCart_RemovesCart()
        {
            int id = _accessor.AddCart();
            _accessor.DeleteCart(id);
            Cart result = _accessor.GetCart(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}
