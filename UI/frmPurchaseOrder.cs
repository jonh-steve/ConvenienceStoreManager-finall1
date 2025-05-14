using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.BusinessLogic;
using ConvenienceStoreManager.Utils;
// Thư viện icon cho UI thêm cute - Bạn có thể thêm nếu đã cài đặt FontAwesome.Sharp
// using FontAwesome.Sharp;

namespace ConvenienceStoreManager.UI
{
    public partial class frmPurchaseOrder : Form
    {
        // Biến thành viên
        private ProductRepository productRepository;
        private PurchaseRepository purchaseRepository;
        private DataTable dtPurchaseDetails;
        private Product selectedProduct;

        /// <summary>
        /// Constructor - Khởi tạo form phiếu nhập hàng
        /// </summary>
        public frmPurchaseOrder()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            purchaseRepository = new PurchaseRepository();
            CreatePurchaseDetailsTable();
            InitNewPurchase();
            
            // 🌸 Style UI hồng hào dễ thương
            this.BackColor = System.Drawing.Color.MistyRose;
            btnAddToPurchase.BackColor = System.Drawing.Color.Pink;
            btnSavePurchase.BackColor = System.Drawing.Color.LightPink;
            btnNewPurchase.BackColor = System.Drawing.Color.HotPink;
            btnAddToPurchase.ForeColor = System.Drawing.Color.White;
            btnSavePurchase.ForeColor = System.Drawing.Color.White;
            btnNewPurchase.ForeColor = System.Drawing.Color.White;
            
            // Nếu muốn dùng icon FontAwesome cho nút (cần cài đặt FontAwesome.Sharp):
            // btnAddToPurchase.IconChar = IconChar.PlusCircle;
        }

