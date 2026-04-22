IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProjectDB')
    CREATE DATABASE ProjectDB;
GO

USE ProjectDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Category' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    Name NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentMethod' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE PaymentMethod (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    CardNumberHash NVARCHAR(255) NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    CardholderName NVARCHAR(100) NOT NULL,
    PinHash NVARCHAR(255) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cart' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Cart (
    Id INT PRIMARY KEY IDENTITY NOT NULL    
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Product' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(18, 2) NOT NULL,
    CategoryId INT NOT NULL,
    ImageURL NVARCHAR(255),
    Manufacturer NVARCHAR(100),
    Rating DECIMAL(3, 2),
    Sku NVARCHAR(50),
    StockQuantity INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CartItem' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE CartItem (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Cart(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sale' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Sale (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME,
    DiscountAmount DECIMAL(18, 2),
    DiscountPercent DECIMAL(5, 2)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SaleItem' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE SaleItem (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    SaleId INT NOT NULL,
    ProductId INT NOT NULL,
    FOREIGN KEY (SaleId) REFERENCES Sale(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SaleCategory' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE SaleCategory (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    SaleId INT NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (SaleId) REFERENCES Sale(Id),
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customer' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Customer (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Created DATETIME DEFAULT GETDATE(),
    PassHash NVARCHAR(100) NOT NULL,
    UserCart INT NOT NULL,
    PaymentMethodId INT,
    FOREIGN KEY (UserCart) REFERENCES Cart(Id),
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethod(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Address' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Address (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    CustomerId INT NOT NULL,
    Street NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    PostalCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE [Order] (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    CustomerId INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE() NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    OrderStatus NVARCHAR(50) NOT NULL,
    ShippingAddressId INT NOT NULL,
    BillingAddressId INT NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
    FOREIGN KEY (ShippingAddressId) REFERENCES Address(Id),
    FOREIGN KEY (BillingAddressId) REFERENCES Address(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ORDERITEM' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE ORDERITEM (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payment' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE Payment (
    Id INT PRIMARY KEY IDENTITY NOT NULL,
    OrderId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentMethod INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (PaymentMethod) REFERENCES PaymentMethod(ID)
);