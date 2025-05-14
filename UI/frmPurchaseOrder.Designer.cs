namespace ConvenienceStoreManager.UI
{
    partial class frmPurchaseOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSupplier = new System.Windows.Forms.TextBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.dtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.lblOrderID = new System.Windows.Forms.Label();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlProductList = new System.Windows.Forms.Panel();
            this.dgvProductSearch = new System.Windows.Forms.DataGridView();
            this.colProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSellingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSearchProduct = new System.Windows.Forms.Panel();
            this.numPurchasePrice = new System.Windows.Forms.NumericUpDown();
            this.lblPurchasePrice = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.btnAddToPurchase = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlPurchaseDetails = new System.Windows.Forms.Panel();
            this.dgvPurchaseDetails = new System.Windows.Forms.DataGridView();
            this.colPurchaseDetailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPurchaseTotal = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.btnSavePurchase = new System.Windows.Forms.Button();
            this.btnNewPurchase = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.pnlTop.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).BeginInit();
            this.pnlSearchProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPurchasePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlPurchaseDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseDetails)).BeginInit();
            this.pnlPurchaseTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtSupplier);
            this.pnlTop.Controls.Add(this.lblSupplier);
            this.pnlTop.Controls.Add(this.lblOrderDate);
            this.pnlTop.Controls.Add(this.dtpOrderDate);
            this.pnlTop.Controls.Add(this.lblOrderID);
            this.pnlTop.Controls.Add(this.txtOrderID);
            this.pnlTop.Controls.Add(this.lblHeader);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 100);
            this.pnlTop.TabIndex = 0;
            // 
            // txtSupplier
            // 
            this.txtSupplier.Location = new System.Drawing.Point(680, 55);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(300, 30);
            this.txtSupplier.TabIndex = 6;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(550, 58);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(124, 25);
            this.lblSupplier.TabIndex = 5;
            this.lblSupplier.Text = "Nhà cung cấp:";
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(800, 18);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(60, 25);
            this.lblOrderDate.TabIndex = 4;
            this.lblOrderDate.Text = "Ngày:";
            // 
            // dtpOrderDate
            // 
            this.dtpOrderDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpOrderDate.Enabled = false;
            this.dtpOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpOrderDate.Location = new System.Drawing.Point(866, 15);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(300, 30);
            this.dtpOrderDate.TabIndex = 3;
            // 
            // lblOrderID
            // 
            this.lblOrderID.AutoSize = true;
            this.lblOrderID.Location = new System.Drawing.Point(550, 18);
            this.lblOrderID.Name = "lblOrderID";
            this.lblOrderID.Size = new System.Drawing.Size(119, 25);
            this.lblOrderID.TabIndex = 2;
            this.lblOrderID.Text = "Mã phiếu nhập:";
            // 
            // txtOrderID
            // 
            this.txtOrderID.Enabled = false;
            this.txtOrderID.Location = new System.Drawing.Point(680, 15);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.ReadOnly = true;
            this.txtOrderID.Size = new System.Drawing.Size(100, 30);
            this.txtOrderID.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(15, 15);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(151, 29);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "NHẬP HÀNG";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlProductList);
            this.pnlLeft.Controls.Add(this.pnlSearchProduct);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 100);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(600, 500);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlProductList
            // 
            this.pnlProductList.Controls.Add(this.dgvProductSearch);
            this.pnlProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProductList.Location = new System.Drawing.Point(0, 130);
            this.pnlProductList.Name = "pnlProductList";
            this.pnlProductList.Padding = new System.Windows.Forms.Padding(15);
            this.pnlProductList.Size = new System.Drawing.Size(600, 370);
            this.pnlProductList.TabIndex = 1;
            // 
            // dgvProductSearch
            // 
            this.dgvProductSearch.AllowUserToAddRows = false;
            this.dgvProductSearch.AllowUserToDeleteRows = false;
            this.dgvProductSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProductID,
            this.colProductCode,
            this.colProductName,
            this.colUnit,
            this.colSellingPrice,
            this.colStockQuantity});
            this.dgvProductSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductSearch.Location = new System.Drawing.Point(15, 15);
            this.dgvProductSearch.MultiSelect = false;
            this.dgvProductSearch.Name = "dgvProductSearch";
            this.dgvProductSearch.ReadOnly = true;
            this.dgvProductSearch.RowHeadersWidth = 51;
            this.dgvProductSearch.RowTemplate.Height = 24;
            this.dgvProductSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductSearch.Size = new System.Drawing.Size(570, 340);
            this.dgvProductSearch.TabIndex = 0;
            this.dgvProductSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductSearch_CellClick);
            // 
            // colProductID
            // 
            this.colProductID.DataPropertyName = "ProductID";
            this.colProductID.HeaderText = "ID";
            this.colProductID.MinimumWidth = 6;
            this.colProductID.Name = "colProductID";
            this.colProductID.ReadOnly = true;
            this.colProductID.Visible = false;
            this.colProductID.Width = 50;
            // 
            // colProductCode
            // 
            this.colProductCode.DataPropertyName = "ProductCode";
            this.colProductCode.HeaderText = "Mã SP";
            this.colProductCode.MinimumWidth = 6;
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.ReadOnly = true;
            this.colProductCode.Width = 125;
            // 
            // colProductName
            // 
            this.colProductName.DataPropertyName = "ProductName";
            this.colProductName.HeaderText = "Tên sản phẩm";
            this.colProductName.MinimumWidth = 6;
            this.colProductName.Name = "colProductName";
            this.colProductName.ReadOnly = true;
            this.colProductName.Width = 200;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "Unit";
            this.colUnit.HeaderText = "ĐVT";
            this.colUnit.MinimumWidth = 6;
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Width = 80;
            // 
            // colSellingPrice
            // 
            this.colSellingPrice.DataPropertyName = "SellingPrice";
            this.colSellingPrice.HeaderText = "Giá bán";
            this.colSellingPrice.MinimumWidth = 6;
            this.colSellingPrice.Name = "colSellingPrice";
            this.colSellingPrice.ReadOnly = true;
            this.colSellingPrice.Width = 125;
            // 
            // colStockQuantity
            // 
            this.colStockQuantity.DataPropertyName = "StockQuantity";
            this.colStockQuantity.HeaderText = "Tồn kho";
            this.colStockQuantity.MinimumWidth = 6;
            this.colStockQuantity.Name = "colStockQuantity";
            this.colStockQuantity.ReadOnly = true;
            this.colStockQuantity.Width = 125;
            // 
            // pnlSearchProduct
            // 
            this.pnlSearchProduct.Controls.Add(this.numPurchasePrice);
            this.pnlSearchProduct.Controls.Add(this.lblPurchasePrice);
            this.pnlSearchProduct.Controls.Add(this.numQuantity);
            this.pnlSearchProduct.Controls.Add(this.lblQuantity);
            this.pnlSearchProduct.Controls.Add(this.btnAddToPurchase);
            this.pnlSearchProduct.Controls.Add(this.txtSearch);
            this.pnlSearchProduct.Controls.Add(this.lblSearch);
            this.pnlSearchProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchProduct.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchProduct.Name = "pnlSearchProduct";
            this.pnlSearchProduct.Size = new System.Drawing.Size(600, 130);
            this.pnlSearchProduct.TabIndex = 0;
            // 
            // numPurchasePrice
            // 
            this.numPurchasePrice.DecimalPlaces = 2;
            this.numPurchasePrice.Location = new System.Drawing.Point(431, 53);
            this.numPurchasePrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numPurchasePrice.Name = "numPurchasePrice";
            this.numPurchasePrice.Size = new System.Drawing.Size(156, 30);
            this.numPurchasePrice.TabIndex = 6;
            this.numPurchasePrice.ThousandsSeparator = true;
            // 
            // lblPurchasePrice
            // 
            this.lblPurchasePrice.AutoSize = true;
            this.lblPurchasePrice.Location = new System.Drawing.Point(344, 55);
            this.lblPurchasePrice.Name = "lblPurchasePrice";
            this.lblPurchasePrice.Size = new System.Drawing.Size(91, 25);
            this.lblPurchasePrice.TabIndex = 5;
            this.lblPurchasePrice.Text = "Giá nhập:";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(153, 53);
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(156, 30);
            this.numQuantity.TabIndex = 4;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(15, 55);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(108, 25);
            this.lblQuantity.TabIndex = 3;
            this.lblQuantity.Text = "Số lượng:";
            // 
            // btnAddToPurchase
            // 
            this.btnAddToPurchase.Location = new System.Drawing.Point(467, 89);
            this.btnAddToPurchase.Name = "btnAddToPurchase";
            this.btnAddToPurchase.Size = new System.Drawing.Size(120, 35);
            this.btnAddToPurchase.TabIndex = 2;
            this.btnAddToPurchase.Text = "Thêm";
            this.btnAddToPurchase.UseVisualStyleBackColor = true;
            this.btnAddToPurchase.Click += new System.EventHandler(this.btnAddToPurchase_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(153, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(434, 30);
            this.txtSearch.TabIndex = 1;
            //this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(15, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(132, 25);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm sản phẩm:";
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlPurchaseDetails);
            this.pnlRight.Controls.Add(this.pnlPurchaseTotal);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(600, 100);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(600, 500);
            this.pnlRight.TabIndex = 2;
            // 
            // pnlPurchaseDetails
            // 
            this.pnlPurchaseDetails.Controls.Add(this.dgvPurchaseDetails);
            this.pnlPurchaseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPurchaseDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlPurchaseDetails.Name = "pnlPurchaseDetails";
            this.pnlPurchaseDetails.Padding = new System.Windows.Forms.Padding(15);
            this.pnlPurchaseDetails.Size = new System.Drawing.Size(600, 300);
            this.pnlPurchaseDetails.TabIndex = 1;
            // 
            // dgvPurchaseDetails
            // 
            this.dgvPurchaseDetails.AllowUserToAddRows = false;
            this.dgvPurchaseDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchaseDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPurchaseDetailID,
            this.colPurchaseProductID,
            this.colPurchaseProductName,
            this.colPurchaseUnit,
            this.colPurchaseQuantity,
            this.colPurchasePrice,
            this.colPurchaseSubtotal});
            this.dgvPurchaseDetails.ContextMenuStrip = null;
            this.dgvPurchaseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchaseDetails.Location = new System.Drawing.Point(15, 15);
            this.dgvPurchaseDetails.MultiSelect = false;
            this.dgvPurchaseDetails.Name = "dgvPurchaseDetails";
            this.dgvPurchaseDetails.ReadOnly = true;
            this.dgvPurchaseDetails.RowHeadersWidth = 51;
            this.dgvPurchaseDetails.RowTemplate.Height = 24;
            this.dgvPurchaseDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurchaseDetails.Size = new System.Drawing.Size(570, 270);
            this.dgvPurchaseDetails.TabIndex = 0;
            //this.dgvPurchaseDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseDetails_CellClick);
            // 
            // colPurchaseDetailID
            // 
            this.colPurchaseDetailID.DataPropertyName = "PurchaseDetailID";
            this.colPurchaseDetailID.HeaderText = "ID";
            this.colPurchaseDetailID.MinimumWidth = 6;
            this.colPurchaseDetailID.Name = "colPurchaseDetailID";
            this.colPurchaseDetailID.ReadOnly = true;
            this.colPurchaseDetailID.Visible = false;
            this.colPurchaseDetailID.Width = 50;
            // 
            // colPurchaseProductID
            // 
            this.colPurchaseProductID.DataPropertyName = "ProductID";
            this.colPurchaseProductID.HeaderText = "Mã SP";
            this.colPurchaseProductID.MinimumWidth = 6;
            this.colPurchaseProductID.Name = "colPurchaseProductID";
            this.colPurchaseProductID.ReadOnly = true;
            this.colPurchaseProductID.Visible = false;
            this.colPurchaseProductID.Width = 80;
            // 
            // colPurchaseProductName
            // 
            this.colPurchaseProductName.DataPropertyName = "ProductName";
            this.colPurchaseProductName.HeaderText = "Tên sản phẩm";
            this.colPurchaseProductName.MinimumWidth = 6;
            this.colPurchaseProductName.Name = "colPurchaseProductName";
            this.colPurchaseProductName.ReadOnly = true;
            this.colPurchaseProductName.Width = 200;
            // 
            // colPurchaseUnit
            // 
            this.colPurchaseUnit.DataPropertyName = "Unit";
            this.colPurchaseUnit.HeaderText = "ĐVT";
            this.colPurchaseUnit.MinimumWidth = 6;
            this.colPurchaseUnit.Name = "colPurchaseUnit";
            this.colPurchaseUnit.ReadOnly = true;
            this.colPurchaseUnit.Width = 80;
            // 
            // colPurchaseQuantity
            // 
            this.colPurchaseQuantity.DataPropertyName = "Quantity";
            this.colPurchaseQuantity.HeaderText = "SL";
            this.colPurchaseQuantity.MinimumWidth = 6;
            this.colPurchaseQuantity.Name = "colPurchaseQuantity";
            this.colPurchaseQuantity.ReadOnly = true;
            this.colPurchaseQuantity.Width = 80;
            // 
            // colPurchasePrice
            // 
            this.colPurchasePrice.DataPropertyName = "PurchasePrice";
            this.colPurchasePrice.HeaderText = "Giá nhập";
            this.colPurchasePrice.MinimumWidth = 6;
            this.colPurchasePrice.Name = "colPurchasePrice";
            this.colPurchasePrice.ReadOnly = true;
            this.colPurchasePrice.Width = 125;
            // 
            // colPurchaseSubtotal
            // 
            this.colPurchaseSubtotal.DataPropertyName = "Subtotal";
            this.colPurchaseSubtotal.HeaderText = "Thành tiền";
            this.colPurchaseSubtotal.MinimumWidth = 6;
            this.colPurchaseSubtotal.Name = "colPurchaseSubtotal";
            this.colPurchaseSubtotal.ReadOnly = true;
            this.colPurchaseSubtotal.Width = 125;
            // 
            // pnlPurchaseTotal
            // 
            this.pnlPurchaseTotal.Controls.Add(this.txtNotes);
            this.pnlPurchaseTotal.Controls.Add(this.lblNotes);
            this.pnlPurchaseTotal.Controls.Add(this.btnSavePurchase);
            this.pnlPurchaseTotal.Controls.Add(this.btnNewPurchase);
            this.pnlPurchaseTotal.Controls.Add(this.lblTotalAmount);
            this.pnlPurchaseTotal.Controls.Add(this.txtTotalAmount);
            this.pnlPurchaseTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPurchaseTotal.Location = new System.Drawing.Point(0, 300);
            this.pnlPurchaseTotal.Name = "pnlPurchaseTotal";
            this.pnlPurchaseTotal.Size = new System.Drawing.Size(600, 200);
            this.pnlPurchaseTotal.TabIndex = 0;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(165, 55);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(420, 80);
            this.txtNotes.TabIndex = 5;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(15, 55);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(85, 25);
            this.lblNotes.TabIndex = 4;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // btnSavePurchase
            // 
            this.btnSavePurchase.Location = new System.Drawing.Point(385, 145);
            this.btnSavePurchase.Name = "btnSavePurchase";
            this.btnSavePurchase.Size = new System.Drawing.Size(200, 40);
            this.btnSavePurchase.TabIndex = 3;
            this.btnSavePurchase.Text = "Lưu phiếu nhập";
            this.btnSavePurchase.UseVisualStyleBackColor = true;
            this.btnSavePurchase.Click += new System.EventHandler(this.btnSavePurchase_Click);
            // 
            // btnNewPurchase
            // 
            this.btnNewPurchase.Location = new System.Drawing.Point(165, 145);
            this.btnNewPurchase.Name = "btnNewPurchase";
            this.btnNewPurchase.Size = new System.Drawing.Size(200, 40);
            this.btnNewPurchase.TabIndex = 2;
            this.btnNewPurchase.Text = "Phiếu nhập mới";
            this.btnNewPurchase.UseVisualStyleBackColor = true;
            this.btnNewPurchase.Click += new System.EventHandler(this.btnNewPurchase_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(15, 15);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(107, 25);
            this.lblTotalAmount.TabIndex = 1;
            this.lblTotalAmount.Text = "Tổng tiền:";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalAmount.Location = new System.Drawing.Point(165, 15);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(420, 30);
            this.txtTotalAmount.TabIndex = 0;
            this.txtTotalAmount.Text = "0";
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmPurchaseOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPurchaseOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập hàng";
            this.Load += new System.EventHandler(this.frmPurchaseOrder_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlProductList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).EndInit();
            this.pnlSearchProduct.ResumeLayout(false);
            this.pnlSearchProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPurchasePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlPurchaseDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseDetails)).EndInit();
            this.pnlPurchaseTotal.ResumeLayout(false);
            this.pnlPurchaseTotal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // Controls của form
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtOrderID;
        private System.Windows.Forms.Label lblOrderID;
        private System.Windows.Forms.DateTimePicker dtpOrderDate;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.TextBox txtSupplier;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlSearchProduct;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAddToPurchase;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label lblPurchasePrice;
        private System.Windows.Forms.NumericUpDown numPurchasePrice;
        private System.Windows.Forms.Panel pnlProductList;
        private System.Windows.Forms.DataGridView dgvProductSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSellingPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockQuantity;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlPurchaseTotal;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Button btnNewPurchase;
        private System.Windows.Forms.Button btnSavePurchase;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Panel pnlPurchaseDetails;
        private System.Windows.Forms.DataGridView dgvPurchaseDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseDetailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchasePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseSubtotal;
    }
}