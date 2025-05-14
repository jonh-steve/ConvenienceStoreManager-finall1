using System;
using System.Collections.Generic;

namespace ConvenienceStoreManager.Entities
{
    /// <summary>
    /// Lớp đại diện cho phiếu nhập hàng
    /// </summary>
    public class PurchaseOrder
    {
        /// <summary>
        /// Mã phiếu nhập
        /// </summary>
        public int PurchaseOrderID { get; set; }

        /// <summary>
        /// Ngày tạo phiếu nhập
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// Tổng tiền phiếu nhập
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Danh sách chi tiết phiếu nhập
        /// </summary>
        public List<PurchaseOrderDetail> Details { get; set; }

        /// <summary>
        /// Constructor không tham số
        /// </summary>
        public PurchaseOrder()
        {
            OrderDate = DateTime.Now;
            SupplierName = string.Empty;
            TotalAmount = 0;
            Notes = string.Empty;
            Details = new List<PurchaseOrderDetail>();
        }

        /// <summary>
        /// Constructor đầy đủ tham số
        /// </summary>
        public PurchaseOrder(int purchaseOrderID, DateTime orderDate, string supplierName, decimal totalAmount, string notes)
        {
            PurchaseOrderID = purchaseOrderID;
            OrderDate = orderDate;
            SupplierName = supplierName;
            TotalAmount = totalAmount;
            Notes = notes;
            Details = new List<PurchaseOrderDetail>();
        }
    }
}