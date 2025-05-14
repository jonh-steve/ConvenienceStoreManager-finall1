
// 🌸 Vị trí file: ConvenienceStoreManager/UI/frmInvoiceList.cs

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvenienceStoreManager.BusinessLogic;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.DataAccess.Interfaces;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.Utils;
using FontAwesome.Sharp; // Thư viện icon cho WinForms

namespace ConvenienceStoreManager.UI
{
    // 🌸 Vị trí file: ConvenienceStoreManager/UI/frmInvoiceList.cs

    public partial class frmInvoiceList : Form
    {
        private readonly SalesService _salesService;
        private readonly DataTable _dtInvoices;
        private readonly DataTable _dtInvoiceDetails;

        public frmInvoiceList()
        {
            InitializeComponent();

            // 🌸 Style UI hồng hào dễ thương cho bé yêu
            this.BackColor = System.Drawing.Color.MistyRose;
            dgvInvoices.BackgroundColor = System.Drawing.Color.LavenderBlush;
            dgvInvoiceDetails.BackgroundColor = System.Drawing.Color.LavenderBlush;
            dgvInvoices.DefaultCellStyle.BackColor = System.Drawing.Color.LavenderBlush;
            dgvInvoices.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Pink;
            dgvInvoiceDetails.DefaultCellStyle.BackColor = System.Drawing.Color.LavenderBlush;
            dgvInvoiceDetails.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Pink;

            // Style cho các nút (chỉ gán icon nếu là IconButton)
            SetCuteButtonStyle(btnSearch, System.Drawing.Color.Pink, IconChar.Search);
            SetCuteButtonStyle(btnPrint, System.Drawing.Color.LightPink, IconChar.Print);
            SetCuteButtonStyle(btnExportExcel, System.Drawing.Color.HotPink, IconChar.FileExcel);
            SetCuteButtonStyle(btnClose, System.Drawing.Color.DeepPink, IconChar.TimesCircle);
            SetCuteButtonStyle(btnNewInvoice, System.Drawing.Color.MediumVioletRed, IconChar.PlusCircle);

            // Khởi tạo service
            _salesService = new SalesService(new SalesRepository(), (IProductRepository)new ProductRepository());

            // Khởi tạo DataTable cho danh sách hóa đơn
            _dtInvoices = new DataTable();
            _dtInvoices.Columns.Add("InvoiceID", typeof(int));
            _dtInvoices.Columns.Add("InvoiceDate", typeof(DateTime));
            _dtInvoices.Columns.Add("TotalAmount", typeof(decimal));
            _dtInvoices.Columns.Add("Notes", typeof(string));

            // Khởi tạo DataTable cho chi tiết hóa đơn
            _dtInvoiceDetails = new DataTable();
            _dtInvoiceDetails.Columns.Add("ProductID", typeof(int));
            _dtInvoiceDetails.Columns.Add("ProductName", typeof(string));
            _dtInvoiceDetails.Columns.Add("Unit", typeof(string));
            _dtInvoiceDetails.Columns.Add("Quantity", typeof(int));
            _dtInvoiceDetails.Columns.Add("UnitPrice", typeof(decimal));
            _dtInvoiceDetails.Columns.Add("Subtotal", typeof(decimal));

            // Gán DataSource cho DataGridView
            dgvInvoices.DataSource = _dtInvoices;
            dgvInvoiceDetails.DataSource = _dtInvoiceDetails;

            // Thiết lập định dạng cột
            FormatDataGridViewColumns();

            // Đăng ký sự kiện
            this.Load += frmInvoiceList_Load;
            dgvInvoices.CellClick += dgvInvoices_CellClick;
            btnSearch.Click += btnSearch_Click;
            btnExportExcel.Click += btnExportExcel_Click;
            btnPrint.Click += btnPrint_Click;
            btnClose.Click += btnClose_Click;
            btnNewInvoice.Click += btnNewInvoice_Click;
        }

