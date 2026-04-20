using DataContracts;
using Moq;

namespace CartTest;

[TestClass]
public class CartEngineTests
{
    private Mock<ICartAccessor> _cartAccessorMock = null!;
    private Mock<ICartItemAccessor> _cartItemAccessorMock = null!;
    private CartEngine _cartEngine = null!;

    [TestInitialize]
    public void Setup()
    {
        _cartAccessorMock = new Mock<ICartAccessor>();
        _cartItemAccessorMock = new Mock<ICartItemAccessor>();
        _cartEngine = new CartEngine(_cartAccessorMock.Object, _cartItemAccessorMock.Object);
    }

    [TestMethod]
    public void AddCart_ReturnsId()
    {
        _cartAccessorMock.Setup(a => a.AddCart()).Returns(1);

        int result = _cartEngine.AddCart();

        Assert.AreEqual(1, result);
        _cartAccessorMock.Verify(a => a.AddCart(), Times.Once);
    }

    [TestMethod]
    public void GetCart_ReturnsCart_WhenCartExists()
    {
        var cart = new Cart { Id = 1 };
        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(cart);

        var result = _cartEngine.GetCart(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        _cartAccessorMock.Verify(a => a.GetCart(1), Times.Once);
    }

    [TestMethod]
    public void GetCart_InvalidId_ThrowsException()
    {
        try
        {
            _cartEngine.GetCart(0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void GetCartItems_ReturnsItems_WhenCartExists()
    {
        var items = new List<CartItem>
        {
            new CartItem { Id = 1, CartId = 1, ProductId = 10, Quantity = 2 }
        };

        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(new Cart { Id = 1 });
        _cartItemAccessorMock.Setup(a => a.GetCartItemsByCart(1)).Returns(items);

        var result = _cartEngine.GetCartItems(1);

        Assert.AreEqual(1, result.Count);
        _cartItemAccessorMock.Verify(a => a.GetCartItemsByCart(1), Times.Once);
    }

    [TestMethod]
    public void AddCartItem_ReturnsNewId_WhenProductNotAlreadyInCart()
    {
        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(new Cart { Id = 1 });
        _cartItemAccessorMock.Setup(a => a.GetCartItemsByCart(1)).Returns(new List<CartItem>());
        _cartItemAccessorMock.Setup(a => a.AddCartItem(1, 10, 2)).Returns(5);

        int result = _cartEngine.AddCartItem(1, 10, 2);

        Assert.AreEqual(5, result);
        _cartItemAccessorMock.Verify(a => a.AddCartItem(1, 10, 2), Times.Once);
    }

    [TestMethod]
    public void AddCartItem_ExistingProduct_UpdatesQuantityInsteadOfAdding()
    {
        var existingItems = new List<CartItem>
        {
            new CartItem { Id = 3, CartId = 1, ProductId = 10, Quantity = 2 }
        };

        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(new Cart { Id = 1 });
        _cartItemAccessorMock.Setup(a => a.GetCartItemsByCart(1)).Returns(existingItems);

        int result = _cartEngine.AddCartItem(1, 10, 3);

        Assert.AreEqual(3, result);
        _cartItemAccessorMock.Verify(a => a.UpdateCartItemQuantity(3, 5), Times.Once);
        _cartItemAccessorMock.Verify(a => a.AddCartItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void UpdateCartItemQuantity_ValidInput_UpdatesQuantity()
    {
        _cartItemAccessorMock.Setup(a => a.GetCartItem(1))
            .Returns(new CartItem { Id = 1, Quantity = 2 });

        _cartEngine.UpdateCartItemQuantity(1, 4);

        _cartItemAccessorMock.Verify(a => a.UpdateCartItemQuantity(1, 4), Times.Once);
    }

    [TestMethod]
    public void UpdateCartItemQuantity_ZeroQuantity_DeletesItem()
    {
        _cartItemAccessorMock.Setup(a => a.GetCartItem(1))
            .Returns(new CartItem { Id = 1, Quantity = 2 });

        _cartEngine.UpdateCartItemQuantity(1, 0);

        _cartItemAccessorMock.Verify(a => a.DeleteCartItem(1), Times.Once);
    }

    [TestMethod]
    public void RemoveCartItem_ExistingItem_DeletesItem()
    {
        _cartItemAccessorMock.Setup(a => a.GetCartItem(1))
            .Returns(new CartItem { Id = 1 });

        _cartEngine.RemoveCartItem(1);

        _cartItemAccessorMock.Verify(a => a.DeleteCartItem(1), Times.Once);
    }

    [TestMethod]
    public void RemoveCartItem_InvalidId_ThrowsException()
    {
        try
        {
            _cartEngine.RemoveCartItem(0);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void ClearCart_ExistingCart_DeletesAllCartItems()
    {
        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(new Cart { Id = 1 });

        _cartEngine.ClearCart(1);

        _cartItemAccessorMock.Verify(a => a.DeleteAllCartItems(1), Times.Once);
    }

    [TestMethod]
    public void DeleteCart_ExistingCart_DeletesItemsAndCart()
    {
        _cartAccessorMock.Setup(a => a.GetCart(1)).Returns(new Cart { Id = 1 });

        _cartEngine.DeleteCart(1);

        _cartItemAccessorMock.Verify(a => a.DeleteAllCartItems(1), Times.Once);
        _cartAccessorMock.Verify(a => a.DeleteCart(1), Times.Once);
    }
}