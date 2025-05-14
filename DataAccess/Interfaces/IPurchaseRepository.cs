// File: IPurchaseRepository.cs
// Vị trí: ConvenienceStoreManager/DataAccess/Interfaces/IPurchaseRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.DataAccess
{
    public interface IPurchaseRepository
    {
        /// <summary>
        /// Lấy tất cả phiếu nhập.
        /// </summary>
        List<PurchaseOrder> GetAllPurchaseOrders();

        /// <summary>
        /// Lấy tất cả phiếu nhập bất đồng bộ.
        /// </summary>
        Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync();

        /// <summary>
        /// Lấy phiếu nhập theo ID.
        /// </summary>
        /// <param name="id">Mã phiếu nhập.</param>
        PurchaseOrder GetPurchaseOrderById(int id);

        /// <summary>
        /// Lấy phiếu nhập theo ID bất đồng bộ.
        /// </summary>
        /// <param name="id">Mã phiếu nhập.</param>
        Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);

        /// <summary>
        /// Lấy chi tiết phiếu nhập theo mã phiếu nhập.
        /// </summary>
        /// <param name="purchaseOrderId">Mã phiếu nhập.</param>
        List<PurchaseOrderDetail> GetPurchaseOrderDetails(int purchaseOrderId);

        /// <summary>
        /// Lấy chi tiết phiếu nhập bất đồng bộ.
        /// </summary>
        /// <param name="purchaseOrderId">Mã phiếu nhập.</param>
        Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailsAsync(int purchaseOrderId);

        /// <summary>
        /// Lấy danh sách phiếu nhập theo khoảng thời gian.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        List<PurchaseOrder> GetPurchaseOrdersByDateRange(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Lấy danh sách phiếu nhập theo khoảng thời gian bất đồng bộ.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        Task<List<PurchaseOrder>> GetPurchaseOrdersByDateRangeAsync(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Tạo phiếu nhập mới và chi tiết phiếu nhập.
        /// </summary>
        /// <param name="order">Thông tin phiếu nhập.</param>
        /// <param name="details">Danh sách chi tiết phiếu nhập.</param>
        void CreatePurchaseOrder(PurchaseOrder order, List<PurchaseOrderDetail> details);

        /// <summary>
        /// Tạo phiếu nhập mới và chi tiết phiếu nhập bất đồng bộ.
        /// </summary>
        /// <param name="order">Thông tin phiếu nhập.</param>
        /// <param name="details">Danh sách chi tiết phiếu nhập.</param>
        Task CreatePurchaseOrderAsync(PurchaseOrder order, List<PurchaseOrderDetail> details);
    }
}