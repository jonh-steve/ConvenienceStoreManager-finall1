using System;
using System.Collections.Generic;
using System.Data;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.Utils;

namespace ConvenienceStoreManager.BusinessLogic
{
    public class PurchaseService
    {
        private PurchaseRepository purchaseRepository;
        private ProductRepository productRepository;

        /// <summary>
        /// Khởi tạo một instance của PurchaseService
        /// </summary>
        public PurchaseService()
        {
            purchaseRepository = new PurchaseRepository();
            productRepository = new ProductRepository();
        }

        /// <summary>
        /// Tạo phiếu nhập mới và các chi tiết phiếu nhập
        /// </summary>
        /// <param name="purchase">Thông tin phiếu nhập</param>
        /// <param name="details">Danh sách chi tiết phiếu nhập</param>
        /// <returns>ID của phiếu nhập vừa tạo</returns>
        /// <exception cref="ArgumentNullException">Nếu phiếu nhập hoặc chi tiết phiếu nhập là null</exception>
        /// <exception cref="ArgumentException">Nếu dữ liệu không hợp lệ</exception>
        /// <exception cref="Exception">Nếu không tìm thấy sản phẩm hoặc có lỗi khác</exception>
        public void CreatePurchaseOrder(PurchaseOrder purchase, List<PurchaseOrderDetail> details)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase), "Phiếu nhập không được phép null");

            if (details == null || details.Count == 0)
                throw new ArgumentException("Chi tiết phiếu nhập không được phép rỗng", nameof(details));

            if (purchase.TotalAmount <= 0)
                throw new ArgumentException("Tổng tiền phiếu nhập phải lớn hơn 0", nameof(purchase.TotalAmount));

            // Kiểm tra thông tin của từng chi tiết phiếu nhập
            foreach (PurchaseOrderDetail detail in details)
            {
                Product product = productRepository.GetProductById(detail.ProductID);

                if (product == null)
                    throw new Exception($"Không tìm thấy sản phẩm có ID {detail.ProductID}");

                if (detail.Quantity <= 0)
                    throw new ArgumentException($"Số lượng sản phẩm {product.ProductName} phải lớn hơn 0", nameof(detail.Quantity));

                if (detail.PurchasePrice <= 0)
                    throw new ArgumentException($"Giá nhập của sản phẩm {product.ProductName} phải lớn hơn 0", nameof(detail.PurchasePrice));

                if (Math.Abs(detail.Subtotal - (detail.Quantity * detail.PurchasePrice)) > 0.01m)
                    throw new ArgumentException($"Thành tiền của sản phẩm {product.ProductName} không chính xác", nameof(detail.Subtotal));
            }

            // Kiểm tra tổng tiền
            decimal calculatedTotal = 0;
            foreach (PurchaseOrderDetail detail in details)
            {
                calculatedTotal += detail.Subtotal;
            }

            if (Math.Abs(calculatedTotal - purchase.TotalAmount) > 1) // Cho phép sai số nhỏ do làm tròn
                throw new ArgumentException($"Tổng tiền phiếu nhập ({purchase.TotalAmount}) không khớp với tổng thành tiền chi tiết ({calculatedTotal})");

            try
            {
                // Tạo phiếu nhập
                purchaseRepository.CreatePurchaseOrder(purchase, details);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin phiếu nhập theo ID
        /// </summary>
        /// <param name="purchaseOrderID">ID của phiếu nhập</param>
        /// <returns>Đối tượng PurchaseOrder hoặc null nếu không tìm thấy</returns>
        /// <exception cref="ArgumentException">Nếu ID không hợp lệ</exception>
        public PurchaseOrder GetPurchaseOrderById(int purchaseOrderID)
        {
            if (purchaseOrderID <= 0)
                throw new ArgumentException("ID phiếu nhập không hợp lệ", nameof(purchaseOrderID));

            try
            {
                return purchaseRepository.GetPurchaseOrderById(purchaseOrderID);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách chi tiết phiếu nhập theo ID phiếu nhập
        /// </summary>
        /// <param name="purchaseOrderID">ID của phiếu nhập</param>
        /// <returns>Danh sách chi tiết phiếu nhập</returns>
        /// <exception cref="ArgumentException">Nếu ID không hợp lệ</exception>
        public List<PurchaseOrderDetail> GetPurchaseOrderDetails(int purchaseOrderID)
        {
            if (purchaseOrderID <= 0)
                throw new ArgumentException("ID phiếu nhập không hợp lệ", nameof(purchaseOrderID));

            try
            {
                return purchaseRepository.GetPurchaseOrderDetails(purchaseOrderID);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả phiếu nhập
        /// </summary>
        /// <returns>Danh sách các phiếu nhập</returns>
        public List<PurchaseOrder> GetAllPurchaseOrders()
        {
            try
            {
                return purchaseRepository.GetAllPurchaseOrders();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách phiếu nhập theo khoảng thời gian
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu</param>
        /// <param name="toDate">Ngày kết thúc</param>
        /// <returns>Danh sách các phiếu nhập</returns>
        /// <exception cref="ArgumentException">Nếu khoảng thời gian không hợp lệ</exception>
        public List<PurchaseOrder> GetPurchaseOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc");

            try
            {
                return purchaseRepository.GetPurchaseOrdersByDateRange(fromDate, toDate);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra xem phiếu nhập có tồn tại hay không
        /// </summary>
        /// <param name="purchaseOrderID">ID của phiếu nhập</param>
        /// <returns>True nếu phiếu nhập tồn tại, False nếu không</returns>
        public bool PurchaseOrderExists(int purchaseOrderID)
        {
            try
            {
                return GetPurchaseOrderById(purchaseOrderID) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tính tổng chi phí nhập hàng theo khoảng thời gian
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu</param>
        /// <param name="toDate">Ngày kết thúc</param>
        /// <returns>Tổng chi phí nhập hàng</returns>
        /// <exception cref="ArgumentException">Nếu khoảng thời gian không hợp lệ</exception>
        public decimal CalculatePurchaseCost(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc");

            try
            {
                List<PurchaseOrder> purchases = GetPurchaseOrdersByDateRange(fromDate, toDate);
                decimal cost = 0;

                foreach (PurchaseOrder purchase in purchases)
                {
                    cost += purchase.TotalAmount;
                }

                return cost;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
                throw;
            }
        }
    }
}
