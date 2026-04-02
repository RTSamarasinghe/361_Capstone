USE ProjectDB;
GO

INSERT INTO Users (Name, Email) VALUES
    ('Alice', 'alice@example.com'),
    ('Bob',   'bob@example.com');
GO

INSERT INTO Orders (UserId, Total) VALUES
    (1, 49.99),
    (2, 120.00);
GO