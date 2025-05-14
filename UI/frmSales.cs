using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;
using ConvenienceStoreManager.Utils;
using FontAwesome.Sharp;

namespace ConvenienceStoreManager.UI
{
    public partial class frmSales : Form
    {
        private ProductRepository productRepository;
        private SalesRepository salesRepository;
        private DataTable dtInvoiceDetails;
        private Product selectedProduct;
        private Label lblCuteMessage;

        public frmSales()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            salesRepository = new SalesRepository();
            
            // Thêm label thông báo dễ thương
            lblCuteMessage = new Label();
            lblCuteMessage.AutoSize = true;
            lblCuteMessage.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lblCuteMessage.ForeColor = Color.HotPink;
            lblCuteMessage.Location = new Point(20, this.Height - 70);
            lblCuteMessage.Text = "";
            this.Controls.Add(lblCuteMessage);
            
            // Thay đổi nút thanh toán thành IconButton
            if (btnSaveInvoice is Button originalButton)
            {
                // Lưu vị trí và kích thước của nút cũ
                Point location = originalButton.Location;
                Size size = originalButton.Size;
                
                // Xóa nút cũ
                this.Controls.Remove(originalButton);
                
                // Tạo IconButton mới
                IconButton newButton = new IconButton();
                newButton.Name = "btnSaveInvoice";
                newButton.IconChar = IconChar.Heart;
                newButton.IconColor = Color.HotPink;
                newButton.IconSize = 32;
                newButton.Text = "Thanh toán";
                newButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                newButton.BackColor = Color.LightPink;
                newButton.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold);
                newButton.Location = location;
                newButton.Size = size;
                newButton.Click += btnSaveInvoice_Click;
                
                // Thêm nút mới vào form
                this.Controls.Add(newButton);
                btnSaveInvoice = newButton;
            }
            
