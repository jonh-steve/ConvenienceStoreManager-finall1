using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ConvenienceStoreManager.DataAccess;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.UI
{
    public partial class frmProductManagement : Form
    {
        private ProductRepository productRepository;
        private bool isAdding = false;
        private bool isEditing = false;

        public frmProductManagement()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
        }

        private void frmProductManagement_Load(object sender, EventArgs e)
        {
            LoadProducts();
            SetControlState(false);
        }

        /// <summary>
        /// Tải danh sách sản phẩm và hiển thị lên DataGridView
        /// </summary>
        private void LoadProducts()
        {
            List<Product> products = productRepository.GetAllProducts();
            dgvProducts.DataSource = products;

            // Định dạng lại các cột trong DataGridView
            dgvProducts.Columns["ProductID"].HeaderText = "Mã SP";
            dgvProducts.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvProducts.Columns["ProductCode"].HeaderText = "Mã vạch";
            dgvProducts.Columns["Unit"].HeaderText = "Đơn vị tính";
            dgvProducts.Columns["SellingPrice"].HeaderText = "Giá bán";
            dgvProducts.Columns["SellingPrice"].DefaultCellStyle.Format = "N0";
            dgvProducts.Columns["StockQuantity"].HeaderText = "Tồn kho";

            // Đặt lại độ rộng của các cột
            dgvProducts.Columns["ProductID"].Width = 80;
            dgvProducts.Columns["ProductName"].Width = 200;
            dgvProducts.Columns["ProductCode"].Width = 100;
            dgvProducts.Columns["Unit"].Width = 80;
            dgvProducts.Columns["SellingPrice"].Width = 100;
            dgvProducts.Columns["StockQuantity"].Width = 80;
        }

        /// <summary>
        /// Đặt trạng thái cho các control trên form
        /// </summary>
        private void SetControlState(bool isEditable)
        {
            // Các control nhập liệu
            txtProductName.Enabled = isEditable;
            txtProductCode.Enabled = isEditable;
            txtUnit.Enabled = isEditable;
            nudSellingPrice.Enabled = isEditable;
            nudStockQuantity.Enabled = isEditable;

            // Các nút chức năng
            btnAdd.Enabled = !isEditable;
            btnEdit.Enabled = !isEditable && dgvProducts.SelectedRows.Count > 0;
            btnDelete.Enabled = !isEditable && dgvProducts.SelectedRows.Count > 0;
            btnSave.Enabled = isEditable;
            btnCancel.Enabled = isEditable;

            // DataGridView
            dgvProducts.Enabled = !isEditable;
        }

        /// <summary>
        /// Xóa trắng các control nhập liệu
        /// </summary>
        private void ClearInputs()
        {
            txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtUnit.Text = string.Empty;
            nudSellingPrice.Value = 0;
            nudStockQuantity.Value = 0;
        }

        /// <summary>
        /// Hiển thị thông tin sản phẩm được chọn lên các control nhập liệu
        /// </summary>
        private void DisplaySelectedProduct()
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProducts.SelectedRows[0];
                txtProductID.Text = row.Cells["ProductID"].Value.ToString();
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtProductCode.Text = row.Cells["ProductCode"].Value?.ToString() ?? string.Empty;
                txtUnit.Text = row.Cells["Unit"].Value?.ToString() ?? string.Empty;
                nudSellingPrice.Value = Convert.ToDecimal(row.Cells["SellingPrice"].Value);
                nudStockQuantity.Value = Convert.ToInt32(row.Cells["StockQuantity"].Value);
            }
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu nhập
        /// </summary>
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return false;
            }

            if (nudSellingPrice.Value <= 0)
            {
                MessageBox.Show("Giá bán phải lớn hơn 0", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudSellingPrice.Focus();
                return false;
            }

            if (nudStockQuantity.Value < 0)
            {
                MessageBox.Show("Số lượng tồn kho không được âm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudStockQuantity.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lưu thông tin sản phẩm (thêm mới hoặc cập nhật)
        /// </summary>
        private void SaveProduct()
        {
            if (!ValidateInputs())
            {
                return;
            }

            Product product = new Product
            {
                ProductName = txtProductName.Text.Trim(),
                ProductCode = string.IsNullOrWhiteSpace(txtProductCode.Text) ? null : txtProductCode.Text.Trim(),
                Unit = string.IsNullOrWhiteSpace(txtUnit.Text) ? null : txtUnit.Text.Trim(),
                SellingPrice = nudSellingPrice.Value,
                StockQuantity = (int)nudStockQuantity.Value
            };

            int result;

            if (isAdding)
            {
                result = productRepository.AddProduct(product);

                if (result > 0)
                {
                    MessageBox.Show("Thêm sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == -1)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // isEditing
            {
                product.ProductID = int.Parse(txtProductID.Text);
                bool results = productRepository.UpdateProduct(product);

                if ( results )
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (results == false)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại với sản phẩm khác", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Cập nhật sản phẩm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Đặt lại trạng thái
            isAdding = false;
            isEditing = false;
            SetControlState(false);

            // Tải lại danh sách sản phẩm
            LoadProducts();
        }

        #region Event Handlers

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            DisplaySelectedProduct();

            btnEdit.Enabled = dgvProducts.SelectedRows.Count > 0;
            btnDelete.Enabled = dgvProducts.SelectedRows.Count > 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAdding = true;
            ClearInputs();
            SetControlState(true);
            txtProductName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                isEditing = true;
                SetControlState(true);
                txtProductName.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
                string productName = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{productName}'?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool deleteResult = productRepository.DeleteProduct(productID);

                    if (deleteResult)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    else if (deleteResult == false)
                    {
                        MessageBox.Show("Không thể xóa sản phẩm này vì đã có trong hóa đơn hoặc phiếu nhập",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveProduct();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isAdding = false;
            isEditing = false;
            SetControlState(false);

            if (dgvProducts.SelectedRows.Count > 0)
            {
                DisplaySelectedProduct();
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                List<Product> searchResults = productRepository.SearchProducts(txtSearch.Text.Trim());
                dgvProducts.DataSource = searchResults;
            }
            else
            {
                LoadProducts();
            }
        }

        #endregion
    }
}