// Vị trí: ConvenienceStoreManager/UI/frmReports.cs

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ConvenienceStoreManager.BusinessLogic;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Utils;

namespace ConvenienceStoreManager.UI
{
    public partial class frmReports : Form
    {
        private readonly ReportService _reportService;

        public frmReports()
        {
            InitializeComponent();

            // Khởi tạo service
            _reportService = new ReportService(new SalesRepository(), new ProductRepository(), new SalesService(new SalesRepository(), new ProductRepository()));
        }

        private async void frmReports_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho DateTimePicker
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;

            // Thiết lập giá trị mặc định cho ComboBox
            cboTopCount.Items.AddRange(new object[] { 5, 10, 20, 50 });
            cboTopCount.SelectedIndex = 0;

            // Tải dữ liệu
            await LoadReportsAsync();
        }

        private async Task LoadReportsAsync()
        {
            try
            {
                // Hiển thị trạng thái đang tải
                lblStatus.Text = "Đang tải dữ liệu báo cáo...";
                prgLoading.Visible = true;
                prgLoading.Style = ProgressBarStyle.Marquee;

                // Lấy giá trị từ controls
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;
                int topCount = Convert.ToInt32(cboTopCount.SelectedItem);

                // Tính doanh thu, lợi nhuận
                decimal revenue = await _reportService.CalculateRevenueAsync(fromDate, toDate);
                decimal profit = await _reportService.CalculateProfitAsync(fromDate, toDate);
                decimal inventoryValue = await _reportService.GetInventoryValueAsync();

                // Hiển thị thông tin tổng quan
                lblRevenue.Text = revenue.ToString("N0") + " VND";
                lblProfit.Text = profit.ToString("N0") + " VND";
                lblInventoryValue.Text = inventoryValue.ToString("N0") + " VND";

                // Lấy danh sách sản phẩm bán chạy nhất
                var topProducts = await _reportService.GetTopSellingProductsAsync(fromDate, toDate, topCount);
                dgvTopProducts.DataSource = topProducts;

                // Lấy doanh thu theo ngày
                var dailyRevenue = await _reportService.GetDailyRevenueAsync(fromDate, toDate);

                // Hiển thị biểu đồ doanh thu
                await LoadRevenueChartAsync(dailyRevenue);

                // Cập nhật trạng thái
                lblStatus.Text = "Đã tải xong dữ liệu báo cáo.";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải dữ liệu báo cáo: {ex.Message}");
                lblStatus.Text = "Đã xảy ra lỗi khi tải dữ liệu báo cáo.";
            }
            finally
            {
                prgLoading.Visible = false;
            }
        }


        private async Task LoadRevenueChartAsync(List<DailyRevenueReport> dailyRevenue)
{
    await Task.Run(() =>
    {
        // Chuẩn bị dữ liệu (background thread)
        var chartArea = new ChartArea("MainChartArea")
        {
            AxisX = { Title = "Ngày", LabelStyle = { Format = "dd/MM" }, MajorGrid = { LineColor = Color.LightGray } },
            AxisY = { Title = "Doanh thu (VND)", MajorGrid = { LineColor = Color.LightGray } }
        };
        var series = new Series("Doanh thu")
        {
            ChartType = SeriesChartType.Column,
            Color = Color.HotPink, // Bé yêu chọn màu hồng cho dễ thương
            Legend = "Legend"      // Bé yêu nhớ gán đúng tên Legend nhé!
        };
        foreach (var item in dailyRevenue)
        {
            series.Points.AddXY(item.RevenueDate, item.DailyRevenue);
        }
        var legend = new Legend("Legend") { Docking = Docking.Bottom };
        var title = new Title("Doanh thu theo ngày", Docking.Top, new Font("Comic Sans MS", 12, FontStyle.Bold), Color.DeepPink);

        // Update UI (UI thread)
        Invoke(new Action(() =>
        {
            chartRevenue.Series.Clear();
            chartRevenue.ChartAreas.Clear();
            chartRevenue.ChartAreas.Add(chartArea);
            chartRevenue.Series.Add(series);
            chartRevenue.Legends.Clear();
            chartRevenue.Legends.Add(legend);
            chartRevenue.Titles.Clear();
            chartRevenue.Titles.Add(title);

            // Bé yêu thêm style nền cho chart cho thật hồng hào
            chartRevenue.BackColor = Color.MistyRose;
            chartRevenue.ChartAreas[0].BackColor = Color.WhiteSmoke;
        }));
    });
}



        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private async void cboTopCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tải lại danh sách sản phẩm bán chạy nhất với số lượng mới
            try
            {
                int topCount = Convert.ToInt32(cboTopCount.SelectedItem);
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;

                var topProducts = await _reportService.GetTopSellingProductsAsync(fromDate, toDate, topCount);
                dgvTopProducts.DataSource = topProducts;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải danh sách sản phẩm bán chạy: {ex.Message}");
            }
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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cập nhật dữ liệu khi chuyển tab
            switch (tabControl.SelectedIndex)
            {
                case 0: // Tab Tổng quan
                    // Dữ liệu đã được tải khi form load
                    break;
                case 1: // Tab Sản phẩm
                    LoadProductReportsAsync();
                    break;
                case 2: // Tab Doanh thu
                    LoadRevenueReportsAsync();
                    break;
            }
        }

        private async void LoadProductReportsAsync()
        {
            try
            {
                // Hiển thị trạng thái đang tải
                lblStatus.Text = "Đang tải dữ liệu báo cáo sản phẩm...";
                prgLoading.Visible = true;
                prgLoading.Style = ProgressBarStyle.Marquee;

                // Lấy giá trị từ controls
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;
                int topCount = Convert.ToInt32(cboTopCount.SelectedItem);

                // Lấy danh sách sản phẩm có doanh thu cao nhất
                var topRevenueProducts = await _reportService.GetTopRevenueProductsAsync(fromDate, toDate, topCount);
                dgvTopRevenueProducts.DataSource = topRevenueProducts;

                // Cập nhật trạng thái
                lblStatus.Text = "Đã tải xong dữ liệu báo cáo sản phẩm.";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải dữ liệu báo cáo sản phẩm: {ex.Message}");
                lblStatus.Text = "Đã xảy ra lỗi khi tải dữ liệu báo cáo sản phẩm.";
            }
            finally
            {
                prgLoading.Visible = false;
            }
        }

        private async void LoadRevenueReportsAsync()
        {
            try
            {
                // Hiển thị trạng thái đang tải
                lblStatus.Text = "Đang tải dữ liệu báo cáo doanh thu...";
                prgLoading.Visible = true;
                prgLoading.Style = ProgressBarStyle.Marquee;

                // Lấy giá trị từ controls
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;

                // Lấy doanh thu theo ngày
                var dailyRevenue = await _reportService.GetDailyRevenueAsync(fromDate, toDate);
                dgvDailyRevenue.DataSource = dailyRevenue;

                // Cập nhật trạng thái
                lblStatus.Text = "Đã tải xong dữ liệu báo cáo doanh thu.";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Lỗi khi tải dữ liệu báo cáo doanh thu: {ex.Message}");
                lblStatus.Text = "Đã xảy ra lỗi khi tải dữ liệu báo cáo doanh thu.";
            }
            finally
            {
                prgLoading.Visible = false;
            }
        }
    }
}