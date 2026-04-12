
using Managers;
using DataContracts;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ManagerTests
{
    [TestClass]
    public sealed class CustomerManagerTests
    {


        private Mock<ICustomerAccessor> _mockAccessor;
        private CustomerManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _mockAccessor = new Mock<ICustomerAccessor>();
            _manager = new CustomerManager(_mockAccessor.Object);
        }
        [TestMethod]
        public void AddCustomer()
        {
            _mockAccessor
                .Setup(a => a.GetCustomerByEmail("test@email.com"))
                .Returns((Customer)null);

            _mockAccessor
                .Setup(a => a.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 1, 1))
                .Returns(1);

            int result = _manager.AddCustomer("John", "test@email.com", "hash", 1, 1);

            Assert.AreEqual(1, result);

        }

    [TestMethod]
        public void AddCustomer_Throws_WhenEmailAlreadyExists()
    {
        _mockAccessor
            .Setup(a => a.GetCustomerByEmail("exists@email.com"))
            .Returns(new Customer { Id = 2, Email = "exists@email.com" });

            Assert.Throws<ArgumentException>(() => _manager.AddCustomer("Jane", "exists@email.com", "hash", 1, 1));
        }

    [TestMethod]
    public void AddCustomer_TrimsInputs_And_CallsAccessor()
    {
        _mockAccessor
            .Setup(a => a.GetCustomerByEmail(It.IsAny<string>()))
            .Returns((Customer)null);

        _mockAccessor
            .Setup(a => a.AddCustomer("John", "test@email.com", "hash", 1, 1))
            .Returns(5)
            .Verifiable();

        int result = _manager.AddCustomer(" John ", " test@email.com ", "hash", 1, 1);

        Assert.AreEqual(5, result);
        _mockAccessor.Verify(a => a.AddCustomer("John", "test@email.com", "hash", 1, 1), Times.Once);
    }

    [TestMethod]
    public void GetCustomer_Throws_OnInvalidId()
    {
            Assert.Throws<ArgumentException>(() => _manager.GetCustomer(0));
        }

    [TestMethod]
    public void GetCustomer_Throws_WhenNotFound()
    {
        _mockAccessor.Setup(a => a.GetCustomer(1)).Returns((Customer)null);
        Assert.Throws<Exception>(() => _manager.GetCustomer(1));
    }

    [TestMethod]
    public void GetCustomer_Returns_Customer_WhenFound()
    {
        var c = new Customer { Id = 1, Name = "Bob", Email = "bob@test.com" };
        _mockAccessor.Setup(a => a.GetCustomer(1)).Returns(c);

        var result = _manager.GetCustomer(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Bob", result.Name);
    }

    [TestMethod]
    public void UpdateCustomer_Throws_WhenEmailTakenByAnother()
    {
        var existing = new Customer { Id = 1, Name = "A", Email = "a@x.com" };
        var other = new Customer { Id = 2, Name = "B", Email = "b@x.com" };

        _mockAccessor.Setup(a => a.GetCustomer(1)).Returns(existing);
        _mockAccessor.Setup(a => a.GetCustomerByEmail("b@x.com")).Returns(other);

        Assert.Throws<ArgumentException>(() => _manager.UpdateCustomer(1, "A", "b@x.com", "hash"));
    }

    [TestMethod]
    public void UpdateCustomer_CallsAccessor_WhenValid()
    {
        var existing = new Customer { Id = 1, Name = "A", Email = "a@x.com" };

        _mockAccessor.Setup(a => a.GetCustomer(1)).Returns(existing);
        _mockAccessor.Setup(a => a.GetCustomerByEmail("new@x.com")).Returns((Customer)null);
        _mockAccessor.Setup(a => a.UpdateCustomer(1, "NewName", "new@x.com", "hash")).Verifiable();

        _manager.UpdateCustomer(1, " NewName ", " new@x.com ", "hash");

        _mockAccessor.Verify(a => a.UpdateCustomer(1, "NewName", "new@x.com", "hash"), Times.Once);
    }

    [TestMethod]
    public void DeleteCustomer_Throws_WhenNotFound()
    {
        _mockAccessor.Setup(a => a.GetCustomer(10)).Returns((Customer)null);
        Assert.Throws<Exception>(() => _manager.DeleteCustomer(10));
    }

    [TestMethod]
    public void DeleteCustomer_CallsAccessor_WhenFound()
    {
        var existing = new Customer { Id = 3, Name = "Del", Email = "del@x.com" };
        _mockAccessor.Setup(a => a.GetCustomer(3)).Returns(existing);
        _mockAccessor.Setup(a => a.DeleteCustomer(3)).Verifiable();

        _manager.DeleteCustomer(3);

        _mockAccessor.Verify(a => a.DeleteCustomer(3), Times.Once);
    }

    }
}

