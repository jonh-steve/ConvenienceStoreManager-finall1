namespace ConvenienceStoreManager.UI
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProductManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvoiceList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPurchaseOrderList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOperations = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSales = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPurchaseOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel = new System.Windows.Forms.Panel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuSystem,
                this.mnuManagement,
                this.mnuOperations,
                this.mnuReports,
                this.mnuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(984, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // mnuSystem
            // 
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuExit});
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(69, 20);
            this.mnuSystem.Text = "Hệ thống";
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(104, 22);
            this.mnuExit.Text = "Thoát";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuManagement
            // 
            this.mnuManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuProductManagement,
                this.mnuInvoiceList,
                this.mnuPurchaseOrderList});
            this.mnuManagement.Name = "mnuManagement";
            this.mnuManagement.Size = new System.Drawing.Size(60, 20);
            this.mnuManagement.Text = "Quản lý";
            // 
            // mnuProductManagement
            // 
            this.mnuProductManagement.Name = "mnuProductManagement";
            this.mnuProductManagement.Size = new System.Drawing.Size(180, 22);
            this.mnuProductManagement.Text = "Sản phẩm";
            this.mnuProductManagement.Click += new System.EventHandler(this.mnuProductManagement_Click);
            // 
            // mnuInvoiceList
            // 
            this.mnuInvoiceList.Name = "mnuInvoiceList";
            this.mnuInvoiceList.Size = new System.Drawing.Size(180, 22);
            this.mnuInvoiceList.Text = "Danh sách hóa đơn";
            this.mnuInvoiceList.Click += new System.EventHandler(this.mnuInvoiceList_Click);
            // 
            // mnuPurchaseOrderList
            // 
            this.mnuPurchaseOrderList.Name = "mnuPurchaseOrderList";
            this.mnuPurchaseOrderList.Size = new System.Drawing.Size(180, 22);
            this.mnuPurchaseOrderList.Text = "Danh sách phiếu nhập";
            this.mnuPurchaseOrderList.Click += new System.EventHandler(this.mnuPurchaseOrderList_Click);
            // 
            // mnuOperations
            // 
            this.mnuOperations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuSales,
                this.mnuPurchaseOrder});
            this.mnuOperations.Name = "mnuOperations";
            this.mnuOperations.Size = new System.Drawing.Size(75, 20);
            this.mnuOperations.Text = "Nghiệp vụ";
            // 
            // mnuSales
            // 
            this.mnuSales.Name = "mnuSales";
            this.mnuSales.Size = new System.Drawing.Size(180, 22);
            this.mnuSales.Text = "Bán hàng";
            this.mnuSales.Click += new System.EventHandler(this.mnuSales_Click);
            // 
            // mnuPurchaseOrder
            // 
            this.mnuPurchaseOrder.Name = "mnuPurchaseOrder";
            this.mnuPurchaseOrder.Size = new System.Drawing.Size(180, 22);
            this.mnuPurchaseOrder.Text = "Nhập hàng";
            this.mnuPurchaseOrder.Click += new System.EventHandler(this.mnuPurchaseOrder_Click);
            // 
            // mnuReports
            // 
            this.mnuReports.Name = "mnuReports";
            this.mnuReports.Size = new System.Drawing.Size(62, 20);
            this.mnuReports.Text = "Báo cáo";
            this.mnuReports.Click += new System.EventHandler(this.mnuReports_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(62, 20);
            this.mnuHelp.Text = "Trợ giúp";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuAbout.Text = "Thông tin";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 589);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 17);
            this.lblStatus.Text = "Sẵn sàng...";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lblDateTime);
            this.panel.Controls.Add(this.lblUsername);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 24);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(984, 565);
            this.panel.TabIndex = 2;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 12);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Người dùng: Admin";
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Location = new System.Drawing.Point(850, 12);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(100, 13);
            this.lblDateTime.TabIndex = 1;
            this.lblDateTime.Text = "dd/MM/yyyy HH:mm";
            // 
            // timerDateTime
            // 
            this.timerDateTime.Enabled = true;
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phần mềm Quản lý Cửa hàng Tiện lợi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuManagement;
        private System.Windows.Forms.ToolStripMenuItem mnuProductManagement;
        private System.Windows.Forms.ToolStripMenuItem mnuInvoiceList;
        private System.Windows.Forms.ToolStripMenuItem mnuPurchaseOrderList;
        private System.Windows.Forms.ToolStripMenuItem mnuOperations;
        private System.Windows.Forms.ToolStripMenuItem mnuSales;
        private System.Windows.Forms.ToolStripMenuItem mnuPurchaseOrder;
        private System.Windows.Forms.ToolStripMenuItem mnuReports;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Timer timerDateTime;
    }
}