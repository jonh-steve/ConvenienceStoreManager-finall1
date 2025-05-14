using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.DataAccess.Interfaces;

namespace ConvenienceStoreManager.DataAccess
{
    /// <summary>
    /// Lớp thao tác với bảng Products trong CSDL
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        #region Phương thức bất đồng bộ


        /// <summary>
        /// Lấy tất cả sản phẩm từ CSDL (bất đồng bộ)
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM Products ORDER BY ProductName";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Product product = new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode")),
                                    Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ? null : reader.GetString(reader.GetOrdinal("Unit")),
                                    SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                                };

                                products.Add(product);
                            }
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID (bất đồng bộ)
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Đối tượng Product hoặc null nếu không tìm thấy</returns>
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            string query = "SELECT * FROM Products WHERE ProductID = @ProductID";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode")),
                                    Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ? null : reader.GetString(reader.GetOrdinal("Unit")),
                                    SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                                };
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo mã sản phẩm (không phải ID) (bất đồng bộ)
        /// </summary>
        /// <param name="productCode">Mã sản phẩm</param>
        /// <returns>Đối tượng Product hoặc null nếu không tìm thấy</returns>
        public async Task<Product> GetProductByCodeAsync(string productCode)
        {
            string query = "SELECT * FROM Products WHERE ProductCode = @ProductCode";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductCode", productCode);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode")),
                                    Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ? null : reader.GetString(reader.GetOrdinal("Unit")),
                                    SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                                };
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm theo mã: " + ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo từ khóa (bất đồng bộ)
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sản phẩm phù hợp</returns>
        public async Task<List<Product>> SearchProductsAsync(string keyword)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM Products WHERE ProductName LIKE @Keyword OR ProductCode LIKE @Keyword ORDER BY ProductName";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Product product = new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode")),
                                    Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ? null : reader.GetString(reader.GetOrdinal("Unit")),
                                    SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                                };

                                products.Add(product);
                            }
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo danh sách ID (bất đồng bộ)
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm</param>
        /// <returns>Danh sách sản phẩm tìm thấy</returns>
        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new List<Product>();

            List<Product> products = new List<Product>();

            // Tạo danh sách tham số SQL cho IN clause
            string parameters = string.Join(",", productIds.Select((id, index) => $"@p{index}"));
            string query = $"SELECT * FROM Products WHERE ProductID IN ({parameters}) ORDER BY ProductName";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số
                        for (int i = 0; i < productIds.Count; i++)
                        {
                            command.Parameters.AddWithValue($"@p{i}", productIds[i]);
                        }

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Product product = new Product
                                {
                                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode")),
                                    Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ? null : reader.GetString(reader.GetOrdinal("Unit")),
                                    SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                                };

                                products.Add(product);
                            }
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm theo IDs: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm sản phẩm mới vào CSDL (bất đồng bộ)
        /// </summary>
        /// <param name="product">Thông tin sản phẩm</param>
        /// <returns>ID của sản phẩm mới thêm</returns>
        public async Task<int> AddProductAsync(Product product)
        {
            string query = @"INSERT INTO Products (ProductName, ProductCode, Unit, SellingPrice, StockQuantity) 
                             VALUES (@ProductName, @ProductCode, @Unit, @SellingPrice, @StockQuantity);
                             SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@ProductCode", product.ProductCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Unit", product.Unit ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

                        object result = await command.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }

                        throw new Exception("Không thể thêm sản phẩm!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm (bất đồng bộ)
        /// </summary>
        /// <param name="product">Thông tin sản phẩm đã cập nhật</param>
        /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy sản phẩm</returns>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            string query = @"UPDATE Products 
                             SET ProductName = @ProductName, 
                                 ProductCode = @ProductCode, 
                                 Unit = @Unit, 
                                 SellingPrice = @SellingPrice, 
                                 StockQuantity = @StockQuantity 
                             WHERE ProductID = @ProductID";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@ProductCode", product.ProductCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Unit", product.Unit ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa sản phẩm theo ID (bất đồng bộ)
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy sản phẩm</returns>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            string checkQuery = @"SELECT COUNT(*) FROM SalesInvoiceDetails WHERE ProductID = @ProductID
                                 UNION ALL
                                 SELECT COUNT(*) FROM PurchaseOrderDetails WHERE ProductID = @ProductID";

            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem sản phẩm đã được sử dụng trong giao dịch chưa
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ProductID", productId);
                        using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                        {
                            int salesCount = 0;
                            int purchaseCount = 0;

                            if (await reader.ReadAsync())
                                salesCount = reader.GetInt32(0);

                            if (await reader.ReadAsync())
                                purchaseCount = reader.GetInt32(0);

                            if (salesCount > 0 || purchaseCount > 0)
                                throw new Exception("Sản phẩm đã được sử dụng trong giao dịch, không thể xóa!");
                        }
                    }

                    // Thực hiện xóa sản phẩm
                    string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho của sản phẩm (bất đồng bộ)
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <param name="quantityChange">Lượng thay đổi (dương nếu nhập hàng, âm nếu bán hàng)</param>
        /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy sản phẩm</returns>
        public async Task<bool> UpdateStockAsync(int productId, int quantityChange)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    await connection.OpenAsync();

                    // Kiểm tra tồn kho nếu giảm
                    if (quantityChange < 0)
                    {
                        string checkQuery = "SELECT StockQuantity FROM Products WHERE ProductID = @ProductID";
                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@ProductID", productId);
                            object currentStock = await checkCommand.ExecuteScalarAsync();

                            if (currentStock == null || Convert.ToInt32(currentStock) < Math.Abs(quantityChange))
                                throw new Exception("Số lượng tồn kho không đủ!");
                        }
                    }

                    // Cập nhật tồn kho
                    string query = @"UPDATE Products 
                                     SET StockQuantity = StockQuantity + @QuantityChange 
                                     WHERE ProductID = @ProductID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.Parameters.AddWithValue("@QuantityChange", quantityChange);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tồn kho: " + ex.Message);
            }
        }

        #endregion

        #region Phương thức đồng bộ (giữ lại để tương thích với code cũ)

        /// <summary>
        /// Lấy tất cả sản phẩm từ CSDL
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM Products ORDER BY ProductName";

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    Product product = new Product
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        ProductName = row["ProductName"].ToString(),
                        ProductCode = row["ProductCode"].ToString(),
                        Unit = row["Unit"].ToString(),
                        SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                        StockQuantity = Convert.ToInt32(row["StockQuantity"])
                    };

                    products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Đối tượng Product hoặc null nếu không tìm thấy</returns>
        public Product GetProductById(int productId)
        {
            string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId)
            };

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new Product
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        ProductName = row["ProductName"].ToString(),
                        ProductCode = row["ProductCode"].ToString(),
                        Unit = row["Unit"].ToString(),
                        SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                        StockQuantity = Convert.ToInt32(row["StockQuantity"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo mã sản phẩm (không phải ID)
        /// </summary>
        /// <param name="productCode">Mã sản phẩm</param>
        /// <returns>Đối tượng Product hoặc null nếu không tìm thấy</returns>
        public Product GetProductByCode(string productCode)
        {
            string query = "SELECT * FROM Products WHERE ProductCode = @ProductCode";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductCode", productCode)
            };

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    return new Product
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        ProductName = row["ProductName"].ToString(),
                        ProductCode = row["ProductCode"].ToString(),
                        Unit = row["Unit"].ToString(),
                        SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                        StockQuantity = Convert.ToInt32(row["StockQuantity"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm theo mã: " + ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo từ khóa
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sản phẩm phù hợp</returns>
        public List<Product> SearchProducts(string keyword)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM Products WHERE ProductName LIKE @Keyword OR ProductCode LIKE @Keyword ORDER BY ProductName";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            try
            {
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                foreach (DataRow row in dataTable.Rows)
                {
                    Product product = new Product
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        ProductName = row["ProductName"].ToString(),
                        ProductCode = row["ProductCode"].ToString(),
                        Unit = row["Unit"].ToString(),
                        SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                        StockQuantity = Convert.ToInt32(row["StockQuantity"])
                    };

                    products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm sản phẩm mới vào CSDL
        /// </summary>
        /// <param name="product">Thông tin sản phẩm</param>
        /// <returns>ID của sản phẩm mới thêm</returns>
        public int AddProduct(Product product)
        {
            string query = @"INSERT INTO Products (ProductName, ProductCode, Unit, SellingPrice, StockQuantity) 
                             VALUES (@ProductName, @ProductCode, @Unit, @SellingPrice, @StockQuantity);
                             SELECT SCOPE_IDENTITY()";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@ProductCode", product.ProductCode ?? (object)DBNull.Value),
                new SqlParameter("@Unit", product.Unit ?? (object)DBNull.Value),
                new SqlParameter("@SellingPrice", product.SellingPrice),
                new SqlParameter("@StockQuantity", product.StockQuantity)
            };

            try
            {
                object result = DatabaseHelper.ExecuteScalar(query, parameters);

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }

                throw new Exception("Không thể thêm sản phẩm!");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="product">Thông tin sản phẩm đã cập nhật</param>
        /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy sản phẩm</returns>
        public bool UpdateProduct(Product product)
        {
            string query = @"UPDATE Products 
                             SET ProductName = @ProductName, 
                                 ProductCode = @ProductCode, 
                                 Unit = @Unit, 
                                 SellingPrice = @SellingPrice, 
                                 StockQuantity = @StockQuantity 
                             WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", product.ProductID),
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@ProductCode", product.ProductCode ?? (object)DBNull.Value),
                new SqlParameter("@Unit", product.Unit ?? (object)DBNull.Value),
                new SqlParameter("@SellingPrice", product.SellingPrice),
                new SqlParameter("@StockQuantity", product.StockQuantity)
            };

            try
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa sản phẩm theo ID
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy sản phẩm</returns>
        public bool DeleteProduct(int productId)
        {
            string checkQuery = @"SELECT COUNT(*) FROM SalesInvoiceDetails WHERE ProductID = @ProductID
                                 UNION ALL
                                 SELECT COUNT(*) FROM PurchaseOrderDetails WHERE ProductID = @ProductID";

            SqlParameter[] checkParameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId)
            };

            try
            {
                DataTable resultTable = DatabaseHelper.ExecuteQuery(checkQuery, checkParameters);

                if (resultTable.Rows.Count > 0)
                {
                    int salesCount = Convert.ToInt32(resultTable.Rows[0][0]);
                    int purchaseCount = Convert.ToInt32(resultTable.Rows[1][0]);

                    if (salesCount > 0 || purchaseCount > 0)
                    {
                        throw new Exception("Sản phẩm đã được sử dụng trong giao dịch, không thể xóa!");
                    }
                }

                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", productId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho của sản phẩm
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <param name="quantityChange">Lượng thay đổi (dương nếu nhập hàng, âm nếu bán hàng)</param>
        /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy sản phẩm</returns>
        public bool UpdateStock(int productId, int quantityChange)
        {
            // Kiểm tra tồn kho nếu giảm
            if (quantityChange < 0)
            {
                string checkQuery = "SELECT StockQuantity FROM Products WHERE ProductID = @ProductID";
                SqlParameter[] checkParameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", productId)
                };

                object currentStock = DatabaseHelper.ExecuteScalar(checkQuery, checkParameters);
                if (currentStock == null || Convert.ToInt32(currentStock) < Math.Abs(quantityChange))
                {
                    throw new Exception("Số lượng tồn kho không đủ!");
                }
            }

            string query = @"UPDATE Products 
                            SET StockQuantity = StockQuantity + @QuantityChange 
                            WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@ProductID", productId),
               new SqlParameter("@QuantityChange", quantityChange)
            };

            try
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tồn kho: " + ex.Message);
            }
        }

        #endregion
    }
}