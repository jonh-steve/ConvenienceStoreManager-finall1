using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConvenienceStoreManager.DataAccess.Interfaces;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.BusinessLogic
{
    /// <summary>
    /// Lớp xử lý logic nghiệp vụ liên quan đến hóa đơn bán hàng.
    /// </summary>
    public class SalesService
    {
        private readonly ISalesRepository salesRepository;
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Constructor với Dependency Injection.
        /// </summary>
        /// <param name="salesRepository">Repository xử lý dữ liệu hóa đơn.</param>
        /// <param name="productRepository">Repository xử lý dữ liệu sản phẩm.</param>
        public SalesService(ISalesRepository salesRepository, IProductRepository productRepository)
        {
            this.salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        #region Phương thức bất đồng bộ

        /// <summary>
        /// Tạo hóa đơn mới và các chi tiết liên quan.
        /// </summary>
        /// <param name="invoice">Thông tin hóa đơn.</param>
        /// <param name="details">Danh sách chi tiết hóa đơn.</param>
        /// <returns>ID của hóa đơn vừa tạo.</returns>
        /// <exception cref="ArgumentNullException">Hóa đơn hoặc chi tiết null.</exception>
        /// <exception cref="ArgumentException">Dữ liệu không hợp lệ.</exception>
        /// <exception cref="Exception">Lỗi khi kiểm tra tồn kho hoặc tạo hóa đơn.</exception>
        public async Task<int> CreateInvoiceAsync(SalesInvoice invoice, List<SalesInvoiceDetail> details)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice), "Hóa đơn không được phép null");

            if (details == null || !details.Any())
                throw new ArgumentException("Chi tiết hóa đơn không được phép rỗng", nameof(details));

            if (invoice.TotalAmount <= 0)
                throw new ArgumentException("Tổng tiền hóa đơn phải lớn hơn 0", nameof(invoice.TotalAmount));

            // Lấy danh sách sản phẩm để kiểm tra tồn kho
            var productIds = details.Select(d => d.ProductID).Distinct().ToList();
            var products = await productRepository.GetProductsByIdsAsync(productIds);
            var productDict = products.ToDictionary(p => p.ProductID, p => p);

            // Kiểm tra tồn kho và dữ liệu chi tiết
            foreach (var detail in details)
            {
                Product product;
                if (!productDict.TryGetValue(detail.ProductID, out product))
                    throw new Exception($"Không tìm thấy sản phẩm có ID {detail.ProductID}");

                if (detail.Quantity <= 0)
                    throw new ArgumentException($"Số lượng sản phẩm {product.ProductName} phải lớn hơn 0", nameof(detail.Quantity));

                if (detail.Quantity > product.StockQuantity)
                    throw new Exception($"Sản phẩm {product.ProductName} chỉ còn {product.StockQuantity} {product.Unit} trong kho, không đủ số lượng yêu cầu ({detail.Quantity})");

                if (detail.UnitPrice <= 0)
                    throw new ArgumentException($"Đơn giá sản phẩm {product.ProductName} phải lớn hơn 0", nameof(detail.UnitPrice));

                if (Math.Abs(detail.Subtotal - (detail.Quantity * detail.UnitPrice)) > 0.01m)
                    throw new ArgumentException($"Thành tiền của sản phẩm {product.ProductName} không chính xác", nameof(detail.Subtotal));
            }

            // Kiểm tra tổng tiền
            decimal calculatedTotal = details.Sum(d => d.Subtotal);
            if (Math.Abs(calculatedTotal - invoice.TotalAmount) > 0.01m)
                throw new ArgumentException($"Tổng tiền hóa đơn ({invoice.TotalAmount}) không khớp với tổng thành tiền chi tiết ({calculatedTotal})");

            // Tạo hóa đơn
            try
            {
                return await salesRepository.CreateInvoiceAsync(invoice, details);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tạo hóa đơn: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo hóa đơn: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin hóa đơn theo ID.
        /// </summary>
        /// <param name="invoiceId">ID hóa đơn.</param>
        /// <returns>Thông tin hóa đơn hoặc null nếu không tìm thấy.</returns>
        /// <exception cref="ArgumentException">ID không hợp lệ.</exception>
        public async Task<SalesInvoice> GetInvoiceByIdAsync(int invoiceId)
        {
            if (invoiceId <= 0)
                throw new ArgumentException("ID hóa đơn không hợp lệ", nameof(invoiceId));

            try
            {
                return await salesRepository.GetInvoiceByIdAsync(invoiceId);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy hóa đơn: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn theo ID hóa đơn.
        /// </summary>
        /// <param name="invoiceId">ID hóa đơn.</param>
        /// <returns>Danh sách chi tiết hóa đơn.</returns>
        /// <exception cref="ArgumentException">ID không hợp lệ.</exception>
        public async Task<List<SalesInvoiceDetail>> GetInvoiceDetailsAsync(int invoiceId)
        {
            if (invoiceId <= 0)
                throw new ArgumentException("ID hóa đơn không hợp lệ", nameof(invoiceId));

            try
            {
                return await salesRepository.GetInvoiceDetailsAsync(invoiceId);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy chi tiết hóa đơn: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả hóa đơn.
        /// </summary>
        /// <returns>Danh sách hóa đơn.</returns>
        public async Task<List<SalesInvoice>> GetAllInvoicesAsync()
        {
            try
            {
                return await salesRepository.GetAllInvoicesAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách hóa đơn: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách hóa đơn theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Danh sách hóa đơn.</returns>
        /// <exception cref="ArgumentException">Khoảng thời gian không hợp lệ.</exception>
        public async Task<List<SalesInvoice>> GetInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc", nameof(fromDate));

            try
            {
                return await salesRepository.GetInvoicesByDateRangeAsync(fromDate, toDate);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy hóa đơn theo khoảng thời gian: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra hóa đơn có tồn tại hay không.
        /// </summary>
        /// <param name="invoiceId">ID hóa đơn.</param>
        /// <returns>True nếu hóa đơn tồn tại, ngược lại False.</returns>
        public async Task<bool> InvoiceExistsAsync(int invoiceId)
        {
            if (invoiceId <= 0)
                return false;

            try
            {
                return await salesRepository.InvoiceExistsAsync(invoiceId);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tính doanh thu theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Tổng doanh thu.</returns>
        /// <exception cref="ArgumentException">Khoảng thời gian không hợp lệ.</exception>
        public async Task<decimal> CalculateRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc", nameof(fromDate));

            try
            {
                return await salesRepository.CalculateRevenueAsync(fromDate, toDate);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tính doanh thu: {ex.Message}", ex);
            }
        }

        #endregion

        #region Phương thức đồng bộ (để tương thích với code cũ)

        /// <summary>
        /// Tạo hóa đơn mới và các chi tiết liên quan (phương thức đồng bộ).
        /// </summary>
        /// <param name="invoice">Thông tin hóa đơn.</param>
        /// <param name="details">Danh sách chi tiết hóa đơn.</param>
        /// <returns>ID của hóa đơn vừa tạo.</returns>
        public int CreateInvoice(SalesInvoice invoice, List<SalesInvoiceDetail> details)
        {
            return CreateInvoiceAsync(invoice, details).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy thông tin hóa đơn theo ID (phương thức đồng bộ).
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>Thông tin hóa đơn hoặc null nếu không tìm thấy.</returns>
        public SalesInvoice GetInvoiceById(int invoiceID)
        {
            return GetInvoiceByIdAsync(invoiceID).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn theo ID hóa đơn (phương thức đồng bộ).
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>Danh sách chi tiết hóa đơn.</returns>
        public List<SalesInvoiceDetail> GetInvoiceDetails(int invoiceID)
        {
            return GetInvoiceDetailsAsync(invoiceID).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách tất cả hóa đơn (phương thức đồng bộ).
        /// </summary>
        /// <returns>Danh sách hóa đơn.</returns>
        public List<SalesInvoice> GetAllInvoices()
        {
            return GetAllInvoicesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách hóa đơn theo khoảng thời gian (phương thức đồng bộ).
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Danh sách hóa đơn.</returns>
        public List<SalesInvoice> GetInvoicesByDateRange(DateTime fromDate, DateTime toDate)
        {
            return GetInvoicesByDateRangeAsync(fromDate, toDate).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Kiểm tra hóa đơn có tồn tại hay không (phương thức đồng bộ).
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>True nếu hóa đơn tồn tại, ngược lại False.</returns>
        public bool InvoiceExists(int invoiceID)
        {
            return InvoiceExistsAsync(invoiceID).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Tính doanh thu theo khoảng thời gian (phương thức đồng bộ).
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Tổng doanh thu.</returns>
        public decimal CalculateRevenue(DateTime fromDate, DateTime toDate)
        {
            return CalculateRevenueAsync(fromDate, toDate).GetAwaiter().GetResult();
        }

        #endregion
    }
}