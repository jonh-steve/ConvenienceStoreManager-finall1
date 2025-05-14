using System;
using System.Collections.Generic;

namespace ConvenienceStoreManager.Entities
{
    /// <summary>
    /// Lớp đại diện cho hóa đơn bán hàng
    /// </summary>
    public class SalesInvoice
    {
        /// <summary>
        /// Mã hóa đơn
        /// </summary>
        public int InvoiceID { get; set; }

        /// <summary>
        /// Ngày tạo hóa đơn
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Tổng tiền hóa đơn
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Danh sách chi tiết hóa đơn
        /// </summary>
        public List<SalesInvoiceDetail> Details { get; set; }

        /// <summary>
        /// Constructor không tham số
        /// </summary>
        public SalesInvoice()
        {
            InvoiceDate = DateTime.Now;
            TotalAmount = 0;
            Notes = string.Empty;
            Details = new List<SalesInvoiceDetail>();
        }

        /// <summary>
        /// Constructor đầy đủ tham số
        /// </summary>
        public SalesInvoice(int invoiceID, DateTime invoiceDate, decimal totalAmount, string notes)
        {
            InvoiceID = invoiceID;
            InvoiceDate = invoiceDate;
            TotalAmount = totalAmount;
            Notes = notes;
            Details = new List<SalesInvoiceDetail>();
        }
    }
}