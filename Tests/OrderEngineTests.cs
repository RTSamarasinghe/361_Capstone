using DataContracts;
using Moq;

namespace CustomerTest;

[TestClass]
public class OrderEngineTests
{
    private Mock<IOrderAccessor> _orderAccessorMock = null!;
    private OrderEngine _orderEngine = null!;

    [TestInitialize]
    public void Setup()
    {
        _orderAccessorMock = new Mock<IOrderAccessor>();
        _orderEngine = new OrderEngine(_orderAccessorMock.Object);
    }

    [TestMethod]
    public void AddOrder_ValidInput_ReturnsId()
    {
        _orderAccessorMock
            .Setup(a => a.AddOrder(1, 100.0m, "Pending", 10, 20))
            .Returns(5);

        int result = _orderEngine.AddOrder(1, 100.0m, "Pending", 10, 20);

        Assert.AreEqual(5, result);
        _orderAccessorMock.Verify(a => a.AddOrder(1, 100.0m, "Pending", 10, 20), Times.Once);
    }

    [TestMethod]
    public void AddOrder_InvalidCustomerId_ThrowsException()
    {
        try
        {
            _orderEngine.AddOrder(0, 100.0m, "Pending", 10, 20);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void GetOrder_ExistingOrder_ReturnsOrder()
    {
        var order = new Order { Id = 1, OrderStatus = "Pending" };
        _orderAccessorMock.Setup(a => a.GetOrder(1)).Returns(order);

        var result = _orderEngine.GetOrder(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        _orderAccessorMock.Verify(a => a.GetOrder(1), Times.Once);
    }

    [TestMethod]
    public void GetOrder_NotFound_ThrowsException()
    {
        _orderAccessorMock.Setup(a => a.GetOrder(1)).Returns((Order)null!);

        try
        {
            _orderEngine.GetOrder(1);
            Assert.Fail("Expected Exception was not thrown.");
        }
        catch (Exception)
        {
        }
    }

    [TestMethod]
    public void GetOrdersByCustomer_ReturnsOrders()
    {
        var orders = new List<Order> { new Order { Id = 1 } };

        _orderAccessorMock.Setup(a => a.GetOrdersByCustomer(1)).Returns(orders);

        var result = _orderEngine.GetOrdersByCustomer(1);

        Assert.AreEqual(1, result.Count);
        _orderAccessorMock.Verify(a => a.GetOrdersByCustomer(1), Times.Once);
    }

    [TestMethod]
    public void GetOrdersByStatus_ReturnsOrders()
    {
        var orders = new List<Order> { new Order { Id = 1, OrderStatus = "Pending" } };

        _orderAccessorMock.Setup(a => a.GetOrdersByStatus("Pending")).Returns(orders);

        var result = _orderEngine.GetOrdersByStatus("Pending");

        Assert.AreEqual(1, result.Count);
        _orderAccessorMock.Verify(a => a.GetOrdersByStatus("Pending"), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ValidInput_CallsAccessor()
    {
        _orderAccessorMock
            .Setup(a => a.GetOrder(1))
            .Returns(new Order { Id = 1 });

        _orderEngine.UpdateOrderStatus(1, "Shipped");

        _orderAccessorMock.Verify(a => a.UpdateOrderStatus(1, "Shipped"), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_InvalidId_ThrowsException()
    {
        try
        {
            _orderEngine.UpdateOrderStatus(0, "Shipped");
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void UpdateOrderTotalAmount_ValidInput_CallsAccessor()
    {
        _orderAccessorMock
            .Setup(a => a.GetOrder(1))
            .Returns(new Order { Id = 1 });

        _orderEngine.UpdateOrderTotalAmount(1, 200.0m);

        _orderAccessorMock.Verify(a => a.UpdateOrderTotalAmount(1, 200.0m), Times.Once);
    }

    [TestMethod]
    public void DeleteOrder_ExistingOrder_CallsAccessor()
    {
        _orderAccessorMock
            .Setup(a => a.GetOrder(1))
            .Returns(new Order { Id = 1 });

        _orderEngine.DeleteOrder(1);

        _orderAccessorMock.Verify(a => a.DeleteOrder(1), Times.Once);
    }

    [TestMethod]
    public void DeleteOrder_InvalidId_ThrowsException()
    {
        try
        {
            _orderEngine.DeleteOrder(0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }
}