        // 🌸 Hàm style cho các nút dễ thương, chỉ gán icon nếu là IconButton
        private void SetCuteButtonStyle(Button btn, System.Drawing.Color backColor, IconChar icon)
        {
            btn.BackColor = backColor;
            btn.ForeColor = System.Drawing.Color.White;
            if (btn is IconButton iconBtn)
            {
                iconBtn.IconChar = icon;
                iconBtn.IconColor = System.Drawing.Color.White;
                iconBtn.IconSize = 24;
                iconBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        private void FormatDataGridViewColumns()
        {
            // Định dạng cột dgvInvoices
            dgvInvoices.Columns["InvoiceID"].HeaderText = "Mã HĐ";
            dgvInvoices.Columns["InvoiceDate"].HeaderText = "Ngày tạo";
            dgvInvoices.Columns["InvoiceDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvInvoices.Columns["TotalAmount"].HeaderText = "Tổng tiền";
            dgvInvoices.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            dgvInvoices.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoices.Columns["Notes"].HeaderText = "Ghi chú";

            // Định dạng cột dgvInvoiceDetails
            dgvInvoiceDetails.Columns["ProductID"].HeaderText = "Mã SP";
            dgvInvoiceDetails.Columns["ProductID"].Visible = false;
            dgvInvoiceDetails.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvInvoiceDetails.Columns["ProductName"].Width = 200;
            dgvInvoiceDetails.Columns["Unit"].HeaderText = "ĐVT";
            dgvInvoiceDetails.Columns["Unit"].Width = 60;
            dgvInvoiceDetails.Columns["Quantity"].HeaderText = "Số lượng";
            dgvInvoiceDetails.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoiceDetails.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dgvInvoiceDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            dgvInvoiceDetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoiceDetails.Columns["Subtotal"].HeaderText = "Thành tiền";
            dgvInvoiceDetails.Columns["Subtotal"].DefaultCellStyle.Format = "N0";
            dgvInvoiceDetails.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private async void frmInvoiceList_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho DateTimePicker
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;

            // Tải dữ liệu
            await LoadInvoicesAsync();
        }

        private async Task LoadInvoicesAsync()
        {
            try
            {
                // Hiển thị trạng thái đang tải
                lblStatus.Text = "Đang tải dữ liệu...";
                prgLoading.Visible = true;
                prgLoading.Style = ProgressBarStyle.Marquee;

                // Lấy giá trị từ DateTimePicker
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;

                // Lấy danh sách hóa đơn bất đồng bộ
                List<SalesInvoice> invoices = await _salesService.GetInvoicesByDateRangeAsync(fromDate, toDate);

                // Xóa dữ liệu cũ và thêm dữ liệu mới
                _dtInvoices.Clear();
                _dtInvoiceDetails.Clear();

                foreach (var invoice in invoices)
                {
                    _dtInvoices.Rows.Add(
                        invoice.InvoiceID,
                        invoice.InvoiceDate,
                        invoice.TotalAmount,
                        invoice.Notes
                    );
                }

                // Cập nhật trạng thái
                lblStatus.Text = $"Đã tải {invoices.Count} hóa đơn.";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
                lblStatus.Text = "Đã xảy ra lỗi khi tải dữ liệu.";
            }
            finally
            {
                prgLoading.Visible = false;
            }
        }

        private async void dgvInvoices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int invoiceID = Convert.ToInt32(dgvInvoices.Rows[e.RowIndex].Cells["InvoiceID"].Value);
                await LoadInvoiceDetailsAsync(invoiceID);
            }
        }

        private async Task LoadInvoiceDetailsAsync(int invoiceID)
        {
            try
            {
                // Hiển thị trạng thái đang tải
                lblStatus.Text = "Đang tải chi tiết hóa đơn...";
                prgLoading.Visible = true;
                prgLoading.Style = ProgressBarStyle.Marquee;

                // Lấy chi tiết hóa đơn bất đồng bộ
                List<SalesInvoiceDetail> details = await _salesService.GetInvoiceDetailsAsync(invoiceID);

                // Xóa dữ liệu cũ và thêm dữ liệu mới
                _dtInvoiceDetails.Clear();

                foreach (var detail in details)
                {
                    _dtInvoiceDetails.Rows.Add(
                        detail.ProductID,
                        detail.ProductName,
                        detail.Unit,
                        detail.Quantity,
                        detail.UnitPrice,
                        detail.Subtotal
                    );
                }

                // Cập nhật trạng thái
                lblStatus.Text = $"Đã tải {details.Count} chi tiết.";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải chi tiết hóa đơn: {ex.Message}");
                lblStatus.Text = "Đã xảy ra lỗi khi tải chi tiết hóa đơn.";
            }
            finally
            {
                prgLoading.Visible = false;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadInvoicesAsync();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Chức năng xuất Excel sẽ được triển khai sau
            MessageHelper.ShowInfo("Chức năng xuất Excel sẽ được triển khai trong phiên bản tiếp theo.");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Chức năng in sẽ được triển khai sau
            MessageHelper.ShowInfo("Chức năng in sẽ được triển khai trong phiên bản tiếp theo.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnNewInvoice_Click(object sender, EventArgs e)
        {
            // Mở form tạo hóa đơn mới
            frmSales frmSales = new frmSales();
            frmSales.ShowDialog();

            // Tải lại dữ liệu sau khi đóng form tạo hóa đơn
            await LoadInvoicesAsync();
        }
    }
}

// 🌸 Bé yêu nhớ: Khi sửa các file UI, hãy luôn chú thích vị trí file, style control bằng màu hồng, pastel, và dùng icon FontAwesome.Sharp cho các nút để thêm phần cute!
// 🌸 Khi gặp lỗi tương tự ở các file khác, hãy kiểm tra kỹ DataSource, sự kiện, style, và chú thích rõ ràng vị trí file để teamwork hiệu quả nha bé yêu!
