using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accessors;
using DataContracts;
//For puropse of testing, I'm doing integration tests here since the accessor is tightly coupled to the database. In a real world scenario, I would abstract the database access and mock it for unit testing.
namespace Tests
{
    [TestClass]
    public class CategoryAccessorTests
    {
        private readonly CategoryAccessor _accessor = new CategoryAccessor();
        private int _insertedId;

        [TestCleanup]
        public void Cleanup()
        {
            if (_insertedId > 0)
            {
                _accessor.DeleteCategory(_insertedId);
            }
        }

        [TestMethod]
        public void AddCategory_ReturnsNewId()
        {
            _insertedId = _accessor.AddCategory("Test Category");
            Assert.IsTrue(_insertedId > 0);
        }

        [TestMethod]
        public void GetCategory_ReturnsCorrectCategory()
        {
            _insertedId = _accessor.AddCategory("Test Category");
            Category result = _accessor.GetCategory(_insertedId);
            Assert.IsNotNull(result);
            Assert.AreEqual(_insertedId, result.Id);
            Assert.AreEqual("Test Category", result.Name);
        }

        [TestMethod]
        public void GetCategory_ReturnsNull_WhenNotFound()
        {
            Category result = _accessor.GetCategory(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllCategories_ReturnsList()
        {
            _insertedId = _accessor.AddCategory("Test Category");
            var result = _accessor.GetAllCategories();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void UpdateCategory_UpdatesName()
        {
            _insertedId = _accessor.AddCategory("Old Name");
            _accessor.UpdateCategory(_insertedId, "New Name");
            Category result = _accessor.GetCategory(_insertedId);
            Assert.AreEqual("New Name", result.Name);
        }

        [TestMethod]
        public void DeleteCategory_RemovesCategory()
        {
            int id = _accessor.AddCategory("To Delete");
            _accessor.DeleteCategory(id);
            Category result = _accessor.GetCategory(id);
            Assert.IsNull(result);
            _insertedId = 0;
        }
    }
}