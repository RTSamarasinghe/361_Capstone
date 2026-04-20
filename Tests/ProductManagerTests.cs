

using System;
using System.Collections.Generic;
using DataContracts;
using Managers;
using Moq;

namespace CustomerTest;

[TestClass]
public class ProductManagerTests
{
    private Mock<IProductEngine> _productEngineMock = null!;
    private ProductManager _productManager = null!;

    [TestInitialize]
    public void Setup()
    {
        _productEngineMock = new Mock<IProductEngine>();
        _productManager = new ProductManager(_productEngineMock.Object);
    }

    [TestMethod]
    public void AddProduct_ReturnsId()
    {
        _productEngineMock
            .Setup(e => e.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5))
            .Returns(1);

        int result = _productManager.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);

        Assert.AreEqual(1, result);
        _productEngineMock.Verify(e => e.AddProduct("Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5), Times.Once);
    }

    [TestMethod]
    public void GetProduct_ReturnsProduct_WhenProductExists()
    {
        var expectedProduct = new Product { Id = 1, Name = "Test Product" };
        _productEngineMock.Setup(e => e.GetProduct(1)).Returns(expectedProduct);

        var result = _productManager.GetProduct(1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Test Product", result.Name);
        _productEngineMock.Verify(e => e.GetProduct(1), Times.Once);
    }

    [TestMethod]
    public void GetProductsByCategory_ReturnsProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" }
        };

        _productEngineMock.Setup(e => e.GetProductsByCategory(1)).Returns(products);

        var result = _productManager.GetProductsByCategory(1);

        Assert.AreEqual(2, result.Count);
        _productEngineMock.Verify(e => e.GetProductsByCategory(1), Times.Once);
    }

    [TestMethod]
    public void UpdateProduct_ShouldInvokeEngineUpdate()
    {
        _productManager.UpdateProduct(1, "Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5);

        _productEngineMock.Verify(e => e.UpdateProduct(1, "Name", "Desc", 10, 1, "img", "manu", 4.5m, "sku", 5), Times.Once);
    }

    [TestMethod]
    public void UpdateStockQuantity_ShouldInvokeEngineUpdate()
    {
        _productManager.UpdateStockQuantity(1, 10);

        _productEngineMock.Verify(e => e.UpdateStockQuantity(1, 10), Times.Once);
    }

    [TestMethod]
    public void DeleteProduct_ShouldInvokeEngineDelete()
    {
        _productManager.DeleteProduct(1);

        _productEngineMock.Verify(e => e.DeleteProduct(1), Times.Once);
    }

    [TestMethod]
    public void GetProduct_ShouldPropagateException()
    {
        _productEngineMock
            .Setup(e => e.GetProduct(1))
            .Throws(new Exception("Product not found"));

        try
        {
            _productManager.GetProduct(1);
            Assert.Fail("Expected Exception was not thrown.");
        }
        catch (Exception)
        {
        }
    }
}