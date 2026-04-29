USE ProjectDB;
GO

INSERT INTO Category (Name) VALUES ('Nonsense');
INSERT INTO Category (Name) VALUES ('Vaccum Cleaners');
GO

INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity) 
VALUES ('Dehydrated Water', 'Water that has been dehydrated to remove all moisture', 9.99, 1, 'Images/DehydratedWater.png', 'Nonsense Inc.', 4.5, 'DW-001', 100);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Fireproof Matches', 'Matches that are fireproof, to prevent fires', 17.38, 1, 'Images/FireproofMatches.png', 'Nonsense Inc.', 1.4, 'FPM-001', 200);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Silent Alarm Clock', 'An alarm clock that doesn''t make any noise', 28.99, 1, 'Images/SilentAlarmClock.png', 'Nonsense Inc.', 3.2, 'SAC-001', 150);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Waterproof Sponges', 'Sponges that are waterproof and won''t absorb water', 3.50, 1, 'Images/WaterproofSponge.png', 'Nonsense Inc.', 2.8, 'WPS-001', 80);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Wireless Extension Cord', 'An extension cord that is wireless and doesn''t require plugging in', 7.37, 1, 'Images/WirelessExtensionCord.png', 'Nonsense Inc.', 0.4, 'WEC-001', 120);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Bissell2252', 'A powerful vacuum cleaner for deep cleaning', 199.99, 2, 'Images/Bissell2252.png', 'Bissell', 4.7, 'BIS-2252', 50);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Hoover WindTunnel 3', 'A versatile vacuum cleaner with advanced suction technology', 249.99, 2, 'Images/HooverWindTunnel.png', 'Hoover', 4.5, 'HOO-WT3', 30);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Shark NV352', 'A compact vacuum cleaner with powerful suction and easy maneuverability', 129.99, 2, 'Images/SharkNV352.png', 'Shark', 4.3, 'SHARK-NV352', 60);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Shark Rocket', 'A lightweight vacuum cleaner with powerful suction and versatile attachments', 149.99, 2, 'Images/SharkRocket.png', 'Shark', 4.6, 'SHARK-ROCKET', 40);
INSERT INTO Product (Name, Description, Price, CategoryId, ImageURL, Manufacturer, Rating, Sku, StockQuantity)
VALUES('Miele Guard M1', 'A high-end vacuum cleaner with advanced suction technology', 249.99, 2, 'Images/MieleGuardM1.png', 'Miele', 4.8, 'MIELE-GM1', 25);
GO

INSERT INTO Sale (StartDate, EndDate, DiscountAmount, DiscountPercent)
VALUES ('2024-01-01', '2050-04-20', 1.00, NULL);
INSERT INTO Sale (StartDate, EndDate, DiscountAmount, DiscountPercent)
VALUES ('2025-04-01', '2026-06-15', NULL, 15.00);
GO

INSERT INTO SaleItem (SaleId, ProductId)
VALUES (1, 3);
INSERT INTO SaleCategory (SaleId, CategoryId)
VALUES (2, 2);
GO