        /// <summary>
        /// 📍 Tạo cấu trúc DataTable lưu trữ chi tiết phiếu nhập
        /// </summary>
        private void CreatePurchaseDetailsTable()
        {
            // Xóa nguồn dữ liệu cũ nếu có
            dgvPurchaseDetails.DataSource = null;
            dgvPurchaseDetails.Columns.Clear();

            // Tạo DataTable mới
            dtPurchaseDetails = new DataTable();
            dtPurchaseDetails.Columns.Add("ProductID", typeof(int));
            dtPurchaseDetails.Columns.Add("ProductName", typeof(string));
            dtPurchaseDetails.Columns.Add("Unit", typeof(string));
            dtPurchaseDetails.Columns.Add("Quantity", typeof(int));
            dtPurchaseDetails.Columns.Add("PurchasePrice", typeof(decimal));
            dtPurchaseDetails.Columns.Add("Subtotal", typeof(decimal));
            dtPurchaseDetails.Columns.Add("Remove", typeof(string)); // Cột giả để hiển thị nút xóa

            // Gán DataTable làm nguồn dữ liệu cho DataGridView
            dgvPurchaseDetails.DataSource = dtPurchaseDetails;

            // Thêm cột nút xóa (button) nếu chưa có
            if (dgvPurchaseDetails.Columns["btnRemove"] == null)
            {
                DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn
                {
                    Name = "btnRemove",
                    HeaderText = "",
                    Text = "🗑️ Xóa",
                    UseColumnTextForButtonValue = true,
                    Width = 60
                };
                dgvPurchaseDetails.Columns.Add(btnRemove);
            }

            // Ẩn cột ID và cột giả
            if (dgvPurchaseDetails.Columns["ProductID"] != null)
                dgvPurchaseDetails.Columns["ProductID"].Visible = false;
            if (dgvPurchaseDetails.Columns["Remove"] != null)
                dgvPurchaseDetails.Columns["Remove"].Visible = false;

            // Định dạng các cột số
            if (dgvPurchaseDetails.Columns["Quantity"] != null)
                dgvPurchaseDetails.Columns["Quantity"].DefaultCellStyle.Format = "N0";
            if (dgvPurchaseDetails.Columns["PurchasePrice"] != null)
                dgvPurchaseDetails.Columns["PurchasePrice"].DefaultCellStyle.Format = "N0";
            if (dgvPurchaseDetails.Columns["Subtotal"] != null)
                dgvPurchaseDetails.Columns["Subtotal"].DefaultCellStyle.Format = "N0";

            // Đặt tiêu đề các cột
            if (dgvPurchaseDetails.Columns["ProductName"] != null)
                dgvPurchaseDetails.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            if (dgvPurchaseDetails.Columns["Unit"] != null)
                dgvPurchaseDetails.Columns["Unit"].HeaderText = "ĐVT";
            if (dgvPurchaseDetails.Columns["Quantity"] != null)
                dgvPurchaseDetails.Columns["Quantity"].HeaderText = "Số lượng";
            if (dgvPurchaseDetails.Columns["PurchasePrice"] != null)
                dgvPurchaseDetails.Columns["PurchasePrice"].HeaderText = "Giá nhập";
            if (dgvPurchaseDetails.Columns["Subtotal"] != null)
                dgvPurchaseDetails.Columns["Subtotal"].HeaderText = "Thành tiền";

            // 🌸 Style DataGridView cho dễ thương hồng hào
            ApplyCutePinkStyle(dgvPurchaseDetails);
        }
        // Hàm style DataGridView hồng hào dễ thương
        private void ApplyCutePinkStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = System.Drawing.Color.MistyRose;
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.LavenderBlush;
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Pink;
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Comic Sans MS", 10, System.Drawing.FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Pink;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Comic Sans MS", 11, System.Drawing.FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = System.Drawing.Color.HotPink;
            dgv.RowHeadersVisible = false;
        }
        // Khởi tạo phiếu nhập mới
        private void InitNewPurchase()
        {
            txtOrderID.Text = "Chưa có";
            dtpOrderDate.Value = DateTime.Now;
            txtSupplier.Text = string.Empty;
            txtSearch.Text = string.Empty;
            numQuantity.Value = 1;
            numPurchasePrice.Value = 0;
            txtTotalAmount.Text = "0";
            txtNotes.Text = string.Empty;
            selectedProduct = null;

            dtPurchaseDetails.Clear();
            dgvProductSearch.DataSource = null;
        }

        // Tải danh sách sản phẩm
        private void LoadProducts()
        {
            try
            {
                List<Product> products = productRepository.GetAllProducts();
                dgvProductSearch.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
            }
        }

        // Tìm kiếm sản phẩm theo từ khóa
        private void SearchProducts()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    LoadProducts();
                    return;
                }

                List<Product> products = productRepository.SearchProducts(keyword);
                dgvProductSearch.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
            }
        }

        // Thêm sản phẩm vào phiếu nhập
        private void AddProductToPurchase()
        {
            if (selectedProduct == null)
            {
                MessageHelper.ShowWarning("Vui lòng chọn sản phẩm trước khi thêm vào phiếu nhập!");
                return;
            }

            int quantity = (int)numQuantity.Value;
            decimal purchasePrice = numPurchasePrice.Value;

            if (quantity <= 0)
            {
                MessageHelper.ShowWarning("Số lượng phải lớn hơn 0!");
                return;
            }

            if (purchasePrice <= 0)
            {
                MessageHelper.ShowWarning("Giá nhập phải lớn hơn 0!");
                return;
            }

            // Kiểm tra xem sản phẩm đã có trong phiếu nhập chưa
            bool productExists = false;
            foreach (DataRow row in dtPurchaseDetails.Rows)
            {
                if ((int)row["ProductID"] == selectedProduct.ProductID)
                {
                    if (MessageHelper.ShowConfirmation($"Sản phẩm {selectedProduct.ProductName} đã có trong phiếu nhập. Bạn có muốn cập nhật không?") == DialogResult.Yes)
                    {
                        // Cập nhật số lượng, giá nhập và thành tiền
                        row["Quantity"] = quantity;
                        row["PurchasePrice"] = purchasePrice;
                        row["Subtotal"] = quantity * purchasePrice;
                    }
                    productExists = true;
                    break;
                }
            }

            // Nếu sản phẩm chưa có trong phiếu nhập, thêm mới
            if (!productExists)
            {
                DataRow newRow = dtPurchaseDetails.NewRow();
                newRow["ProductID"] = selectedProduct.ProductID;
                newRow["ProductName"] = selectedProduct.ProductName;
                newRow["Unit"] = selectedProduct.Unit;
                newRow["Quantity"] = quantity;
                newRow["PurchasePrice"] = purchasePrice;
                newRow["Subtotal"] = quantity * purchasePrice;
                dtPurchaseDetails.Rows.Add(newRow);
            }

            CalculateTotalAmount();
            numQuantity.Value = 1;
            numPurchasePrice.Value = 0;
            selectedProduct = null;
        }

        /// <summary>
        /// 📍 Tính toán tổng tiền phiếu nhập
        /// </summary>
        private void CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (DataRow row in dtPurchaseDetails.Rows)
            {
                total += Convert.ToDecimal(row["Subtotal"]);
            }
            txtTotalAmount.Text = total.ToString("N0");
        }

        // Lưu phiếu nhập và cập nhật tồn kho
        private void  SavePurchase()
        {
            if (dtPurchaseDetails.Rows.Count == 0)
            {
                MessageHelper.ShowWarning("Phiếu nhập chưa có sản phẩm nào!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSupplier.Text))
            {
                if (MessageHelper.ShowConfirmation("Bạn chưa nhập tên nhà cung cấp. Vẫn tiếp tục?") == DialogResult.No)
                {
                    txtSupplier.Focus();
                    return;
                }
            }

            try
            {
                // Tạo đối tượng phiếu nhập
                PurchaseOrder purchase = new PurchaseOrder
                {
                    OrderDate = dtpOrderDate.Value,
                    SupplierName = txtSupplier.Text.Trim(),
                    TotalAmount = decimal.Parse(txtTotalAmount.Text.Replace(",", "")),
                    Notes = txtNotes.Text.Trim()
                };

                // Tạo danh sách chi tiết phiếu nhập
                List<PurchaseOrderDetail> details = new List<PurchaseOrderDetail>();
                foreach (DataRow row in dtPurchaseDetails.Rows)
                {
                    PurchaseOrderDetail detail = new PurchaseOrderDetail
                    {
                        ProductID = (int)row["ProductID"],
                        Quantity = (int)row["Quantity"],
                        PurchasePrice = (decimal)row["PurchasePrice"],
                        Subtotal = (decimal)row["Subtotal"]
                    };
                    details.Add(detail);
                }

                // Lưu phiếu nhập và cập nhật tồn kho
                purchaseRepository.CreatePurchaseOrder(purchase, details);
                MessageHelper.ShowSuccess("Đã lưu phiếu nhập thành công!");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDatabaseError(ex);
            }
        }

        // Xử lý sự kiện khi form được tải
        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // Xử lý sự kiện khi click nút tìm kiếm
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchProducts();
        }

        // Xử lý sự kiện khi nhấn Enter trong ô tìm kiếm
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Ngăn không cho phát ra tiếng "beep"
                SearchProducts();
            }
        }

        // Xử lý sự kiện khi click vào một dòng trong danh sách sản phẩm
        private void dgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductSearch.Rows[e.RowIndex];
                int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                selectedProduct = new Product
                {
                    ProductID = productID,
                    ProductName = row.Cells["ProductName"].Value.ToString(),
                    Unit = row.Cells["Unit"].Value.ToString(),
                    SellingPrice = Convert.ToDecimal(row.Cells["SellingPrice"].Value),
                    StockQuantity = Convert.ToInt32(row.Cells["StockQuantity"].Value)
                };

                // Có thể đặt giá nhập mặc định bằng giá bán hoặc để trống
                numPurchasePrice.Value = selectedProduct.SellingPrice * 0.8m; // Ví dụ: giá nhập = 80% giá bán
            }
        }

        // Xử lý sự kiện khi click nút thêm vào phiếu nhập
        private void btnAddToPurchase_Click(object sender, EventArgs e)
        {
            AddProductToPurchase();
        }

        // Xử lý sự kiện khi click nút lưu phiếu nhập
        private void btnSavePurchase_Click(object sender, EventArgs e)
        {
            SavePurchase();
        }

        // Xử lý sự kiện khi click nút tạo phiếu nhập mới
        private void btnNewPurchase_Click(object sender, EventArgs e)
        {
            // Hỏi xác nhận nếu đang có phiếu nhập chưa lưu
            if (dtPurchaseDetails.Rows.Count > 0)
            {
                if (MessageHelper.ShowConfirmation("Bạn có chắc muốn tạo phiếu nhập mới? Phiếu nhập hiện tại sẽ bị hủy!") == DialogResult.Yes)
                {
                    InitNewPurchase();
                }
            }
            else
            {
                InitNewPurchase();
            }
        }

        /// <summary>
        /// 📍 Xử lý sự kiện khi click nút xóa sản phẩm khỏi phiếu nhập
        /// </summary>
        private void dgvPurchaseDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvPurchaseDetails.Columns["btnRemove"].Index)
            {
                if (MessageHelper.ShowConfirmation("Bạn có chắc muốn xóa sản phẩm này khỏi phiếu nhập?") == DialogResult.Yes)
                {
                    dtPurchaseDetails.Rows.RemoveAt(e.RowIndex);
                    CalculateTotalAmount();
                    dgvPurchaseDetails.Refresh();
                    MessageHelper.ShowSuccess("Đã xóa sản phẩm khỏi phiếu nhập!");
                }
            }
        }

        /// <summary>
        /// 🌸 Ghi chú sửa lỗi tương tự cho các file khác:
        /// - Khi muốn thêm nút xóa vào DataGridView, hãy tạo DataGridViewButtonColumn và kiểm tra nếu đã tồn tại chưa.
        /// - Khi xử lý sự kiện CellClick, nhớ kiểm tra đúng tên cột ("btnRemove") và chỉ xóa dòng khi xác nhận.
        /// - Để style dễ thương, hãy dùng màu hồng cho các control, có thể dùng FontAwesome icon cho các nút.
        /// - Nếu muốn ẩn cột giả (ví dụ "Remove"), hãy đặt Visible = false.
        /// - Khi tính tổng tiền, nhớ dùng Convert.ToDecimal để tránh lỗi ép kiểu.
        /// </summary>
    }
}
