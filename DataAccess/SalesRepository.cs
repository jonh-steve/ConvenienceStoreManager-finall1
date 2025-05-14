using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.DataAccess.Interfaces;

namespace ConvenienceStoreManager.DataAccess
{
    /// <summary>
    /// Repository xử lý dữ liệu hóa đơn bán hàng.
    /// </summary>
    public class SalesRepository : ISalesRepository
    {
        #region Phương thức bất đồng bộ

        /// <summary>
        /// Tạo hóa đơn bán hàng và chi tiết hóa đơn.
        /// </summary>
        /// <param name="invoice">Thông tin hóa đơn.</param>
        /// <param name="details">Danh sách chi tiết hóa đơn.</param>
        /// <returns>ID của hóa đơn vừa tạo.</returns>
        /// <exception cref="ArgumentNullException">Hóa đơn hoặc chi tiết null.</exception>
        /// <exception cref="Exception">Lỗi khi kiểm tra tồn kho hoặc tạo hóa đơn.</exception>
        public async Task<int> CreateInvoiceAsync(SalesInvoice invoice, List<SalesInvoiceDetail> details)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice), "Hóa đơn không được phép null");
            if (details == null || !details.Any())
                throw new ArgumentNullException(nameof(details), "Chi tiết hóa đơn không được phép rỗng");

            int invoiceId = 0;
            SqlConnection conn = null;
            SqlTransaction transaction = null;

            try
            {
                conn = DatabaseHelper.GetConnection();
                await conn.OpenAsync();
                transaction = conn.BeginTransaction();

                // Kiểm tra tồn kho cho tất cả sản phẩm
                var productIds = details.Select(d => d.ProductID).Distinct().ToList();
                var stockQuery = @"
                    SELECT ProductID, StockQuantity
                    FROM Products
                    WHERE ProductID IN (" + string.Join(",", productIds) + ")";

                var stockDict = new Dictionary<int, int>();

                using (SqlCommand cmdStock = new SqlCommand(stockQuery, conn, transaction))
                {
                    using (SqlDataReader reader = await cmdStock.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stockDict[reader.GetInt32(0)] = reader.GetInt32(1);
                        }
                    }
                }

                foreach (var detail in details)
                {
                    int stock;
                    if (!stockDict.TryGetValue(detail.ProductID, out stock) || stock < detail.Quantity)
                    {
                        throw new Exception($"Sản phẩm ID {detail.ProductID} không đủ tồn kho!");
                    }
                }

                // Tạo hóa đơn
                const string insertInvoiceSql = @"
                    INSERT INTO SalesInvoices (InvoiceDate, TotalAmount, Notes)
                    OUTPUT INSERTED.InvoiceID
                    VALUES (@InvoiceDate, @TotalAmount, @Notes);";

                using (SqlCommand cmdInsertInvoice = new SqlCommand(insertInvoiceSql, conn, transaction))
                {
                    cmdInsertInvoice.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                    cmdInsertInvoice.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
                    cmdInsertInvoice.Parameters.AddWithValue("@Notes", (object)invoice.Notes ?? DBNull.Value);

                    object result = await cmdInsertInvoice.ExecuteScalarAsync();
                    invoiceId = Convert.ToInt32(result);
                }

                // Tạo chi tiết hóa đơn và cập nhật tồn kho
                foreach (var detail in details)
                {
                    // Tạo chi tiết hóa đơn
                    const string insertDetailSql = @"
                        INSERT INTO SalesInvoiceDetails (InvoiceID, ProductID, Quantity, UnitPrice, Subtotal)
                        VALUES (@InvoiceID, @ProductID, @Quantity, @UnitPrice, @Subtotal)";

                    using (SqlCommand cmdInsertDetail = new SqlCommand(insertDetailSql, conn, transaction))
                    {
                        cmdInsertDetail.Parameters.AddWithValue("@InvoiceID", invoiceId);
                        cmdInsertDetail.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        cmdInsertDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmdInsertDetail.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                        cmdInsertDetail.Parameters.AddWithValue("@Subtotal", detail.Subtotal);

                        await cmdInsertDetail.ExecuteNonQueryAsync();
                    }

                    // Cập nhật tồn kho
                    const string updateStockSql = @"
                        UPDATE Products
                        SET StockQuantity = StockQuantity - @Quantity
                        WHERE ProductID = @ProductID";

                    using (SqlCommand cmdUpdateStock = new SqlCommand(updateStockSql, conn, transaction))
                    {
                        cmdUpdateStock.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        cmdUpdateStock.Parameters.AddWithValue("@Quantity", detail.Quantity);

                        await cmdUpdateStock.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();
                return invoiceId;
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw new Exception($"Lỗi cơ sở dữ liệu khi tạo hóa đơn: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw new Exception($"Lỗi khi tạo hóa đơn: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
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

            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT InvoiceID, InvoiceDate, TotalAmount, Notes
                    FROM SalesInvoices
                    WHERE InvoiceID = @InvoiceID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new SalesInvoice
                            {
                                InvoiceID = reader.GetInt32(0),
                                InvoiceDate = reader.GetDateTime(1),
                                TotalAmount = reader.GetDecimal(2),
                                Notes = reader.IsDBNull(3) ? null : reader.GetString(3)
                            };
                        }
                    }
                }
                return null;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
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

            var details = new List<SalesInvoiceDetail>();
            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT d.InvoiceDetailID, d.InvoiceID, d.ProductID, d.Quantity, d.UnitPrice, d.Subtotal,
                           p.ProductName, p.Unit
                    FROM SalesInvoiceDetails d
                    INNER JOIN Products p ON d.ProductID = p.ProductID
                    WHERE d.InvoiceID = @InvoiceID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            details.Add(new SalesInvoiceDetail
                            {
                                InvoiceDetailID = reader.GetInt32(0),
                                InvoiceID = reader.GetInt32(1),
                                ProductID = reader.GetInt32(2),
                                Quantity = reader.GetInt32(3),
                                UnitPrice = reader.GetDecimal(4),
                                Subtotal = reader.GetDecimal(5),
                                ProductName = reader.GetString(6),
                                Unit = reader.GetString(7)
                            });
                        }
                    }
                }
                return details;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả hóa đơn.
        /// </summary>
        /// <returns>Danh sách hóa đơn.</returns>
        public async Task<List<SalesInvoice>> GetAllInvoicesAsync()
        {
            var invoices = new List<SalesInvoice>();
            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT InvoiceID, InvoiceDate, TotalAmount, Notes
                    FROM SalesInvoices
                    ORDER BY InvoiceDate DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            invoices.Add(new SalesInvoice
                            {
                                InvoiceID = reader.GetInt32(0),
                                InvoiceDate = reader.GetDateTime(1),
                                TotalAmount = reader.GetDecimal(2),
                                Notes = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
                return invoices;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Lấy danh sách hóa đơn theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Danh sách hóa đơn.</returns>
        public async Task<List<SalesInvoice>> GetInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var invoices = new List<SalesInvoice>();
            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT InvoiceID, InvoiceDate, TotalAmount, Notes
                    FROM SalesInvoices
                    WHERE CAST(InvoiceDate AS DATE) BETWEEN @FromDate AND @ToDate
                    ORDER BY InvoiceDate DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            invoices.Add(new SalesInvoice
                            {
                                InvoiceID = reader.GetInt32(0),
                                InvoiceDate = reader.GetDateTime(1),
                                TotalAmount = reader.GetDecimal(2),
                                Notes = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
                return invoices;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Kiểm tra hóa đơn có tồn tại không.
        /// </summary>
        /// <param name="invoiceId">ID hóa đơn.</param>
        /// <returns>True nếu tồn tại, False nếu không tồn tại.</returns>
        public async Task<bool> InvoiceExistsAsync(int invoiceId)
        {
            if (invoiceId <= 0)
                return false;

            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT COUNT(1)
                    FROM SalesInvoices
                    WHERE InvoiceID = @InvoiceID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                    await conn.OpenAsync();
                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    return count > 0;
                }
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Tính doanh thu theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Tổng doanh thu.</returns>
        public async Task<decimal> CalculateRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            SqlConnection conn = null;
            try
            {
                conn = DatabaseHelper.GetConnection();
                const string sql = @"
                    SELECT SUM(TotalAmount)
                    FROM SalesInvoices
                    WHERE CAST(InvoiceDate AS DATE) BETWEEN @FromDate AND @ToDate";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    await conn.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToDecimal(result);
                    return 0;
                }
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        #endregion

        #region Phương thức đồng bộ (để tương thích với code cũ)

        /// <summary>
        /// Tạo hóa đơn bán hàng và chi tiết hóa đơn.
        /// </summary>
        /// <param name="invoice">Thông tin hóa đơn.</param>
        /// <param name="details">Danh sách chi tiết hóa đơn.</param>
        /// <returns>ID của hóa đơn vừa tạo.</returns>
        public int CreateInvoice(SalesInvoice invoice, List<SalesInvoiceDetail> details)
        {
            return CreateInvoiceAsync(invoice, details).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy thông tin hóa đơn theo ID.
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>Thông tin hóa đơn hoặc null nếu không tìm thấy.</returns>
        public SalesInvoice GetInvoiceById(int invoiceID)
        {
            return GetInvoiceByIdAsync(invoiceID).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn theo ID hóa đơn.
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>Danh sách chi tiết hóa đơn.</returns>
        public List<SalesInvoiceDetail> GetInvoiceDetails(int invoiceID)
        {
            return GetInvoiceDetailsAsync(invoiceID).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách tất cả hóa đơn.
        /// </summary>
        /// <returns>Danh sách hóa đơn.</returns>
        public List<SalesInvoice> GetAllInvoices()
        {
            return GetAllInvoicesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lấy danh sách hóa đơn theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Danh sách hóa đơn.</returns>
        public List<SalesInvoice> GetInvoicesByDateRange(DateTime fromDate, DateTime toDate)
        {
            return GetInvoicesByDateRangeAsync(fromDate, toDate).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Kiểm tra hóa đơn có tồn tại không.
        /// </summary>
        /// <param name="invoiceID">ID hóa đơn.</param>
        /// <returns>True nếu tồn tại, False nếu không tồn tại.</returns>
        public bool InvoiceExists(int invoiceID)
        {
            return InvoiceExistsAsync(invoiceID).GetAwaiter().GetResult();
        }

        #endregion
    }
}