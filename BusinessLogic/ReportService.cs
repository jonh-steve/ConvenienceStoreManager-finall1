
﻿// 🌸 Vị trí file: BusinessLogic/ReportService.cs
// 🌸 Bé yêu lưu ý: Đây là file xử lý logic báo cáo, đã được chuẩn hóa, chú thích vị trí file, và hướng dẫn sửa lỗi tương tự cho các file khác nha!

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.DataAccess.Interfaces;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.BusinessLogic
{
    // 🌸 Vị trí file: BusinessLogic/ReportService.cs
    // 🌸 Bé yêu nhớ: Khi sửa hoặc tạo file, luôn chú thích vị trí file ở đầu class để teamwork dễ dàng hơn nha!

    /// <summary>
    /// Lớp xử lý logic nghiệp vụ liên quan đến báo cáo thống kê.
    /// Bé yêu nhớ: Nếu có UI thì style thật hồng hào, dễ thương, dùng FontAwesome.Sharp cho icon nhé!
    /// </summary>
    public class ReportService
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IProductRepository _productRepository;
        private readonly SalesService _salesService;

        /// <summary>
        /// Constructor với Dependency Injection.
        /// </summary>
        /// <param name="salesRepository">Repository xử lý dữ liệu hóa đơn bán hàng.</param>
        /// <param name="productRepository">Repository xử lý dữ liệu sản phẩm.</param>
        /// <param name="salesService">Service xử lý logic nghiệp vụ bán hàng.</param>
        public ReportService(ISalesRepository salesRepository, IProductRepository productRepository, SalesService salesService)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _salesService = salesService ?? throw new ArgumentNullException(nameof(salesService));
        }

        /// <summary>
        /// Constructor không tham số, sử dụng cho code cũ.
        /// </summary>
        public ReportService()
        {
            _salesRepository = new SalesRepository();
            _productRepository = new ProductRepository();
            _salesService = new SalesService(_salesRepository, _productRepository);
        }

        #region Phương thức đồng bộ (cho code cũ)

        /// <summary>
        /// Tính tổng doanh thu trong khoảng thời gian.
        /// </summary>
        public decimal CalculateRevenue(DateTime fromDate, DateTime toDate)
        {
            try
            {
                string query = @"
                    SELECT SUM(TotalAmount)
                    FROM SalesInvoices
                    WHERE InvoiceDate BETWEEN @FromDate AND @ToDate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính doanh thu: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính tổng chi phí nhập hàng trong khoảng thời gian.
        /// </summary>
        public decimal CalculatePurchaseCost(DateTime fromDate, DateTime toDate)
        {
            try
            {
                string query = @"
                    SELECT SUM(TotalAmount)
                    FROM PurchaseOrders
                    WHERE OrderDate BETWEEN @FromDate AND @ToDate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính chi phí nhập hàng: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính lợi nhuận trong khoảng thời gian.
        /// </summary>
        public decimal CalculateProfit(DateTime fromDate, DateTime toDate)
        {
            try
            {
                decimal revenue = CalculateRevenue(fromDate, toDate);
                decimal cost = CalculatePurchaseCost(fromDate, toDate);
                return revenue - cost;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính lợi nhuận: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu doanh thu hàng ngày.
        /// </summary>
        public List<DailyRevenueReport> GetDailyRevenue(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<DailyRevenueReport> reports = new List<DailyRevenueReport>();
                string query = @"
                    SELECT CAST(InvoiceDate AS DATE) AS RevenueDate, SUM(TotalAmount) AS DailyRevenue
                    FROM SalesInvoices
                    WHERE InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY CAST(InvoiceDate AS DATE)
                    ORDER BY RevenueDate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    reports.Add(new DailyRevenueReport
                    {
                        RevenueDate = Convert.ToDateTime(row["RevenueDate"]),
                        DailyRevenue = Convert.ToDecimal(row["DailyRevenue"])
                    });
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu doanh thu hàng ngày: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm bán chạy theo số lượng.
        /// </summary>
        public List<TopProductReport> GetTopSellingProducts(DateTime fromDate, DateTime toDate, int topN)
        {
            try
            {
                List<TopProductReport> reports = new List<TopProductReport>();
                string query = @"
                    SELECT TOP (@TopN) p.ProductName, SUM(d.Quantity) AS QuantitySold
                    FROM SalesInvoiceDetails d
                    INNER JOIN SalesInvoices i ON d.InvoiceID = i.InvoiceID
                    INNER JOIN Products p ON d.ProductID = p.ProductID
                    WHERE i.InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY p.ProductName
                    ORDER BY QuantitySold DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TopN", topN),
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    reports.Add(new TopProductReport
                    {
                        ProductName = row["ProductName"].ToString(),
                        QuantitySold = Convert.ToInt32(row["QuantitySold"]),
                        Revenue = 0 // Không có thông tin doanh thu trong truy vấn này
                    });
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm bán chạy: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm có doanh thu cao nhất.
        /// </summary>
        public List<TopProductReport> GetTopRevenueProducts(DateTime fromDate, DateTime toDate, int topN)
        {
            try
            {
                List<TopProductReport> reports = new List<TopProductReport>();
                string query = @"
                    SELECT TOP (@TopN) p.ProductName, SUM(d.Subtotal) AS Revenue
                    FROM SalesInvoiceDetails d
                    INNER JOIN SalesInvoices i ON d.InvoiceID = i.InvoiceID
                    INNER JOIN Products p ON d.ProductID = p.ProductID
                    WHERE i.InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY p.ProductName
                    ORDER BY Revenue DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TopN", topN),
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);
                foreach (DataRow row in dataTable.Rows)
                {
                    reports.Add(new TopProductReport
                    {
                        ProductName = row["ProductName"].ToString(),
                        Revenue = Convert.ToDecimal(row["Revenue"]),
                        QuantitySold = 0 // Không có thông tin số lượng trong truy vấn này
                    });
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm có doanh thu cao: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính giá trị tồn kho hiện tại.
        /// </summary>
        public decimal GetInventoryValue()
        {
            try
            {
                string query = @"
                    SELECT SUM(StockQuantity * SellingPrice)
                    FROM Products";
                object result = DatabaseHelper.ExecuteScalar(query, null);
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá trị tồn kho: " + ex.Message);
            }
        }

        #endregion

        #region Phương thức bất đồng bộ

        /// <summary>
        /// Tính tổng doanh thu trong khoảng thời gian (bất đồng bộ).
        /// </summary>
        public async Task<decimal> CalculateRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _salesService.CalculateRevenueAsync(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính doanh thu: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính tổng chi phí nhập hàng trong khoảng thời gian (bất đồng bộ).
        /// </summary>
        public async Task<decimal> CalculatePurchaseCostAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                // Ở đây tạm thời triển khai trực tiếp, sau này có thể thay bằng PurchaseService
                string query = @"
                    SELECT SUM(TotalAmount)
                    FROM PurchaseOrders
                    WHERE OrderDate BETWEEN @FromDate AND @ToDate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        object result = await cmd.ExecuteScalarAsync();
                        return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính chi phí nhập hàng: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính lợi nhuận trong khoảng thời gian (bất đồng bộ).
        /// </summary>
        public async Task<decimal> CalculateProfitAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                decimal revenue = await CalculateRevenueAsync(fromDate, toDate);
                decimal cost = await CalculatePurchaseCostAsync(fromDate, toDate);
                return revenue - cost;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính lợi nhuận: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu doanh thu hàng ngày (bất đồng bộ).
        /// </summary>
        public async Task<List<DailyRevenueReport>> GetDailyRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<DailyRevenueReport> reports = new List<DailyRevenueReport>();
                string query = @"
                    SELECT CAST(InvoiceDate AS DATE) AS RevenueDate, SUM(TotalAmount) AS DailyRevenue
                    FROM SalesInvoices
                    WHERE InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY CAST(InvoiceDate AS DATE)
                    ORDER BY RevenueDate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reports.Add(new DailyRevenueReport
                                {
                                    RevenueDate = reader.GetDateTime(0),
                                    DailyRevenue = reader.GetDecimal(1)
                                });
                            }
                        }
                    }
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu doanh thu hàng ngày: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm bán chạy theo số lượng (bất đồng bộ).
        /// </summary>
        public async Task<List<TopProductReport>> GetTopSellingProductsAsync(DateTime fromDate, DateTime toDate, int topN)
        {
            try
            {
                List<TopProductReport> reports = new List<TopProductReport>();
                string query = @"
                    SELECT TOP (@TopN) p.ProductName, SUM(d.Quantity) AS QuantitySold
                    FROM SalesInvoiceDetails d
                    INNER JOIN SalesInvoices i ON d.InvoiceID = i.InvoiceID
                    INNER JOIN Products p ON d.ProductID = p.ProductID
                    WHERE i.InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY p.ProductName
                    ORDER BY QuantitySold DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TopN", topN),
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reports.Add(new TopProductReport
                                {
                                    ProductName = reader.GetString(0),
                                    QuantitySold = reader.GetInt32(1),
                                    Revenue = 0 // Không có thông tin doanh thu trong truy vấn này
                                });
                            }
                        }
                    }
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm bán chạy: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm có doanh thu cao nhất (bất đồng bộ).
        /// </summary>
        public async Task<List<TopProductReport>> GetTopRevenueProductsAsync(DateTime fromDate, DateTime toDate, int topN)
        {
            try
            {
                List<TopProductReport> reports = new List<TopProductReport>();
                string query = @"
                    SELECT TOP (@TopN) p.ProductName, SUM(d.Subtotal) AS Revenue
                    FROM SalesInvoiceDetails d
                    INNER JOIN SalesInvoices i ON d.InvoiceID = i.InvoiceID
                    INNER JOIN Products p ON d.ProductID = p.ProductID
                    WHERE i.InvoiceDate BETWEEN @FromDate AND @ToDate
                    GROUP BY p.ProductName
                    ORDER BY Revenue DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TopN", topN),
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate)
                };

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reports.Add(new TopProductReport
                                {
                                    ProductName = reader.GetString(0),
                                    Revenue = reader.GetDecimal(1),
                                    QuantitySold = 0 // Không có thông tin số lượng trong truy vấn này
                                });
                            }
                        }
                    }
                }

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm có doanh thu cao: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính giá trị tồn kho hiện tại (bất đồng bộ).
        /// </summary>
        public async Task<decimal> GetInventoryValueAsync()
        {
            try
            {
                string query = @"
                    SELECT SUM(StockQuantity * SellingPrice)
                    FROM Products";

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = await cmd.ExecuteScalarAsync();
                        return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá trị tồn kho: " + ex.Message);
            }
        }

        #endregion
    }

    /// <summary>
    /// Lớp báo cáo doanh thu hàng ngày.
    /// </summary>
    public class DailyRevenueReport
    {
        public DateTime RevenueDate { get; set; }
        public decimal DailyRevenue { get; set; }
    }

    /// <summary>
    /// Lớp báo cáo sản phẩm bán chạy hoặc có doanh thu cao.
    /// </summary>
    public class TopProductReport
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}

// 🌸 Bé yêu nhớ: Khi sửa các file business logic, hãy luôn chú thích vị trí file, chuẩn hóa code, và nếu có UI thì style thật hồng hào, dễ thương nhé!
// 🌸 Nếu file có DataGridView hoặc các control UI, hãy dùng màu pastel, màu hồng, và icon FontAwesome.Sharp cho các nút để thêm phần cute!
// 🌸 Khi gặp lỗi tương tự ở các file khác, hãy kiểm tra kỹ constructor, dependency injection, try-catch, và chú thích rõ ràng vị trí file để teamwork hiệu quả nha bé yêu!
