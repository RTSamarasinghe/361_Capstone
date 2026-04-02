-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProjectDB')
    CREATE DATABASE ProjectDB;
GO

USE ProjectDB;
GO

-- Example table, replace with your actual schema
CREATE TABLE Customer (
    Id              INT PRIMARY KEY IDENTITY NOT NULL,
    Name            NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(255) NOT NULL UNIQUE,
    Created         DATETIME DEFAULT GETDATE(),
    PassHash        NVARCHAR(100) NOT NULL,
    UserCart        INT NOT NULL,
    PaymentMethodId INT NOT NULL,
    Address         NVARCHAR(255) NOT NULL,
    FOREIGN KEY (UserCart) REFERENCES Cart(Id),
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethod(Id),
    FOREIGN KEY (Address) REFERENCES Address(Id)
);
GO

CREATE TABLE [Order] (
    Id              INT PRIMARY KEY NOT NULL,
    Items           List<INT> NOT NULL,
    CustomerId      INT NOT NULL,
    OrderDate       DATETIME NOT NULL,
    TotalAmount     DECIMAL(18, 2) NOT NULL,
    OrderStatus     STRING NOT NULL,
    PaymentId       INT NOT NULL,
    ShippingAddressId NVARCHAR(255) NOT NULL,
    BillingAddressId  NVARCHAR(255) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
    FOREIGN KEY (PaymentId) REFERENCES Payment(Id),
    FOREIGN KEY (ShippingAddressId) REFERENCES Address(Id),
    FOREIGN KEY (BillingAddressId) REFERENCES Address(Id)
);

CREATE TABLE Payment (
    Id              INT PRIMARY KEY NOT NULL,
    OrderId         INT NOT NULL,
    Amount          DECIMAL(18, 2) NOT NULL,
    PaymentDate     DATETIME NOT NULL,
    PaymentMethod   INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (PaymentMethod) REFERENCES PaymentMethod(ID)
);

CREATE TABLE PaymentMethod (
    ID             INT PRIMARY KEY NOT NULL,
    CardNumberHash NVARCHAR(255) NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    CardholderName NVARCHAR(100) NOT NULL,
    PinHash        NVARCHAR(255) NOT NULL
);

CREATE TABLE Cart (
    Id              INT PRIMARY KEY NOT NULL,    
);

CREATE TABLE CartItem (
    Id              INT PRIMARY KEY NOT NULL,
    CartId          INT NOT NULL,
    ProductId       INT NOT NULL,
    Quantity        INT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Cart(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);

CREATE TABLE Product (
    Id              INT PRIMARY KEY NOT NULL,
    Name            NVARCHAR(100) NOT NULL,
    Description     NVARCHAR(255),
    Price           DECIMAL(18, 2) NOT NULL,
    CategoryId      INT NOT NULL,
    ImageURL        NVARCHAR(255),
    Manufacturer    NVARCHAR(100),
    Rating          DECIMAL(3, 2),
    Sku             NVARCHAR(50),
    StockQuantity   INT NOT NULL
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
    );

CREATE TABLE Category (
    Id              INT PRIMARY KEY NOT NULL,
    Name            NVARCHAR(100) NOT NULL
);

CREATE TABLE Sale (
    Id              INT PRIMARY KEY NOT NULL,
    StartDate       DATETIME NOT NULL,
    EndDate         DATETIME,
    DiscountAmount  DECIMAL(18, 2),
    DiscountPercent DECIMAL(5, 2),
    Products        List<INT>,
    Categories      List<INT>
    FOREIGN KEY (Products) REFERENCES Product(Id),
    FOREIGN KEY (Categories) REFERENCES Category(Id)
);

CREATE TABLE Address (
    Id              INT PRIMARY KEY NOT NULL,
    CustomerId      INT NOT NULL,
    Street          NVARCHAR(255) NOT NULL,
    City            NVARCHAR(100) NOT NULL,
    State           NVARCHAR(100) NOT NULL,
    PostalCode      NVARCHAR(20) NOT NULL,
    Country         NVARCHAR(100) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);