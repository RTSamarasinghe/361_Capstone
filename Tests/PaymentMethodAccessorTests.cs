using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.

namespace Tests
{
    [TestClass]
    public class PaymentMethodAccessorTests
    {
        private readonly PaymentMethodAccessor _accessor = new PaymentMethodAccessor();
        private int _insertedId;

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeletePaymentMethod(_insertedId);
            }
        }

        [TestMethod]
        public void AddPaymentMethod_ReturnsNewId()
        {
            _insertedId = _accessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "John Doe", "hashedpin");
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetPaymentMethod_ReturnsCorrectPaymentMethod()
        {
            _insertedId = _accessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "John Doe", "hashedpin");
            PaymentMethod result = _accessor.GetPaymentMethod(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual("John Doe", result.CardholderName);
        }

        [TestMethod]
        public void GetPaymentMethod_ReturnsNull_WhenNotFound()
        {
            PaymentMethod result = _accessor.GetPaymentMethod(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllPaymentMethods_ReturnsList()
        {
            _insertedId = _accessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "John Doe", "hashedpin");
            var result = _accessor.GetAllPaymentMethods();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void UpdatePaymentMethod_UpdatesFields()
        {
            _insertedId = _accessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "John Doe", "hashedpin");
            _accessor.UpdatePaymentMethod(_insertedId, "newhashedcard", DateTime.Now.AddYears(3), "Jane Doe", "newhashedpin");
            PaymentMethod result = _accessor.GetPaymentMethod(_insertedId);
            Assert.AreEqual("Jane Doe", result.CardholderName);
            Assert.AreEqual("newhashedcard", result.CardNumberHash);
        }

        [TestMethod]
        public void DeletePaymentMethod_RemovesPaymentMethod()
        {
            int id = _accessor.AddPaymentMethod("hashedcard", DateTime.Now.AddYears(2), "John Doe", "hashedpin");
            _accessor.DeletePaymentMethod(id);
            PaymentMethod result = _accessor.GetPaymentMethod(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}