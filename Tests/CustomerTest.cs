using System;
using System.Collections.Generic;
using System.Text;
using Managers;
using DataContracts;
using Moq;

namespace CustomerTest;

[TestClass]
public class CustonerManagerTests
{
    private Mock<ICustomerEngine> _customerEngineMock;
    private CustomerManager _customerManager;

    [TestInitialize]
    public void Setup()
    {
        _customerEngineMock = new Mock<ICustomerEngine>();
        _customerManager = new CustomerManager(_customerEngineMock.Object);
    }

    [TestMethod]
    public void AddCustomer_ReturnID()
    {
        //Arrange
        _customerEngineMock.Setup(engine => engine.AddCustomer(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(1);

        //Act
        int result = _customerManager.AddCustomer("John Doe", "test@gmail.com", "hashedpassword", 1, 1);

        //Assert
        Assert.AreEqual(1, result);
        _customerEngineMock.Verify(e => e.AddCustomer("John Doe", "test@gmail.com", "hashedpassword", 1, 1), Times.Once);
    }

    [TestMethod]
    public void CustomerAlreadyExist_Id()
    {
        var expectedCustomer = new Customer { Id = 1, Name = "John Doe" };
        _customerEngineMock.Setup(e => e.GetCustomer(1)).Returns(expectedCustomer);


        //Act
        var result = _customerManager.GetCustomer(1);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerEngineMock.Verify(e => e.GetCustomer(1), Times.Once);
    }

    [TestMethod]
    public void CustomerAlreadyExist_Email()
    {
        var expectedCustomer = new Customer { Id = 1, Name = "John Doe", Email = "test@gmail.com" };
        _customerEngineMock.Setup(e => e.GetCustomerByEmail("test@gmail.com")).Returns(expectedCustomer);

        //Act
        var result = _customerManager.GetCustomerByEmail("test@gmail.com");

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerEngineMock.Verify(e => e.GetCustomerByEmail("test@gmail.com"), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldInvokeEngineUpdate()
    {
        // Act
        _customerManager.UpdateCustomer(1, "New Name", "new@test.com", "newHash");

        // Assert
        _customerEngineMock.Verify(e => e.UpdateCustomer(1, "New Name", "new@test.com", "newHash"), Times.Once);
    }

    [TestMethod]
    public void DeleteCustomer_ShouldInvokeEngineDelete()
    {
        // Act
        _customerManager.DeleteCustomer(5);

        // Assert
        _customerEngineMock.Verify(e => e.DeleteCustomer(5), Times.Once);
    }
}
