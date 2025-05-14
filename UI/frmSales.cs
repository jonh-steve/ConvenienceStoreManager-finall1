// File: frmSales.cs
// Vị trí: ConvenienceStoreManager/UI/frmSales.cs

using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvenienceStoreManager.BusinessLogic;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.Utils;

namespace ConvenienceStoreManager.UI
{
    public partial class frmSales : Form
    {
        private readonly ProductRepository productRepository;
        private readonly SalesService salesService;
        private DataTable dtInvoiceDetails;
        private decimal totalAmount;

        public frmSales()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            salesService = new SalesService(new SalesRepository(), productRepository);
            InitializeInvoice();
        }

        /// <summary>
        /// Khởi tạo hóa đơn mới, thiết lập DataTable và các control.
        /// </summary>
        private void InitializeInvoice()
        {
            dtpInvoiceDate.Value = DateTime.Now;
            totalAmount = 0;
            lblTotalAmount.Text = "0.00";
            txtNotes.Clear();
            CreateInvoiceDetailsTable();
        }

        /// <summary>
        /// Tạo cấu trúc DataTable để lưu trữ chi tiết hóa đơn và cấu hình DataGridView.
        /// </summary>
        private void CreateInvoiceDetailsTable()
        {
            dtInvoiceDetails = new DataTable();
            dtInvoiceDetails.Columns.Add("ProductID", typeof(int));
            dtInvoiceDetails.Columns.Add("ProductName", typeof(string));
            dtInvoiceDetails.Columns.Add("Unit", typeof(string));
            dtInvoiceDetails.Columns.Add("Quantity", typeof(int));
            dtInvoiceDetails.Columns.Add("UnitPrice", typeof(decimal));
            dtInvoiceDetails.Columns.Add("Subtotal", typeof(decimal));
            dtInvoiceDetails.Columns.Add("Remove", typeof(string)); // Cột giả cho nút xóa

            dgvInvoiceDetails.DataSource = dtInvoiceDetails;

            // Thêm cột nút xóa
            if (!dgvInvoiceDetails.Columns.Contains("btnRemove"))
            {
                DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn
                {
                    Name = "btnRemove",
                    HeaderText = "",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true
                };
                dgvInvoiceDetails.Columns.Add(btnRemove);
            }

            // Định dạng các cột
            dgvInvoiceDetails.Columns["ProductID"].Visible = false;
            dgvInvoiceDetails.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvInvoiceDetails.Columns["Unit"].HeaderText = "Đơn vị";
            dgvInvoiceDetails.Columns["Quantity"].HeaderText = "Số lượng";
            dgvInvoiceDetails.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dgvInvoiceDetails.Columns["Subtotal"].HeaderText = "Thành tiền";
            dgvInvoiceDetails.Columns["Remove"].Visible = false;
            dgvInvoiceDetails.Columns["btnRemove"].Width = 60;
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút tìm kiếm sản phẩm.
        /// </summary>
        private async void btnSearchProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtProductSearch.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageHelper.ShowWarning("Vui lòng nhập mã hoặc tên sản phẩm để tìm kiếm!");
                    return;
                }

                var products = await productRepository.SearchProductsAsync(keyword);
                DataTable dtProducts = new DataTable();
                dtProducts.Columns.Add("ProductID", typeof(int));
                dtProducts.Columns.Add("ProductName", typeof(string));
                dtProducts.Columns.Add("ProductCode", typeof(string));
                dtProducts.Columns.Add("Unit", typeof(string));
                dtProducts.Columns.Add("SellingPrice", typeof(decimal));
                dtProducts.Columns.Add("StockQuantity", typeof(int));

                foreach (var product in products)
                {
                    dtProducts.Rows.Add(
                        product.ProductID,
                        product.ProductName,
                        product.ProductCode,
                        product.Unit,
                        product.SellingPrice,
                        product.StockQuantity
                    );
                }

