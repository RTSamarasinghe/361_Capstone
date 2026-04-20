using DataContracts;
using Moq;

namespace CustomerTest;

[TestClass]
public class OrderManagerTests
{
    private Mock<IOrderEngine> _orderEngineMock = null!;
    private OrderManager _orderManager = null!;

    [TestInitialize]
    public void Setup()
    {
        _orderEngineMock = new Mock<IOrderEngine>();
        _orderManager = new OrderManager(_orderEngineMock.Object);
    }

    [TestMethod]
    public void AddOrder_ReturnsId()
    {
        _orderEngineMock
            .Setup(e => e.AddOrder(1, 100.0m, "Pending", 10, 20))
            .Returns(5);

        int result = _orderManager.AddOrder(1, 100.0m, "Pending", 10, 20);

        Assert.AreEqual(5, result);
        _orderEngineMock.Verify(e => e.AddOrder(1, 100.0m, "Pending", 10, 20), Times.Once);
    }

    [TestMethod]
    public void GetOrder_ReturnsOrder()
    {
        var order = new Order { Id = 1, OrderStatus = "Pending" };

        _orderEngineMock.Setup(e => e.GetOrder(1)).Returns(order);

        var result = _orderManager.GetOrder(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        _orderEngineMock.Verify(e => e.GetOrder(1), Times.Once);
    }

    [TestMethod]
    public void GetOrdersByCustomer_ReturnsOrders()
    {
        var orders = new List<Order>
        {
            new Order { Id = 1 },
            new Order { Id = 2 }
        };

        _orderEngineMock.Setup(e => e.GetOrdersByCustomer(1)).Returns(orders);

        var result = _orderManager.GetOrdersByCustomer(1);

        Assert.AreEqual(2, result.Count);
        _orderEngineMock.Verify(e => e.GetOrdersByCustomer(1), Times.Once);
    }

    [TestMethod]
    public void GetOrdersByStatus_ReturnsOrders()
    {
        var orders = new List<Order>
        {
            new Order { Id = 1, OrderStatus = "Pending" }
        };

        _orderEngineMock.Setup(e => e.GetOrdersByStatus("Pending")).Returns(orders);

        var result = _orderManager.GetOrdersByStatus("Pending");

        Assert.AreEqual(1, result.Count);
        _orderEngineMock.Verify(e => e.GetOrdersByStatus("Pending"), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ShouldInvokeEngine()
    {
        _orderManager.UpdateOrderStatus(1, "Shipped");

        _orderEngineMock.Verify(e => e.UpdateOrderStatus(1, "Shipped"), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderTotalAmount_ShouldInvokeEngine()
    {
        _orderManager.UpdateOrderTotalAmount(1, 250.0m);

        _orderEngineMock.Verify(e => e.UpdateOrderTotalAmount(1, 250.0m), Times.Once);
    }

    [TestMethod]
    public void DeleteOrder_ShouldInvokeEngine()
    {
        _orderManager.DeleteOrder(1);

        _orderEngineMock.Verify(e => e.DeleteOrder(1), Times.Once);
    }

    [TestMethod]
    public void GetOrder_ShouldPropagateException()
    {
        _orderEngineMock
            .Setup(e => e.GetOrder(1))
            .Throws(new Exception("Not found"));

        try
        {
            _orderManager.GetOrder(1);
            Assert.Fail("Expected Exception was not thrown.");
        }
        catch (Exception)
        {
        }
    }
}