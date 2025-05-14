-- Script tạo cơ sở dữ liệu cho phần mềm quản lý cửa hàng tiện lợi
-- Vị trí file: ConvenienceStoreManager/Resources/Database/CreateDatabase.sql

-- Tạo cơ sở dữ liệu
CREATE DATABASE ConvenienceStoreDB;
GO

USE ConvenienceStoreDB;
GO

-- Bảng Products (Sản phẩm)
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    ProductCode NVARCHAR(20) UNIQUE,
    Unit NVARCHAR(20),
    SellingPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0
);
GO

-- Bảng SalesInvoices (Hóa đơn Bán hàng)
CREATE TABLE SalesInvoices (
    InvoiceID INT PRIMARY KEY IDENTITY(1,1),
    InvoiceDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(200)
);
GO

-- Bảng SalesInvoiceDetails (Chi tiết Hóa đơn Bán hàng)
CREATE TABLE SalesInvoiceDetails (
    InvoiceDetailID INT PRIMARY KEY IDENTITY(1,1),
    InvoiceID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (InvoiceID) REFERENCES SalesInvoices(InvoiceID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Bảng PurchaseOrders (Phiếu Nhập hàng)
CREATE TABLE PurchaseOrders (
    PurchaseOrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    SupplierName NVARCHAR(100),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(200)
);
GO

-- Bảng PurchaseOrderDetails (Chi tiết Phiếu Nhập hàng)
CREATE TABLE PurchaseOrderDetails (
    PurchaseOrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseOrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    PurchasePrice DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (PurchaseOrderID) REFERENCES PurchaseOrders(PurchaseOrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Thêm một số dữ liệu mẫu cho bảng Products
INSERT INTO Products (ProductName, ProductCode, Unit, SellingPrice, StockQuantity)
VALUES 
('Sữa tươi Vinamilk 180ml', 'VM001', 'Hộp', 7000, 100),
('Bánh mì sandwich', 'BM001', 'Gói', 15000, 50),
('Nước suối Aquafina 500ml', 'AQ001', 'Chai', 5000, 200),
('Coca Cola 330ml', 'CC001', 'Lon', 10000, 150),
('Mì gói Hảo Hảo', 'MG001', 'Gói', 3500, 300),
('Kẹo Alpenliebe', 'KA001', 'Gói', 8000, 80),
('Bia Tiger 330ml', 'BT001', 'Lon', 15000, 100),
('Dầu gội Head & Shoulders 170ml', 'DG001', 'Chai', 49000, 30),
('Nước rửa chén Sunlight 400g', 'NRC001', 'Chai', 25000, 40),
('Khăn giấy Pulppy', 'KG001', 'Gói', 12000, 60);
GO

-- Thêm một Stored Procedure để cập nhật số lượng tồn kho
CREATE PROCEDURE UpdateProductStock
    @ProductID INT,
    @QuantityChange INT
AS
BEGIN
    UPDATE Products
    SET StockQuantity = StockQuantity + @QuantityChange
    WHERE ProductID = @ProductID;
    
    -- Trả về số lượng tồn kho sau khi cập nhật
    SELECT StockQuantity FROM Products WHERE ProductID = @ProductID;
END
GO

-- Thêm một Stored Procedure để tạo hóa đơn bán hàng và chi tiết
CREATE PROCEDURE CreateSalesInvoice
    @InvoiceDate DATETIME,
    @TotalAmount DECIMAL(18,2),
    @Notes NVARCHAR(200)
AS
BEGIN
    -- Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Thêm hóa đơn
        INSERT INTO SalesInvoices (InvoiceDate, TotalAmount, Notes)
        VALUES (@InvoiceDate, @TotalAmount, @Notes);
        
        -- Lấy ID hóa đơn vừa tạo
        DECLARE @InvoiceID INT = SCOPE_IDENTITY();
        
        -- Trả về ID hóa đơn
        SELECT @InvoiceID AS InvoiceID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        -- Ném lại lỗi để catch ở code .NET
        THROW;
    END CATCH;
END
GO

-- Thêm một Stored Procedure để tạo chi tiết hóa đơn bán hàng
CREATE PROCEDURE CreateSalesInvoiceDetail
    @InvoiceID INT,
    @ProductID INT,
    @Quantity INT,
    @UnitPrice DECIMAL(18,2),
    @Subtotal DECIMAL(18,2)
AS
BEGIN
    -- Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Thêm chi tiết hóa đơn
        INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, Subtotal)
        VALUES (@InvoiceID, @ProductID, @Quantity, @UnitPrice, @Subtotal);
        
        -- Cập nhật số lượng tồn kho
        UPDATE Products
        SET StockQuantity = StockQuantity - @Quantity
        WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        -- Ném lại lỗi để catch ở code .NET
        THROW;
    END CATCH;
END
GO

-- Thêm một Stored Procedure để tạo phiếu nhập hàng và chi tiết
CREATE PROCEDURE CreatePurchaseOrder
    @OrderDate DATETIME,
    @SupplierName NVARCHAR(100),
    @TotalAmount DECIMAL(18,2),
    @Notes NVARCHAR(200)
AS
BEGIN
    -- Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Thêm phiếu nhập
        INSERT INTO PurchaseOrders (OrderDate, SupplierName, TotalAmount, Notes)
        VALUES (@OrderDate, @SupplierName, @TotalAmount, @Notes);
        
        -- Lấy ID phiếu nhập vừa tạo
        DECLARE @PurchaseOrderID INT = SCOPE_IDENTITY();
        
        -- Trả về ID phiếu nhập
        SELECT @PurchaseOrderID AS PurchaseOrderID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        -- Ném lại lỗi để catch ở code .NET
        THROW;
    END CATCH;
END
GO

-- Thêm một Stored Procedure để tạo chi tiết phiếu nhập hàng
CREATE PROCEDURE CreatePurchaseOrderDetail
    @PurchaseOrderID INT,
    @ProductID INT,
    @Quantity INT,
    @PurchasePrice DECIMAL(18,2),
    @Subtotal DECIMAL(18,2)
AS
BEGIN
    -- Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Thêm chi tiết phiếu nhập
        INSERT INTO PurchaseOrderDetails (PurchaseOrderID, ProductID, Quantity, PurchasePrice, Subtotal)
        VALUES (@PurchaseOrderID, @ProductID, @Quantity, @PurchasePrice, @Subtotal);
        
        -- Cập nhật số lượng tồn kho
        UPDATE Products
        SET StockQuantity = StockQuantity + @Quantity
        WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        -- Ném lại lỗi để catch ở code .NET
        THROW;
    END CATCH;
END
GO

PRINT 'Đã tạo xong cơ sở dữ liệu ConvenienceStoreDB!';