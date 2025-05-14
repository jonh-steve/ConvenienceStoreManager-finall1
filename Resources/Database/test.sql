

-- File: ConvenienceStoreManager/Resources/Database/InsertSampleData.sql
-- Chức năng: Chèn thêm dữ liệu mẫu để test giao diện dễ thương hồng hào
-- Bé Yêu nhớ chạy file này sau khi đã tạo database bằng CreateDatabase.sql nhé!

USE ConvenienceStoreDB;
GO

-- Thêm dữ liệu mẫu cho bảng Products (10 sản phẩm dễ thương)
INSERT INTO Products (ProductName, ProductCode, Unit, SellingPrice, StockQuantity) VALUES
(N'Bánh quy Hello Kitty', 'HK001', N'Hộp', 25000, 40),
(N'Kẹo mút Dâu Hồng', 'KMH001', N'Gói', 12000, 60),
(N'Sữa chua uống Yomost Dâu', 'YM001', N'Chai', 9000, 80),
(N'Bút bi màu hồng Sakura', 'BBH001', N'Cây', 7000, 100),
(N'Gấu bông mini Pinky', 'GB001', N'Con', 55000, 20),
(N'Bình nước Kitty', 'BNK001', N'Bình', 35000, 30),
(N'Khăn tay hồng Pastel', 'KTH001', N'Cái', 15000, 50),
(N'Bánh Pocky Dâu', 'PKD001', N'Hộp', 18000, 70),
(N'Kẹp tóc nơ hồng', 'KTH002', N'Cái', 8000, 90),
(N'Sổ tay Cute Pink', 'STP001', N'Cuốn', 22000, 35);
GO

-- Thêm dữ liệu mẫu cho bảng SalesInvoices (5 hóa đơn bán hàng)
INSERT INTO SalesInvoices (InvoiceDate, TotalAmount, Notes) VALUES
('2024-06-01 09:00:00', 50000, N'Khách hàng dễ thương mua bánh và kẹo'),
('2024-06-02 10:30:00', 35000, N'Mua bút bi và sổ tay màu hồng'),
('2024-06-03 14:15:00', 27000, N'Mua kẹo mút và khăn tay pastel'),
('2024-06-04 16:45:00', 43000, N'Mua gấu bông mini Pinky'),
('2024-06-05 11:20:00', 38000, N'Mua bình nước Kitty và bánh Pocky Dâu');
GO

-- Thêm dữ liệu mẫu cho bảng SalesInvoiceDetails (chi tiết hóa đơn)
INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, Subtotal) VALUES
(1, 11, 2, 25000, 50000), -- Bánh quy Hello Kitty
(2, 14, 3, 7000, 21000),  -- Bút bi màu hồng Sakura
(2, 20, 1, 22000, 22000), -- Sổ tay Cute Pink
(3, 12, 2, 12000, 24000), -- Kẹo mút Dâu Hồng
(3, 17, 1, 15000, 15000), -- Khăn tay hồng Pastel
(4, 15, 1, 55000, 55000), -- Gấu bông mini Pinky
(5, 16, 1, 35000, 35000), -- Bình nước Kitty
(5, 18, 1, 18000, 18000); -- Bánh Pocky Dâu
GO

-- Thêm dữ liệu mẫu cho bảng PurchaseOrders (3 phiếu nhập hàng)
INSERT INTO PurchaseOrders (OrderDate, SupplierName, TotalAmount, Notes) VALUES
('2024-05-30 08:00:00', N'Công ty Đồ chơi Pinky', 110000, N'Nhập gấu bông và bình nước'),
('2024-05-31 13:00:00', N'Công ty Bánh kẹo Dâu', 90000, N'Nhập bánh quy và kẹo mút'),
('2024-06-01 15:30:00', N'Công ty Văn phòng phẩm Sakura', 99000, N'Nhập bút bi và sổ tay');
GO

-- Thêm dữ liệu mẫu cho bảng PurchaseOrderDetails (chi tiết phiếu nhập)
INSERT INTO PurchaseOrderDetails (PurchaseOrderID, ProductID, Quantity, PurchasePrice, Subtotal) VALUES
(1, 15, 10, 50000, 500000), -- Gấu bông mini Pinky
(1, 16, 15, 30000, 450000), -- Bình nước Kitty
(2, 11, 20, 20000, 400000), -- Bánh quy Hello Kitty
(2, 12, 30, 10000, 300000), -- Kẹo mút Dâu Hồng
(3, 14, 50, 6000, 300000),  -- Bút bi màu hồng Sakura
(3, 20, 20, 20000, 400000); -- Sổ tay Cute Pink
GO

PRINT N'Đã nạp dữ liệu mẫu dễ thương hồng hào cho ConvenienceStoreDB! 🩷';

-- Kết thúc file: ConvenienceStoreManager/Resources/Database/InsertSampleData.sql

