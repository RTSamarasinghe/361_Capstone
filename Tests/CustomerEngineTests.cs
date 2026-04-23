using System;
using DataContracts;
using Engines;
using Accessors;
using Moq;

namespace CustomerTest;

[TestClass]
public class CustomerEngineTests
{
    private Mock<ICustomerAccessor> _customerAccessorMock = null!;
    private CustomerEngine _customerEngine = null!;

    [TestInitialize]
    public void Setup()
    {
        _customerAccessorMock = new Mock<ICustomerAccessor>();
        _customerEngine = new CustomerEngine(_customerAccessorMock.Object);
    }

    [TestMethod]
    public void AddCustomer_ValidInput_ReturnsId()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomerByEmail("test@gmail.com"))
            .Returns((Customer)null!);

        _customerAccessorMock
            .Setup(a => a.AddCustomer("John Doe", "test@gmail.com", "hashedpassword"))
            .Returns(1);

        int result = _customerEngine.AddCustomer(" John Doe ", " test@gmail.com ", "hashedpassword");

        Assert.AreEqual(1, result);
        _customerAccessorMock.Verify(a => a.GetCustomerByEmail("test@gmail.com"), Times.Once);
        _customerAccessorMock.Verify(a => a.AddCustomer("John Doe", "test@gmail.com", "hashedpassword"), Times.Once);
    }

    [TestMethod]
    public void AddCustomer_DuplicateEmail_ThrowsException()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomerByEmail("test@gmail.com"))
            .Returns(new Customer { Id = 1, Email = "test@gmail.com" });

        try
        {
            _customerEngine.AddCustomer("John Doe", "test@gmail.com", "hashedpassword");
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void GetCustomer_ExistingId_ReturnsCustomer()
    {
        var customer = new Customer { Id = 1, Name = "John Doe" };
        _customerAccessorMock.Setup(a => a.GetCustomer(1)).Returns(customer);

        var result = _customerEngine.GetCustomer(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerAccessorMock.Verify(a => a.GetCustomer(1), Times.Once);
    }

    [TestMethod]
    public void GetCustomer_MissingCustomer_ThrowsException()
    {
        _customerAccessorMock.Setup(a => a.GetCustomer(1)).Returns((Customer)null!);

        try
        {
            _customerEngine.GetCustomer(1);
            Assert.Fail("Expected Exception was not thrown.");
        }
        catch (Exception)
        {
        }
    }

    [TestMethod]
    public void GetCustomerByEmail_ExistingEmail_ReturnsCustomer()
    {
        var customer = new Customer { Id = 1, Name = "John Doe", Email = "test@gmail.com" };
        _customerAccessorMock.Setup(a => a.GetCustomerByEmail("test@gmail.com")).Returns(customer);

        var result = _customerEngine.GetCustomerByEmail(" test@gmail.com ");

        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.Name);
        _customerAccessorMock.Verify(a => a.GetCustomerByEmail("test@gmail.com"), Times.Once);
    }

    [TestMethod]
    public void GetCustomerByEmail_BlankEmail_ThrowsException()
    {
        try
        {
            _customerEngine.GetCustomerByEmail("   ");
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void GetAllCustomers_ReturnsList()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe" },
            new Customer { Id = 2, Name = "Jane Doe" }
        };

        _customerAccessorMock.Setup(a => a.GetAllCustomers()).Returns(customers);

        var result = _customerEngine.GetAllCustomers();

        Assert.AreEqual(2, result.Count);
        _customerAccessorMock.Verify(a => a.GetAllCustomers(), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomer_ValidInput_CallsAccessor()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomer(1))
            .Returns(new Customer { Id = 1, Name = "John Doe", Email = "old@gmail.com" });

        _customerAccessorMock
            .Setup(a => a.GetCustomerByEmail("new@gmail.com"))
            .Returns((Customer)null!);

        _customerEngine.UpdateCustomer(1, " New Name ", " new@gmail.com ", "newhash");

        _customerAccessorMock.Verify(a => a.UpdateCustomer(1, "New Name", "new@gmail.com", "newhash"), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomer_DuplicateEmail_ThrowsException()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomer(1))
            .Returns(new Customer { Id = 1, Name = "John Doe", Email = "old@gmail.com" });

        _customerAccessorMock
            .Setup(a => a.GetCustomerByEmail("taken@gmail.com"))
            .Returns(new Customer { Id = 2, Email = "taken@gmail.com" });

        try
        {
            _customerEngine.UpdateCustomer(1, "John Doe", "taken@gmail.com", "hash");
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void UpdateCustomerCart_ValidInput_CallsAccessor()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomer(1))
            .Returns(new Customer { Id = 1 });

        _customerEngine.UpdateCustomerCart(1, 2);

        _customerAccessorMock.Verify(a => a.UpdateCustomerCart(1, 2), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomerCart_InvalidCartId_ThrowsException()
    {
        try
        {
            _customerEngine.UpdateCustomerCart(1, 0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void UpdateCustomerPaymentMethod_ValidInput_CallsAccessor()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomer(1))
            .Returns(new Customer { Id = 1 });

        _customerEngine.UpdateCustomerPaymentMethod(1, 2);

        _customerAccessorMock.Verify(a => a.UpdateCustomerPaymentMethod(1, 2), Times.Once);
    }

    [TestMethod]
    public void UpdateCustomerPaymentMethod_InvalidPaymentMethodId_ThrowsException()
    {
        try
        {
            _customerEngine.UpdateCustomerPaymentMethod(1, 0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void DeleteCustomer_ExistingCustomer_CallsAccessor()
    {
        _customerAccessorMock
            .Setup(a => a.GetCustomer(1))
            .Returns(new Customer { Id = 1 });

        _customerEngine.DeleteCustomer(1);

        _customerAccessorMock.Verify(a => a.DeleteCustomer(1), Times.Once);
    }

    [TestMethod]
    public void DeleteCustomer_InvalidId_ThrowsException()
    {
        try
        {
            _customerEngine.DeleteCustomer(0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }
}