-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProjectDB')
    CREATE DATABASE ProjectDB;
GO

USE ProjectDB;
GO

-- Example table, replace with your actual schema
CREATE TABLE Users (
    Id       INT PRIMARY KEY IDENTITY,
    Name     NVARCHAR(100) NOT NULL,
    Email    NVARCHAR(255) NOT NULL UNIQUE,
    Created  DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE Orders (
    Id        INT PRIMARY KEY IDENTITY,
    UserId    INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Total     DECIMAL(10,2) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE()
);
GO