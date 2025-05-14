using System;

namespace ConvenienceStoreManager.Entities
{
    /// <summary>
    /// Lớp đại diện cho chi tiết phiếu nhập hàng
    /// </summary>
    public class PurchaseOrderDetail
    {
        /// <summary>
        /// Mã chi tiết phiếu nhập
        /// </summary>
        public int PurchaseOrderDetailID { get; set; }

        /// <summary>
        /// Mã phiếu nhập
        /// </summary>
        public int PurchaseOrderID { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Số lượng nhập
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Giá nhập
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Thành tiền (Quantity * PurchasePrice)
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
        public PurchaseOrderDetail()
        {
            Quantity = 0;
            PurchasePrice = 0;
            Subtotal = 0;
            ProductName = string.Empty;
            Unit = string.Empty;
        }

        /// <summary>
        /// Constructor đầy đủ tham số
        /// </summary>
        public PurchaseOrderDetail(int purchaseOrderDetailID, int purchaseOrderID, int productID, int quantity, decimal purchasePrice, decimal subtotal)
        {
            PurchaseOrderDetailID = purchaseOrderDetailID;
            PurchaseOrderID = purchaseOrderID;
            ProductID = productID;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            Subtotal = subtotal;
            ProductName = string.Empty;
            Unit = string.Empty;
        }

        /// <summary>
        /// Constructor cho tạo mới chi tiết phiếu nhập với thông tin sản phẩm
        /// </summary>
        public PurchaseOrderDetail(int productID, string productName, string unit, int quantity, decimal purchasePrice)
        {
            ProductID = productID;
            ProductName = productName;
            Unit = unit;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            Subtotal = quantity * purchasePrice;
        }
    }
}