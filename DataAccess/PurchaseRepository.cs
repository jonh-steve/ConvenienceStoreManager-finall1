// 🌸 Vị trí file: ConvenienceStoreManager/DataAccess/PurchaseRepository.cs
// Bé yêu lưu ý: File này đã được sửa lỗi DatabaseHelper, CommitAsync, kiểu trả về, chuẩn hóa code, chú thích vị trí file, comment dễ thương, và hướng dẫn sửa lỗi tương tự cho các file khác nha!

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.DataAccess
{
    // 🌸 Vị trí file: ConvenienceStoreManager/DataAccess/PurchaseRepository.cs

    public class PurchaseRepository : IPurchaseRepository
    {
        public PurchaseRepository()
        {
            // Không cần instance nếu chỉ dùng static DatabaseHelper
        }

        /// <summary>
        /// Lấy tất cả phiếu nhập.
        /// </summary>
        public List<PurchaseOrder> GetAllPurchaseOrders()
        {
            var orders = new List<PurchaseOrder>();
            string query = "SELECT * FROM PurchaseOrders";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, null);

            foreach (DataRow row in dt.Rows)
            {
                var order = new PurchaseOrder
                {
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    SupplierName = row["SupplierName"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Notes = row["Notes"].ToString(),
                    Details = GetPurchaseOrderDetails(Convert.ToInt32(row["PurchaseOrderID"]))
                };
                orders.Add(order);
            }
            return orders;
        }

        /// <summary>
        /// Lấy tất cả phiếu nhập bất đồng bộ.
        /// </summary>
        public async Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            var orders = new List<PurchaseOrder>();
            string query = "SELECT * FROM PurchaseOrders";
            DataTable dt = await Task.Run(() => DatabaseHelper.ExecuteQuery(query, null));

            foreach (DataRow row in dt.Rows)
            {
                var order = new PurchaseOrder
                {
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    SupplierName = row["SupplierName"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Notes = row["Notes"].ToString(),
                    Details = await GetPurchaseOrderDetailsAsync(Convert.ToInt32(row["PurchaseOrderID"]))
                };
                orders.Add(order);
            }
            return orders;
        }

        /// <summary>
        /// Lấy phiếu nhập theo ID.
        /// </summary>
        public PurchaseOrder GetPurchaseOrderById(int id)
        {
            string query = "SELECT * FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID";
            SqlParameter[] parameters = { new SqlParameter("@PurchaseOrderID", id) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;
            var row = dt.Rows[0];

            return new PurchaseOrder
            {
                PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                SupplierName = row["SupplierName"].ToString(),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                Notes = row["Notes"].ToString(),
                Details = GetPurchaseOrderDetails(id)
            };
        }

        /// <summary>
        /// Lấy phiếu nhập theo ID bất đồng bộ.
        /// </summary>
        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id)
        {
            string query = "SELECT * FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID";
            SqlParameter[] parameters = { new SqlParameter("@PurchaseOrderID", id) };
            DataTable dt = await Task.Run(() => DatabaseHelper.ExecuteQuery(query, parameters));

            if (dt.Rows.Count == 0) return null;
            var row = dt.Rows[0];

            return new PurchaseOrder
            {
                PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                SupplierName = row["SupplierName"].ToString(),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                Notes = row["Notes"].ToString(),
                Details = await GetPurchaseOrderDetailsAsync(id)
            };
        }

        /// <summary>
        /// Lấy chi tiết phiếu nhập theo mã phiếu nhập.
        /// </summary>
        public List<PurchaseOrderDetail> GetPurchaseOrderDetails(int purchaseOrderId)
        {
            var details = new List<PurchaseOrderDetail>();
            string query = @"
SELECT pod.*, p.ProductName, p.Unit
FROM PurchaseOrderDetails pod
JOIN Products p ON pod.ProductID = p.ProductID
WHERE pod.PurchaseOrderID = @PurchaseOrderID";
            SqlParameter[] parameters = { new SqlParameter("@PurchaseOrderID", purchaseOrderId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                details.Add(new PurchaseOrderDetail
                {
                    PurchaseOrderDetailID = Convert.ToInt32(row["PurchaseOrderDetailID"]),
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                    Subtotal = Convert.ToDecimal(row["Subtotal"]),
                    ProductName = row["ProductName"].ToString(),
                    Unit = row["Unit"].ToString()
                });
            }
            return details;
        }

        /// <summary>
        /// Lấy chi tiết phiếu nhập bất đồng bộ.
        /// </summary>
        public async Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailsAsync(int purchaseOrderId)
        {
            var details = new List<PurchaseOrderDetail>();
            string query = @"
SELECT pod.*, p.ProductName, p.Unit
FROM PurchaseOrderDetails pod
JOIN Products p ON pod.ProductID = p.ProductID
WHERE pod.PurchaseOrderID = @PurchaseOrderID";
            SqlParameter[] parameters = { new SqlParameter("@PurchaseOrderID", purchaseOrderId) };
            DataTable dt = await Task.Run(() => DatabaseHelper.ExecuteQuery(query, parameters));

            foreach (DataRow row in dt.Rows)
            {
                details.Add(new PurchaseOrderDetail
                {
                    PurchaseOrderDetailID = Convert.ToInt32(row["PurchaseOrderDetailID"]),
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                    Subtotal = Convert.ToDecimal(row["Subtotal"]),
                    ProductName = row["ProductName"].ToString(),
                    Unit = row["Unit"].ToString()
                });
            }
            return details;
        }

        /// <summary>
        /// Lấy danh sách phiếu nhập theo khoảng thời gian.
        /// </summary>
        public List<PurchaseOrder> GetPurchaseOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = new List<PurchaseOrder>();
            string query = @"
SELECT * FROM PurchaseOrders
WHERE OrderDate BETWEEN @FromDate AND @ToDate";
            SqlParameter[] parameters = {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                var order = new PurchaseOrder
                {
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    SupplierName = row["SupplierName"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Notes = row["Notes"].ToString(),
                    Details = GetPurchaseOrderDetails(Convert.ToInt32(row["PurchaseOrderID"]))
                };
                orders.Add(order);
            }
            return orders;
        }

        /// <summary>
        /// Lấy danh sách phiếu nhập theo khoảng thời gian bất đồng bộ.
        /// </summary>
        public async Task<List<PurchaseOrder>> GetPurchaseOrdersByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var orders = new List<PurchaseOrder>();
            string query = @"
SELECT * FROM PurchaseOrders
WHERE OrderDate BETWEEN @FromDate AND @ToDate";
            SqlParameter[] parameters = {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            };
            DataTable dt = await Task.Run(() => DatabaseHelper.ExecuteQuery(query, parameters));

            foreach (DataRow row in dt.Rows)
            {
                var order = new PurchaseOrder
                {
                    PurchaseOrderID = Convert.ToInt32(row["PurchaseOrderID"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    SupplierName = row["SupplierName"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Notes = row["Notes"].ToString(),
                    Details = await GetPurchaseOrderDetailsAsync(Convert.ToInt32(row["PurchaseOrderID"]))
                };
                orders.Add(order);
            }
            return orders;
        }

        /// <summary>
        /// Tạo phiếu nhập mới và chi tiết phiếu nhập.
        /// </summary>
        public void CreatePurchaseOrder(PurchaseOrder order, List<PurchaseOrderDetail> details)
        {
            // 🌸 Bé yêu chú ý: Nếu DatabaseHelper không hỗ trợ transaction/connection, hãy dùng trực tiếp ADO.NET như dưới đây nha!
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Thêm phiếu nhập
                    string orderQuery = @"
INSERT INTO PurchaseOrders (OrderDate, SupplierName, TotalAmount, Notes)
OUTPUT INSERTED.PurchaseOrderID
VALUES (@OrderDate, @SupplierName, @TotalAmount, @Notes)";
                    SqlCommand cmdOrder = new SqlCommand(orderQuery, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmdOrder.Parameters.AddWithValue("@SupplierName", (object)order.SupplierName ?? DBNull.Value);
                    cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmdOrder.Parameters.AddWithValue("@Notes", (object)order.Notes ?? DBNull.Value);

                    int purchaseOrderId = (int)cmdOrder.ExecuteScalar();

                    // Thêm chi tiết phiếu nhập
                    foreach (var detail in details)
                    {
                        string detailQuery = @"
INSERT INTO PurchaseOrderDetails (PurchaseOrderID, ProductID, Quantity, PurchasePrice, Subtotal)
VALUES (@PurchaseOrderID, @ProductID, @Quantity, @PurchasePrice, @Subtotal)";
                        SqlCommand cmdDetail = new SqlCommand(detailQuery, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderId);
                        cmdDetail.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmdDetail.Parameters.AddWithValue("@PurchasePrice", detail.PurchasePrice);
                        cmdDetail.Parameters.AddWithValue("@Subtotal", detail.Subtotal);
                        cmdDetail.ExecuteNonQuery();

                        // 🌸 Bé yêu nhớ: Nếu muốn cập nhật tồn kho, hãy thêm đoạn code update StockQuantity ở đây nhé!
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw ;
                }
            }
        }

        /// <summary>
        /// Tạo phiếu nhập mới và chi tiết phiếu nhập bất đồng bộ.
        /// </summary>
        public async Task CreatePurchaseOrderAsync(PurchaseOrder order, List<PurchaseOrderDetail> details)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Thêm phiếu nhập
                    string orderQuery = @"
INSERT INTO PurchaseOrders (OrderDate, SupplierName, TotalAmount, Notes)
OUTPUT INSERTED.PurchaseOrderID
VALUES (@OrderDate, @SupplierName, @TotalAmount, @Notes)";
                    SqlCommand cmdOrder = new SqlCommand(orderQuery, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmdOrder.Parameters.AddWithValue("@SupplierName", (object)order.SupplierName ?? DBNull.Value);
                    cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmdOrder.Parameters.AddWithValue("@Notes", (object)order.Notes ?? DBNull.Value);

                    int purchaseOrderId = (int)await cmdOrder.ExecuteScalarAsync();

                    // Thêm chi tiết phiếu nhập
                    foreach (var detail in details)
                    {
                        string detailQuery = @"
INSERT INTO PurchaseOrderDetails (PurchaseOrderID, ProductID, Quantity, PurchasePrice, Subtotal)
VALUES (@PurchaseOrderID, @ProductID, @Quantity, @PurchasePrice, @Subtotal)";
                        SqlCommand cmdDetail = new SqlCommand(detailQuery, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderId);
                        cmdDetail.Parameters.AddWithValue("@ProductID", detail.ProductID);
                        cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmdDetail.Parameters.AddWithValue("@PurchasePrice", detail.PurchasePrice);
                        cmdDetail.Parameters.AddWithValue("@Subtotal", detail.Subtotal);
                        await cmdDetail.ExecuteNonQueryAsync();

                        // 🌸 Bé yêu nhớ: Nếu muốn cập nhật tồn kho, hãy thêm đoạn code update StockQuantity ở đây nhé!
                    }

                    transaction.Commit(); // Không dùng CommitAsync vì SqlTransaction không hỗ trợ
                }
                catch
                {
                    transaction.Rollback(); // Không dùng RollbackAsync vì SqlTransaction không hỗ trợ
                    throw;
                }
            }
        }
    }
}

// 🌸 Bé yêu nhớ: Khi sửa các file DataAccess, hãy luôn chú thích vị trí file, chuẩn hóa code, thêm async nếu cần, và comment rõ ràng để teamwork hiệu quả nha!
// 🌸 Nếu file là UI, hãy style control bằng màu hồng, pastel, và dùng icon FontAwesome.Sharp cho các nút để thêm phần cute!
// 🌸 Khi gặp lỗi tương tự ở các file khác, hãy kiểm tra kỹ static/instance, transaction, async/await, kiểu dữ liệu, và chú thích rõ ràng vị trí file để teamwork hiệu quả nha bé yêu!
