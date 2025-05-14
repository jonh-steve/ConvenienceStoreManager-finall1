using System;
using System.Windows.Forms;
using System.Data;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Utils;
using System.Drawing.Printing;
using System.Drawing;

namespace ConvenienceStoreManager.UI
{
    public partial class frmPurchaseOrderList : Form
    {
        // Biến
        private readonly PurchaseRepository purchaseRepository; // Đối tượng truy xuất dữ liệu phiếu nhập
        private DataTable dtPurchaseOrders; // Lưu trữ danh sách phiếu nhập
        private DataTable dtPurchaseDetails; // Lưu trữ chi tiết phiếu nhập

        // Constructor
        public frmPurchaseOrderList()
        {
            InitializeComponent();
            purchaseRepository = new PurchaseRepository();
            dtPurchaseOrders = new DataTable();
            dtPurchaseDetails = new DataTable();
        }

        // Sự kiện tải form
        private void frmPurchaseOrderList_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;
            InitializeGridViews();
            LoadPurchaseOrders();
        }

        // Khởi tạo cấu trúc DataGridView
        private void InitializeGridViews()
        {
            dtPurchaseOrders.Columns.Add("PurchaseOrderID", typeof(int));
            dtPurchaseOrders.Columns.Add("OrderDate", typeof(DateTime));
            dtPurchaseOrders.Columns.Add("SupplierName", typeof(string));
            dtPurchaseOrders.Columns.Add("TotalAmount", typeof(decimal));
            dtPurchaseOrders.Columns.Add("Notes", typeof(string));

            dtPurchaseDetails.Columns.Add("ProductID", typeof(int));
            dtPurchaseDetails.Columns.Add("ProductName", typeof(string));
            dtPurchaseDetails.Columns.Add("Quantity", typeof(int));
            dtPurchaseDetails.Columns.Add("PurchasePrice", typeof(decimal));
            dtPurchaseDetails.Columns.Add("Subtotal", typeof(decimal));

            dgvPurchaseOrders.DataSource = dtPurchaseOrders;
            dgvPurchaseDetails.DataSource = dtPurchaseDetails;

            dgvPurchaseOrders.Columns["PurchaseOrderID"].HeaderText = "Mã phiếu nhập";
            dgvPurchaseOrders.Columns["OrderDate"].HeaderText = "Ngày nhập";
            dgvPurchaseOrders.Columns["SupplierName"].HeaderText = "Nhà cung cấp";
            dgvPurchaseOrders.Columns["TotalAmount"].HeaderText = "Tổng tiền";
            dgvPurchaseOrders.Columns["Notes"].HeaderText = "Ghi chú";

            dgvPurchaseDetails.Columns["ProductID"].HeaderText = "Mã sản phẩm";
            dgvPurchaseDetails.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvPurchaseDetails.Columns["Quantity"].HeaderText = "Số lượng";
            dgvPurchaseDetails.Columns["PurchasePrice"].HeaderText = "Giá nhập";
            dgvPurchaseDetails.Columns["Subtotal"].HeaderText = "Thành tiền";
        }

        // Tải danh sách phiếu nhập
        private void LoadPurchaseOrders()
        {
            dtPurchaseOrders.Clear();
            var purchaseOrders = purchaseRepository.GetPurchaseOrdersByDateRange(dtpFromDate.Value, dtpToDate.Value);
            foreach (var purchase in purchaseOrders)
            {
                dtPurchaseOrders.Rows.Add(purchase.PurchaseOrderID, purchase.OrderDate, purchase.SupplierName, purchase.TotalAmount, purchase.Notes);
            }
            dgvPurchaseOrders.Refresh();
        }

        // Tìm kiếm phiếu nhập theo mã
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadPurchaseOrders();
                return;
            }

            dtPurchaseOrders.Clear();
            var purchase = purchaseRepository.GetPurchaseOrderById(int.TryParse(searchText, out int id) ? id : -1);
            if (purchase != null)
            {
                dtPurchaseOrders.Rows.Add(purchase.PurchaseOrderID, purchase.OrderDate, purchase.SupplierName, purchase.TotalAmount, purchase.Notes);
            }
            dgvPurchaseOrders.Refresh();
        }

        // Xem chi tiết phiếu nhập
        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count > 0)
            {
                int purchaseOrderID = Convert.ToInt32(dgvPurchaseOrders.SelectedRows[0].Cells["PurchaseOrderID"].Value);
                LoadPurchaseDetails(purchaseOrderID);
            }
        }

        // Tải chi tiết phiếu nhập
        private void LoadPurchaseDetails(int purchaseOrderID)
        {
            dtPurchaseDetails.Clear();
            var details = purchaseRepository.GetPurchaseOrderDetails(purchaseOrderID);
            foreach (var detail in details)
            {
                dtPurchaseDetails.Rows.Add(detail.ProductID, detail.ProductName, detail.Quantity, detail.PurchasePrice, detail.Subtotal);
            }
            dgvPurchaseDetails.Refresh();
        }

        // In phiếu nhập
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count > 0)
            {
                int purchaseOrderID = Convert.ToInt32(dgvPurchaseOrders.SelectedRows[0].Cells["PurchaseOrderID"].Value);
                PrintPurchaseOrder(purchaseOrderID);
            }
            else
            {
                MessageHelper.ShowWarning("Vui lòng chọn một phiếu nhập để in!");
            }
        }

        // Xử lý in phiếu nhập
        private void PrintPurchaseOrder(int purchaseOrderID)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) => PrintPurchaseOrderPage(s, ev, purchaseOrderID);
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDoc
            };
            previewDialog.ShowDialog();
        }

        // In trang phiếu nhập
        private void PrintPurchaseOrderPage(object sender, PrintPageEventArgs e, int purchaseOrderID)
        {
            float yPos = 10;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            Font printFont = new Font("Arial", 12);
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);

            var purchase = purchaseRepository.GetPurchaseOrderById(purchaseOrderID);
            var details = purchaseRepository.GetPurchaseOrderDetails(purchaseOrderID);

            // In tiêu đề
            e.Graphics.DrawString("PHIẾU NHẬP HÀNG", titleFont, Brushes.Black, leftMargin, yPos);
            yPos += 40;
            e.Graphics.DrawString($"Mã phiếu nhập: {purchase.PurchaseOrderID}", printFont, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            e.Graphics.DrawString($"Ngày nhập: {purchase.OrderDate:dd/MM/yyyy HH:mm}", printFont, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            e.Graphics.DrawString($"Nhà cung cấp: {purchase.SupplierName}", printFont, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            // In chi tiết
            e.Graphics.DrawString("Danh sách sản phẩm:", printFont, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            foreach (var detail in details)
            {
                string line = $"{detail.ProductName} ({detail.Quantity} {detail.Unit}) - {detail.PurchasePrice:N2} = {detail.Subtotal:N2}";
                e.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 20;
            }

            yPos += 20;
            e.Graphics.DrawString($"Tổng tiền: {purchase.TotalAmount:N2} VND", printFont, Brushes.Black, leftMargin, yPos);
        }

        // Sự kiện khi thay đổi ngày
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }
    }
}