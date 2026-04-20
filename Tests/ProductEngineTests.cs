using DataContracts;
using Moq;

namespace ProductTest;

[TestClass]
public class ProductEngineTests
{
    private Mock<IProductAccessor> _productAccessorMock = null!;
    private ProductEngine _productEngine = null!;

    [TestInitialize]
    public void Setup()
    {
        _productAccessorMock = new Mock<IProductAccessor>();
        _productEngine = new ProductEngine(_productAccessorMock.Object);
    }

    // =========================
    // AddProduct
    // =========================

    [TestMethod]
    public void AddProduct_ValidInput_ReturnsId()
    {
        _productAccessorMock
            .Setup(a => a.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5))
            .Returns(1);

        int result = _productEngine.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);

        Assert.AreEqual(1, result);
        _productAccessorMock.Verify(a => a.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5), Times.Once);
    }

    [TestMethod]
    public void AddProduct_InvalidName_ThrowsException()
    {
        try
        {
            _productEngine.AddProduct(" ", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
        }
    }

    // =========================
    // GetProduct
    // =========================

    [TestMethod]
    public void GetProduct_ValidId_ReturnsProduct()
    {
        var product = new Product { Id = 1, Name = "Test" };
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns(product);

        var result = _productEngine.GetProduct(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public void GetProduct_NotFound_ThrowsException()
    {
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns((Product)null!);

        try
        {
            _productEngine.GetProduct(1);
            Assert.Fail("Expected Exception not thrown");
        }
        catch (Exception)
        {
        }
    }

    // =========================
    // GetProductsByCategory
    // =========================

    [TestMethod]
    public void GetProductsByCategory_Valid_ReturnsList()
    {
        var list = new List<Product> { new Product { Id = 1 } };

        _productAccessorMock.Setup(a => a.GetProductsByCategory(1)).Returns(list);

        var result = _productEngine.GetProductsByCategory(1);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetProductsByCategory_InvalidId_ThrowsException()
    {
        try
        {
            _productEngine.GetProductsByCategory(0);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
        }
    }

    // =========================
    // UpdateProduct
    // =========================

    [TestMethod]
    public void UpdateProduct_ValidInput_CallsAccessor()
    {
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns(new Product { Id = 1 });

        _productEngine.UpdateProduct(1, "Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);

        _productAccessorMock.Verify(a =>
            a.UpdateProduct(1, "Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5),
            Times.Once);
    }

    [TestMethod]
    public void UpdateProduct_ProductDoesNotExist_ThrowsException()
    {
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns((Product)null!);

        try
        {
            _productEngine.UpdateProduct(1, "Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
        }
    }

    // =========================
    // UpdateStockQuantity
    // =========================

    [TestMethod]
    public void UpdateStockQuantity_Valid_CallsAccessor()
    {
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns(new Product { Id = 1 });

        _productEngine.UpdateStockQuantity(1, 10);

        _productAccessorMock.Verify(a => a.UpdateStockQuantity(1, 10), Times.Once);
    }

    [TestMethod]
    public void UpdateStockQuantity_Negative_ThrowsException()
    {
        try
        {
            _productEngine.UpdateStockQuantity(1, -1);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
        }
    }

    // =========================
    // DeleteProduct
    // =========================

    [TestMethod]
    public void DeleteProduct_Valid_CallsAccessor()
    {
        _productAccessorMock.Setup(a => a.GetProduct(1)).Returns(new Product { Id = 1 });

        _productEngine.DeleteProduct(1);

        _productAccessorMock.Verify(a => a.DeleteProduct(1), Times.Once);
    }

    [TestMethod]
    public void DeleteProduct_InvalidId_ThrowsException()
    {
        try
        {
            _productEngine.DeleteProduct(0);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
        }
    }
}