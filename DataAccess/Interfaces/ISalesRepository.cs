// Vị trí: ConvenienceStoreManager/DataAccess/Interfaces/ISalesRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.DataAccess.Interfaces
{
    public interface ISalesRepository
    {
        // Phương thức đồng bộ (giữ lại để tương thích với code cũ)
        List<SalesInvoice> GetAllInvoices();
        SalesInvoice GetInvoiceById(int invoiceID);
        List<SalesInvoiceDetail> GetInvoiceDetails(int invoiceID);
        int CreateInvoice(SalesInvoice invoice, List<SalesInvoiceDetail> details);
        List<SalesInvoice> GetInvoicesByDateRange(DateTime fromDate, DateTime toDate);
        bool InvoiceExists(int invoiceID);

        // Phương thức bất đồng bộ mới
        Task<List<SalesInvoice>> GetAllInvoicesAsync();
        Task<SalesInvoice> GetInvoiceByIdAsync(int invoiceID);
        Task<List<SalesInvoiceDetail>> GetInvoiceDetailsAsync(int invoiceID);
        Task<int> CreateInvoiceAsync(SalesInvoice invoice, List<SalesInvoiceDetail> details);
        Task<List<SalesInvoice>> GetInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<bool> InvoiceExistsAsync(int invoiceID);
        Task<decimal> CalculateRevenueAsync(DateTime fromDate, DateTime toDate);
    }
}