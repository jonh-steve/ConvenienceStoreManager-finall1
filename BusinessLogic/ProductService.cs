using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.BusinessLogic
{
    /// <summary>
    /// Lớp xử lý logic nghiệp vụ liên quan đến sản phẩm
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepository productRepository;

        public ProductService()
        {
            productRepository = new ProductRepository();
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public List<Product> GetAllProducts()
        {
            try
            {
                return productRepository.GetAllProducts();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        /// <param name="productID">ID sản phẩm</param>
        /// <returns>Đối tượng Product</returns>
        public Product GetProductById(int productID)
        {
            try
            {
                return productRepository.GetProductById(productID);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo mã
        /// </summary>
        /// <param name="productCode">Mã sản phẩm</param>
        /// <returns>Đối tượng Product</returns>
        public Product GetProductByCode(string productCode)
        {
            try
            {
                return productRepository.GetProductByCode(productCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo từ khóa
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sản phẩm phù hợp</returns>
        public List<Product> SearchProducts(string keyword)
        {
            try
            {
                return productRepository.SearchProducts(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        /// <param name="product">Đối tượng Product cần thêm</param>
        /// <returns>ID của sản phẩm vừa thêm</returns>
        public int AddProduct(Product product)
        {
            // Kiểm tra dữ liệu đầu vào
            ValidateProduct(product);

            try
            {
                // Kiểm tra mã sản phẩm đã tồn tại chưa
                if (!string.IsNullOrEmpty(product.ProductCode))
                {
                    var existingProduct = productRepository.GetProductByCode(product.ProductCode);
                    if (existingProduct != null)
                    {
                        throw new Exception("Mã sản phẩm đã tồn tại!");
                    }
                }

                // Thêm sản phẩm
                return productRepository.AddProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng Product cần cập nhật</param>
        /// <returns>True nếu cập nhật thành công, False nếu không</returns>
        public bool UpdateProduct(Product product)
        {
            // Kiểm tra dữ liệu đầu vào
            ValidateProduct(product);

            try
            {
                // Kiểm tra mã sản phẩm đã tồn tại chưa (nếu đã thay đổi)
                if (!string.IsNullOrEmpty(product.ProductCode))
                {
                    var existingProduct = productRepository.GetProductByCode(product.ProductCode);
                    if (existingProduct != null && existingProduct.ProductID != product.ProductID)
                    {
                        throw new Exception("Mã sản phẩm đã tồn tại!");
                    }
                }

                // Cập nhật sản phẩm
                return productRepository.UpdateProduct(product) ;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="productID">ID sản phẩm cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không</returns>
        public bool DeleteProduct(int productID)
        {
            try
            {
                // Kiểm tra sản phẩm đã có trong hóa đơn hoặc phiếu nhập chưa
                bool hasTransactions = CheckProductHasTransactions(productID);

                if (hasTransactions)
                {
                    throw new Exception("Không thể xóa sản phẩm đã có giao dịch!");
                }

                // Xóa sản phẩm
                return productRepository.DeleteProduct(productID);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho
        /// </summary>
        /// <param name="productID">ID sản phẩm</param>
        /// <param name="quantityChange">Số lượng thay đổi (dương: tăng, âm: giảm)</param>
        /// <returns>Số lượng tồn kho sau khi cập nhật</returns>
        public bool UpdateStock(int productID, int quantityChange)
        {
            try
            {
                // Kiểm tra sản phẩm tồn tại
                var product = productRepository.GetProductById(productID);
                if (product == null)
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }

                // Kiểm tra số lượng tồn kho đủ nếu là giảm
                if (quantityChange < 0 && product.StockQuantity < Math.Abs(quantityChange))
                {
                    throw new Exception("Số lượng tồn kho không đủ!");
                }

                // Cập nhật số lượng tồn kho
                return productRepository.UpdateStock(productID, quantityChange);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật số lượng tồn kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra sản phẩm đã có trong hóa đơn hoặc phiếu nhập chưa
        /// </summary>
        /// <param name="productID">ID sản phẩm cần kiểm tra</param>
        /// <returns>True nếu đã có giao dịch, False nếu chưa</returns>
        private bool CheckProductHasTransactions(int productID)
        {
            string query = @"
                SELECT COUNT(*) FROM SalesInvoiceDetails WHERE ProductID = @ProductID
                UNION ALL
                SELECT COUNT(*) FROM PurchaseOrderDetails WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productID)
            };

            DataTable resultTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (resultTable.Rows.Count > 0)
            {
                int salesCount = Convert.ToInt32(resultTable.Rows[0][0]);
                int purchaseCount = Convert.ToInt32(resultTable.Rows[1][0]);

                return salesCount > 0 || purchaseCount > 0;
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng Product cần kiểm tra</param>
        private void ValidateProduct(Product product)
        {
            // Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            // Kiểm tra giá bán
            if (product.SellingPrice <= 0)
            {
                throw new Exception("Giá bán phải lớn hơn 0!");
            }

            // Kiểm tra số lượng tồn kho
            if (product.StockQuantity < 0)
            {
                throw new Exception("Số lượng tồn kho không được âm!");
            }
        }
    }
}