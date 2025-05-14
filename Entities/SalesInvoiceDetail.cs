using System;

namespace ConvenienceStoreManager.Entities
{
    /// <summary>
    /// Lớp đại diện cho chi tiết hóa đơn bán hàng
    /// </summary>
    public class SalesInvoiceDetail
    {
        /// <summary>
        /// Mã chi tiết hóa đơn
        /// </summary>
        public int InvoiceDetailID { get; set; }

        /// <summary>
        /// Mã hóa đơn
        /// </summary>
        public int InvoiceID { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Số lượng bán
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Đơn giá (giá bán tại thời điểm lập hóa đơn)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Thành tiền (Quantity * UnitPrice)
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Thông tin sản phẩm (optional, để hiển thị trên UI)
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Đơn vị tính của sản phẩm (optional, để hiển thị trên UI)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Constructor không tham số
        /// </summary>
        public SalesInvoiceDetail()
        {
            Quantity = 0;
            UnitPrice = 0;
            Subtotal = 0;
            ProductName = string.Empty;
            Unit = string.Empty;
        }

        /// <summary>
        /// Constructor đầy đủ tham số
        /// </summary>
        public SalesInvoiceDetail(int invoiceDetailID, int invoiceID, int productID, int quantity, decimal unitPrice, decimal subtotal)
        {
            InvoiceDetailID = invoiceDetailID;
            InvoiceID = invoiceID;
            ProductID = productID;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Subtotal = subtotal;
            ProductName = string.Empty;
            Unit = string.Empty;
        }

        /// <summary>
        /// Constructor cho tạo mới chi tiết hóa đơn với thông tin sản phẩm
        /// </summary>
        public SalesInvoiceDetail(int productID, string productName, string unit, int quantity, decimal unitPrice)
        {
            ProductID = productID;
            ProductName = productName;
            Unit = unit;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Subtotal = quantity * unitPrice;
        }
    }
}