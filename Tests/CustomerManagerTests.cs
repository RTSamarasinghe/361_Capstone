using System;
using Managers;
using DataContracts;
using Moq;

namespace CustomerTest;

[TestClass]
public class CustomerManagerTests
{
    private Mock<ICustomerEngine> _customerEngineMock = null!;
    private CustomerManager _customerManager = null!;

    [TestInitialize]
    public void Setup()
    {
        _customerEngineMock = new Mock<ICustomerEngine>();
        _customerManager = new CustomerManager(_customerEngineMock.Object);
    }

    [TestMethod]
    public void AddCustomer_ReturnsId()
    {
        _customerEngineMock
            .Setup(engine => engine.AddCustomer(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .Returns(1);

        int result = _customerManager.AddCustomer("John Doe", "test@gmail.com", "hashedpassword", 1, 1);

        Assert.AreEqual(1, result);
        _customerEngineMock.Verify(e => e.AddCustomer("John Doe", "test@gmail.com", "hashedpassword", 1, 1), Times.Once);
    }

    [TestMethod]
    public void GetCustomer_ReturnsCustomer_WhenCustomerExists()
    {
        var expectedCustomer = new Customer { Id = 1, Name = "John Doe" };
        _customerEngineMock.Setup(e => e.GetCustomer(1)).Returns(expectedCustomer);

        var result = _customerManager.GetCustomer(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerEngineMock.Verify(e => e.GetCustomer(1), Times.Once);
    }

    [TestMethod]
    public void GetCustomerByEmail_ReturnsCustomer_WhenCustomerExists()
    {
        var expectedCustomer = new Customer { Id = 1, Name = "John Doe", Email = "test@gmail.com" };
        _customerEngineMock.Setup(e => e.GetCustomerByEmail("test@gmail.com")).Returns(expectedCustomer);

        var result = _customerManager.GetCustomerByEmail("test@gmail.com");

        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerEngineMock.Verify(e => e.GetCustomerByEmail("test@gmail.com"), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldInvokeEngineUpdate()
    {
        _customerManager.UpdateCustomer(1, "New Name", "new@test.com", "newHash");

        _customerEngineMock.Verify(e => e.UpdateCustomer(1, "New Name", "new@test.com", "newHash"), Times.Once);
    }

    [TestMethod]
    public void DeleteCustomer_ShouldInvokeEngineDelete()
    {
        _customerManager.DeleteCustomer(5);

        _customerEngineMock.Verify(e => e.DeleteCustomer(5), Times.Once);
    }

    [TestMethod]
    public void GetCustomer_ShouldPropagateException()
    {
        _customerEngineMock
            .Setup(e => e.GetCustomer(1))
            .Throws(new Exception("Customer not found"));

        try
        {
            _customerManager.GetCustomer(1);
            Assert.Fail("Expected Exception was not thrown.");
        }
        catch (Exception)
        {
        }
    }
}