                dgvProductSearch.DataSource = dtProducts;
                dgvProductSearch.Columns["ProductID"].Visible = false;
                dgvProductSearch.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                dgvProductSearch.Columns["ProductCode"].HeaderText = "Mã sản phẩm";
                dgvProductSearch.Columns["Unit"].HeaderText = "Đơn vị";
                dgvProductSearch.Columns["SellingPrice"].HeaderText = "Giá bán";
                dgvProductSearch.Columns["StockQuantity"].HeaderText = "Tồn kho";
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút thêm sản phẩm vào hóa đơn.
        /// </summary>
        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductSearch.SelectedRows.Count == 0)
                {
                    MessageHelper.ShowWarning("Vui lòng chọn một sản phẩm để thêm!");
                    return;
                }

                if (!ValidationHelper.IsValidInteger(txtQuantity, "Số lượng", out int quantity))
                {
                    return;
                }

                if (!ValidationHelper.IsPositive(quantity, "Số lượng", txtQuantity))
                {
                    return;
                }

                var selectedRow = dgvProductSearch.SelectedRows[0];
                int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
                string productName = selectedRow.Cells["ProductName"].Value.ToString();
                string unit = selectedRow.Cells["Unit"].Value.ToString();
                decimal unitPrice = Convert.ToDecimal(selectedRow.Cells["SellingPrice"].Value);
                int stockQuantity = Convert.ToInt32(selectedRow.Cells["StockQuantity"].Value);

                if (quantity > stockQuantity)
                {
                    MessageHelper.ShowWarning($"Số lượng tồn kho của sản phẩm {productName} chỉ còn {stockQuantity}!");
                    return;
                }

                decimal subtotal = quantity * unitPrice;
                dtInvoiceDetails.Rows.Add(productId, productName, unit, quantity, unitPrice, subtotal);

                totalAmount += subtotal;
                lblTotalAmount.Text = totalAmount.ToString("N2");
                txtQuantity.Clear();
                dgvInvoiceDetails.Refresh();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn vào ô của DataGridView chi tiết hóa đơn.
        /// </summary>
        private void dgvInvoiceDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvInvoiceDetails.Columns["btnRemove"].Index)
            {
                if (MessageHelper.ShowDeleteConfirmation("sản phẩm khỏi hóa đơn"))
                {
                    decimal subtotal = Convert.ToDecimal(dtInvoiceDetails.Rows[e.RowIndex]["Subtotal"]);
                    totalAmount -= subtotal;
                    lblTotalAmount.Text = totalAmount.ToString("N2");

                    dtInvoiceDetails.Rows.RemoveAt(e.RowIndex);
                    dgvInvoiceDetails.Refresh();
                    MessageHelper.ShowDeleteSuccess("Sản phẩm");
                }
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút lưu hóa đơn.
        /// </summary>
        private async void btnSaveInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.HasRows(dgvInvoiceDetails, "Chi tiết hóa đơn"))
                {
                    return;
                }

                var invoice = new SalesInvoice
                {
                    InvoiceDate = dtpInvoiceDate.Value,
                    TotalAmount = totalAmount,
                    Notes = txtNotes.Text.Trim(),
                    Details = new System.Collections.Generic.List<SalesInvoiceDetail>()
                };

                foreach (DataRow row in dtInvoiceDetails.Rows)
                {
                    invoice.Details.Add(new SalesInvoiceDetail
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                        Subtotal = Convert.ToDecimal(row["Subtotal"]),
                        ProductName = row["ProductName"].ToString(),
                        Unit = row["Unit"].ToString()
                    });
                }

                await salesService.CreateInvoiceAsync(invoice);
                MessageHelper.ShowAddSuccess("Hóa đơn");
                InitializeInvoice();
                dgvProductSearch.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút hủy hóa đơn.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowCancelConfirmation())
            {
                InitializeInvoice();
                dgvProductSearch.DataSource = null;
            }
        }
    }
}