            InitNewInvoice();
        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            LoadProducts();
            dtpInvoiceDate.Value = DateTime.Now;
        }

        private void InitNewInvoice()
        {
            // Tạo DataTable chứa thông tin chi tiết hóa đơn
            CreateInvoiceDetailsTable();

            // Đặt DataSource cho DataGridView
            dgvInvoiceDetails.DataSource = dtInvoiceDetails;

            // Định dạng cột DataGridView
            FormatInvoiceDetailsColumns();

            // Đặt lại các trường nhập liệu
            txtProductSearch.Clear();
            txtQuantity.Text = "1";
            txtProductName.Clear();
            txtProductUnit.Clear();
            txtProductPrice.Clear();
            txtTotalAmount.Text = "0";
            txtNotes.Clear();

            // Đặt lại selectedProduct
            selectedProduct = null;
            
            // Xóa thông báo dễ thương
            lblCuteMessage.Text = "";
        }

        private void CreateInvoiceDetailsTable()
        {
            dtInvoiceDetails = new DataTable();
            dtInvoiceDetails.Columns.Add("ProductID", typeof(int));
            dtInvoiceDetails.Columns.Add("ProductName", typeof(string));
            dtInvoiceDetails.Columns.Add("Unit", typeof(string));
            dtInvoiceDetails.Columns.Add("Quantity", typeof(int));
            dtInvoiceDetails.Columns.Add("UnitPrice", typeof(decimal));
            dtInvoiceDetails.Columns.Add("Subtotal", typeof(decimal));
        }

        private void FormatInvoiceDetailsColumns()
        {
            // Đặt tên hiển thị cho các cột
            dgvInvoiceDetails.Columns["ProductID"].HeaderText = "Mã SP";
            dgvInvoiceDetails.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvInvoiceDetails.Columns["Unit"].HeaderText = "ĐVT";
            dgvInvoiceDetails.Columns["Quantity"].HeaderText = "Số lượng";
            dgvInvoiceDetails.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dgvInvoiceDetails.Columns["Subtotal"].HeaderText = "Thành tiền";

            // Đặt độ rộng cột
            dgvInvoiceDetails.Columns["ProductID"].Width = 60;
            dgvInvoiceDetails.Columns["ProductName"].Width = 200;
            dgvInvoiceDetails.Columns["Unit"].Width = 60;
            dgvInvoiceDetails.Columns["Quantity"].Width = 80;
            dgvInvoiceDetails.Columns["UnitPrice"].Width = 100;
            dgvInvoiceDetails.Columns["Subtotal"].Width = 120;

            // Định dạng số
            dgvInvoiceDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            dgvInvoiceDetails.Columns["Subtotal"].DefaultCellStyle.Format = "N0";

            // Căn chỉnh cột
            dgvInvoiceDetails.Columns["ProductID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoiceDetails.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoiceDetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoiceDetails.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void LoadProducts()
        {
            try
            {
                // Lấy tất cả sản phẩm để hiển thị trong DataGridView tìm kiếm
                List<Product> products = productRepository.GetAllProducts();

                // Tạo DataTable từ danh sách sản phẩm
                DataTable dtProducts = new DataTable();
                dtProducts.Columns.Add("ProductID", typeof(int));
                dtProducts.Columns.Add("ProductCode", typeof(string));
                dtProducts.Columns.Add("ProductName", typeof(string));
                dtProducts.Columns.Add("Unit", typeof(string));
                dtProducts.Columns.Add("SellingPrice", typeof(decimal));
                dtProducts.Columns.Add("StockQuantity", typeof(int));

                foreach (var product in products)
                {
                    dtProducts.Rows.Add(
                        product.ProductID,
                        product.ProductCode,
                        product.ProductName,
                        product.Unit,
                        product.SellingPrice,
                        product.StockQuantity
                    );
                }

                // Gán DataTable làm DataSource cho DataGridView sản phẩm
                dgvProducts.DataSource = dtProducts;

                // Định dạng cột DataGridView
                FormatProductColumns();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Lỗi khi tải danh sách sản phẩm: " + ex.Message);
            }
        }

        private void FormatProductColumns()
        {
            // Đặt tên hiển thị cho các cột
            dgvProducts.Columns["ProductID"].HeaderText = "Mã SP";
            dgvProducts.Columns["ProductCode"].HeaderText = "Mã vạch";
            dgvProducts.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvProducts.Columns["Unit"].HeaderText = "ĐVT";
            dgvProducts.Columns["SellingPrice"].HeaderText = "Giá bán";
            dgvProducts.Columns["StockQuantity"].HeaderText = "Tồn kho";

            // Đặt độ rộng cột
            dgvProducts.Columns["ProductID"].Width = 60;
            dgvProducts.Columns["ProductCode"].Width = 100;
            dgvProducts.Columns["ProductName"].Width = 200;
            dgvProducts.Columns["Unit"].Width = 60;
            dgvProducts.Columns["SellingPrice"].Width = 100;
            dgvProducts.Columns["StockQuantity"].Width = 80;

            // Định dạng số
            dgvProducts.Columns["SellingPrice"].DefaultCellStyle.Format = "N0";

            // Căn chỉnh cột
            dgvProducts.Columns["ProductID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns["SellingPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProducts.Columns["StockQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void SearchProducts()
        {
            string searchText = txtProductSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadProducts();
                return;
            }

            try
            {
                // Tìm kiếm sản phẩm dựa trên từ khóa
                List<Product> products = productRepository.SearchProducts(searchText);

                // Cập nhật DataTable
                DataTable dtProducts = (DataTable)dgvProducts.DataSource;
                dtProducts.Clear();

                foreach (var product in products)
                {
                    dtProducts.Rows.Add(
                        product.ProductID,
                        product.ProductCode,
                        product.ProductName,
                        product.Unit,
                        product.SellingPrice,
                        product.StockQuantity
                    );
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click vào header hoặc các vùng không phải là cell
            if (e.RowIndex < 0)
                return;

            try
            {
                // Lấy ID sản phẩm từ DataGridView
                int productId = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["ProductID"].Value);

                // Lấy thông tin chi tiết sản phẩm từ repository
                selectedProduct = productRepository.GetProductById(productId);

                if (selectedProduct != null)
                {
                    // Hiển thị thông tin sản phẩm
                    txtProductName.Text = selectedProduct.ProductName;
                    txtProductUnit.Text = selectedProduct.Unit;
                    txtProductPrice.Text = selectedProduct.SellingPrice.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Lỗi khi chọn sản phẩm: " + ex.Message);
                selectedProduct = null;
            }
        }

        private void btnAddToInvoice_Click(object sender, EventArgs e)
        {
            AddProductToInvoice();
        }

        private void AddProductToInvoice()
        {
            // Kiểm tra sản phẩm đã được chọn chưa
            if (selectedProduct == null)
            {
                MessageHelper.ShowWarning("Vui lòng chọn sản phẩm trước khi thêm vào hóa đơn!");
                return;
            }

            // Kiểm tra và lấy số lượng
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageHelper.ShowWarning("Số lượng phải là số nguyên dương!");
                txtQuantity.Focus();
                return;
            }

            // Kiểm tra tồn kho
            if (quantity > selectedProduct.StockQuantity)
            {
                MessageHelper.ShowWarning($"Số lượng tồn kho không đủ! Hiện chỉ còn {selectedProduct.StockQuantity} {selectedProduct.Unit}.");
                return;
            }

            // Kiểm tra sản phẩm đã có trong hóa đơn chưa
            bool productExistsInInvoice = false;
            foreach (DataRow row in dtInvoiceDetails.Rows)
            {
                if (Convert.ToInt32(row["ProductID"]) == selectedProduct.ProductID)
                {
                    // Nếu sản phẩm đã có, cập nhật số lượng và thành tiền
                    int currentQuantity = Convert.ToInt32(row["Quantity"]);
                    int newQuantity = currentQuantity + quantity;

                    // Kiểm tra tồn kho với số lượng mới
                    if (newQuantity > selectedProduct.StockQuantity)
                    {
                        MessageHelper.ShowWarning($"Số lượng tồn kho không đủ! Hiện chỉ còn {selectedProduct.StockQuantity} {selectedProduct.Unit}.");
                        return;
                    }

                    row["Quantity"] = newQuantity;
                    row["Subtotal"] = Convert.ToDecimal(row["UnitPrice"]) * newQuantity;
                    productExistsInInvoice = true;
                    break;
                }
            }

            // Nếu sản phẩm chưa có trong hóa đơn, thêm mới
            if (!productExistsInInvoice)
            {
                decimal subtotal = selectedProduct.SellingPrice * quantity;
                dtInvoiceDetails.Rows.Add(
                    selectedProduct.ProductID,
                    selectedProduct.ProductName,
                    selectedProduct.Unit,
                    quantity,
                    selectedProduct.SellingPrice,
                    subtotal
                );
            }

            // Cập nhật tổng tiền hóa đơn
            CalculateTotalAmount();

            // Đặt lại thông tin sản phẩm
            txtProductSearch.Clear();
            txtQuantity.Text = "1";
            txtProductName.Clear();
            txtProductUnit.Clear();
            txtProductPrice.Clear();
            selectedProduct = null;

            // Focus vào ô tìm kiếm để tiếp tục thêm sản phẩm
            txtProductSearch.Focus();
        }

        private void dgvInvoiceDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu click vào header hoặc các vùng không phải là cell
            if (e.RowIndex < 0)
                return;

            // Lấy ProductID từ dòng được chọn
            int productId = Convert.ToInt32(dgvInvoiceDetails.Rows[e.RowIndex].Cells["ProductID"].Value);

            // Hiển thị thông tin chi tiết của sản phẩm được chọn (nếu cần)
            // ...
        }

        private void btnRemoveFromInvoice_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn sản phẩm trong hóa đơn chưa
            if (dgvInvoiceDetails.SelectedRows.Count == 0 && dgvInvoiceDetails.SelectedCells.Count == 0)
            {
                MessageHelper.ShowWarning("Vui lòng chọn sản phẩm cần xóa khỏi hóa đơn!");
                return;
            }

            // Lấy chỉ số dòng được chọn
            int rowIndex;
            if (dgvInvoiceDetails.SelectedRows.Count > 0)
                rowIndex = dgvInvoiceDetails.SelectedRows[0].Index;
            else
                rowIndex = dgvInvoiceDetails.SelectedCells[0].RowIndex;

            // Xác nhận xóa
            DialogResult result = MessageHelper.ShowDeleteConfirmation("sản phẩm này khỏi hóa đơn");
            if (result == DialogResult.Yes)
            {
                // Xóa dòng được chọn
                dtInvoiceDetails.Rows.RemoveAt(rowIndex);

                // Cập nhật tổng tiền hóa đơn
                CalculateTotalAmount();
            }
        }

        private void CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataRow row in dtInvoiceDetails.Rows)
            {
                totalAmount += Convert.ToDecimal(row["Subtotal"]);
            }
            txtTotalAmount.Text = totalAmount.ToString("N0");
        }

        private void btnNewInvoice_Click(object sender, EventArgs e)
        {
            if (dtInvoiceDetails.Rows.Count > 0)
            {
                DialogResult result = MessageHelper.ShowConfirmation("Bạn có chắc muốn tạo hóa đơn mới? Hóa đơn hiện tại sẽ bị hủy!");
                if (result == DialogResult.Yes)
                {
                    InitNewInvoice();
                }
            }
            else
            {
                InitNewInvoice();
            }
        }

        
//
// File: UI/frmSales.cs
// Vị trí sửa: Thay thế hàm btnSaveInvoice_Click và SaveInvoice
//

// Sử dụng async/await để tránh block UI
private async void btnSaveInvoice_Click(object sender, EventArgs e)
{
    await SaveInvoiceAsync();
}

// Hàm lưu hóa đơn dễ thương, không block UI, có thông báo màu hồng
private async Task SaveInvoiceAsync()
{
    // Kiểm tra hóa đơn có sản phẩm không
    if (dtInvoiceDetails.Rows.Count == 0)
    {
        MessageHelper.ShowWarning("Không thể lưu hóa đơn trống. Vui lòng thêm sản phẩm vào hóa đơn!");
        lblCuteMessage.Text = "🌸 Hãy thêm sản phẩm vào hóa đơn nhé bé yêu! 🌸";
        lblCuteMessage.ForeColor = Color.HotPink;
        return;
    }

    try
    {
        // Tạo đối tượng Invoice
        SalesInvoice invoice = new SalesInvoice
        {
            InvoiceDate = dtpInvoiceDate.Value,
            // Xử lý lỗi parse số tiền
            TotalAmount = decimal.TryParse(txtTotalAmount.Text.Replace(",", ""), out decimal total) ? total : 0,
            Notes = txtNotes.Text
        };

        // Tạo danh sách chi tiết hóa đơn
        List<SalesInvoiceDetail> details = new List<SalesInvoiceDetail>();
        foreach (DataRow row in dtInvoiceDetails.Rows)
        {
            SalesInvoiceDetail detail = new SalesInvoiceDetail
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                Quantity = Convert.ToInt32(row["Quantity"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                Subtotal = Convert.ToDecimal(row["Subtotal"])
            };
            details.Add(detail);
        }

        // Chạy thao tác lưu ở background để không block UI
        int invoiceId = await Task.Run(() => salesRepository.CreateInvoice(invoice, details));

        if (invoiceId > 0)
        {
            // Thông báo thành công dễ thương
            MessageHelper.ShowSuccess($"💖 Hóa đơn đã được thanh toán thành công với mã {invoiceId}! 💖");
            lblCuteMessage.Text = $"💖 Bé yêu ơi, hóa đơn {invoiceId} đã lưu thành công! 💖";
            lblCuteMessage.ForeColor = Color.HotPink;

            // Làm mới giao diện để tạo hóa đơn mới
            InitNewInvoice();

            // Tải lại danh sách sản phẩm để cập nhật số lượng tồn kho mới
            LoadProducts();

            // Focus vào ô tìm kiếm sản phẩm để sẵn sàng cho hóa đơn tiếp theo
            txtProductSearch.Focus();
        }
        else
        {
            MessageHelper.ShowError("Lỗi khi thanh toán hóa đơn! Vui lòng thử lại.");
            lblCuteMessage.Text = "😢 Có lỗi khi lưu hóa đơn, thử lại nhé bé yêu! 😢";
            lblCuteMessage.ForeColor = Color.HotPink;
        }
    }
    catch (Exception ex)
    {
        MessageHelper.ShowError("Lỗi khi thanh toán hóa đơn: " + ex.Message);
        lblCuteMessage.Text = "😢 Có lỗi khi lưu hóa đơn, thử lại nhé bé yêu! 😢";
        lblCuteMessage.ForeColor = Color.HotPink;
    }
}


        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
