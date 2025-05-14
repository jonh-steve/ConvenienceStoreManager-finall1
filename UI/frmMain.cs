using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlTypes;

namespace ConvenienceStoreManager.UI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin ứng dụng trên tiêu đề form
            string applicationName = ConfigurationManager.AppSettings["ApplicationName"];
            string version = ConfigurationManager.AppSettings["Version"];
            this.Text = $"{applicationName} - Phiên bản {version}";

            // Hiển thị tên người dùng (nếu có)
            lblUsername.Text = "Người dùng: Admin"; // Có thể thay bằng tên người dùng thực tế nếu có hệ thống đăng nhập

            // Hiển thị ngày giờ hiện tại
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            timerDateTime.Start();

            // Cập nhật trạng thái
            lblStatus.Text = "Sẵn sàng...";
        }

        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            // Cập nhật ngày giờ hiện tại mỗi 1 giây
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        // Xử lý sự kiện khi click menu Quản lý -> Sản phẩm
        private void mnuProductManagement_Click(object sender, EventArgs e)
        {
            frmProductManagement frm = new frmProductManagement();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Quản lý -> Danh sách hóa đơn
        private void mnuInvoiceList_Click(object sender, EventArgs e)
        {
            frmInvoiceList frm = new frmInvoiceList();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Quản lý -> Danh sách phiếu nhập
        private void mnuPurchaseOrderList_Click(object sender, EventArgs e)
        {
            frmPurchaseOrderList frm = new frmPurchaseOrderList();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Nghiệp vụ -> Bán hàng
        private void mnuSales_Click(object sender, EventArgs e)
        {
            frmSales frm = new frmSales();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Nghiệp vụ -> Nhập hàng
        private void mnuPurchaseOrder_Click(object sender, EventArgs e)
        {
            frmPurchaseOrder frm = new frmPurchaseOrder();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Báo cáo -> Doanh thu
        private void mnuReports_Click(object sender, EventArgs e)
        {
            frmReports frm = new frmReports();
            frm.ShowDialog();
        }

        // Xử lý sự kiện khi click menu Hệ thống -> Thoát
        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát khỏi ứng dụng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Xử lý sự kiện khi click menu Trợ giúp -> Thông tin phần mềm
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            string applicationName = ConfigurationManager.AppSettings["ApplicationName"];
            string version = ConfigurationManager.AppSettings["Version"];
            string companyName = ConfigurationManager.AppSettings["CompanyName"];

            string message = $"{applicationName}\nPhiên bản: {version}\nPhát triển bởi: {companyName}\n\n© 2025 Bản quyền thuộc về {companyName}";
            MessageBox.Show(message, "Thông tin phần mềm", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}