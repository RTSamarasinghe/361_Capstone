using DataContracts;
using Moq;

namespace CartTest;

[TestClass]
public class CartManagerTests
{
    private Mock<ICartEngine> _cartEngineMock = null!;
    private CartManager _cartManager = null!;

    [TestInitialize]
    public void Setup()
    {
        _cartEngineMock = new Mock<ICartEngine>();
        _cartManager = new CartManager(_cartEngineMock.Object);
    }

    [TestMethod]
    public void AddCart_ReturnsId()
    {
        _cartEngineMock.Setup(e => e.AddCart()).Returns(1);

        int result = _cartManager.AddCart();

        Assert.AreEqual(1, result);
        _cartEngineMock.Verify(e => e.AddCart(), Times.Once);
    }

    [TestMethod]
    public void GetCart_ReturnsCart_WhenCartExists()
    {
        var expectedCart = new Cart { Id = 1 };
        _cartEngineMock.Setup(e => e.GetCart(1)).Returns(expectedCart);

        var result = _cartManager.GetCart(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        _cartEngineMock.Verify(e => e.GetCart(1), Times.Once);
    }

    [TestMethod]
    public void GetCartItems_ReturnsCartItems()
    {
        var expectedItems = new List<CartItem>
        {
            new CartItem { Id = 1, CartId = 1, ProductId = 10, Quantity = 2 },
            new CartItem { Id = 2, CartId = 1, ProductId = 11, Quantity = 1 }
        };

        _cartEngineMock.Setup(e => e.GetCartItems(1)).Returns(expectedItems);

        var result = _cartManager.GetCartItems(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        _cartEngineMock.Verify(e => e.GetCartItems(1), Times.Once);
    }

    [TestMethod]
    public void AddCartItem_ReturnsId()
    {
        _cartEngineMock.Setup(e => e.AddCartItem(1, 10, 2)).Returns(5);

        int result = _cartManager.AddCartItem(1, 10, 2);

        Assert.AreEqual(5, result);
        _cartEngineMock.Verify(e => e.AddCartItem(1, 10, 2), Times.Once);
    }

    [TestMethod]
    public void UpdateCartItemQuantity_ShouldInvokeEngineUpdate()
    {
        _cartManager.UpdateCartItemQuantity(3, 4);

        _cartEngineMock.Verify(e => e.UpdateCartItemQuantity(3, 4), Times.Once);
    }

    [TestMethod]
    public void RemoveCartItem_ShouldInvokeEngineRemove()
    {
        _cartManager.RemoveCartItem(3);

        _cartEngineMock.Verify(e => e.RemoveCartItem(3), Times.Once);
    }

    [TestMethod]
    public void ClearCart_ShouldInvokeEngineClear()
    {
        _cartManager.ClearCart(1);

        _cartEngineMock.Verify(e => e.ClearCart(1), Times.Once);
    }

    [TestMethod]
    public void DeleteCart_ShouldInvokeEngineDelete()
    {
        _cartManager.DeleteCart(1);

        _cartEngineMock.Verify(e => e.DeleteCart(1), Times.Once);
    }
}