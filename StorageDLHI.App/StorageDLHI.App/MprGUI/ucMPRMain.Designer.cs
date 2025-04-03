namespace StorageDLHI.App.MprGUI
{
    partial class ucMPRMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kryptonNavigator1 = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.pageMPRs = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.pageAddMPR = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchProd = new System.Windows.Forms.ToolStripTextBox();
            this.kryptonPanel2 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchProdExistMPR = new System.Windows.Forms.ToolStripTextBox();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.dgvProds = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dgvProdOfMPRs = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.btnAddProd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnConfirm = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            this.kryptonNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageMPRs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageAddMPR)).BeginInit();
            this.pageAddMPR.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdOfMPRs)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.Button.CloseButtonAction = ComponentFactory.Krypton.Navigator.CloseButtonAction.None;
            this.kryptonNavigator1.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.Pages.AddRange(new ComponentFactory.Krypton.Navigator.KryptonPage[] {
            this.pageMPRs,
            this.pageAddMPR});
            this.kryptonNavigator1.SelectedIndex = 1;
            this.kryptonNavigator1.Size = new System.Drawing.Size(1386, 757);
            this.kryptonNavigator1.StateCommon.Panel.Color1 = System.Drawing.Color.White;
            this.kryptonNavigator1.StateCommon.Panel.Color2 = System.Drawing.Color.White;
            this.kryptonNavigator1.StateCommon.Tab.Back.Color1 = System.Drawing.Color.White;
            this.kryptonNavigator1.StateCommon.Tab.Back.Color2 = System.Drawing.Color.White;
            this.kryptonNavigator1.TabIndex = 0;
            this.kryptonNavigator1.Text = "kryptonNavigator1";
            // 
            // pageMPRs
            // 
            this.pageMPRs.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.pageMPRs.Flags = 65534;
            this.pageMPRs.LastVisibleSet = true;
            this.pageMPRs.MinimumSize = new System.Drawing.Size(50, 50);
            this.pageMPRs.Name = "pageMPRs";
            this.pageMPRs.Size = new System.Drawing.Size(1384, 726);
            this.pageMPRs.Text = "MPR List";
            this.pageMPRs.ToolTipTitle = "MPR List";
            this.pageMPRs.UniqueName = "37E7262BB4BD4315628A3D632D8EE9F7";
            // 
            // pageAddMPR
            // 
            this.pageAddMPR.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.pageAddMPR.Controls.Add(this.tableLayoutPanel1);
            this.pageAddMPR.Flags = 65534;
            this.pageAddMPR.LastVisibleSet = true;
            this.pageAddMPR.MinimumSize = new System.Drawing.Size(50, 50);
            this.pageAddMPR.Name = "pageAddMPR";
            this.pageAddMPR.Size = new System.Drawing.Size(1384, 726);
            this.pageAddMPR.Text = "Add MPR";
            this.pageAddMPR.ToolTipTitle = "Add MPR";
            this.pageAddMPR.UniqueName = "C1CDCFEA77D34F9CC58428E5EF444C03";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.kryptonPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1384, 726);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.toolStrip1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(3, 3);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonPanel1.Size = new System.Drawing.Size(1378, 357);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddProd,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.txtSearchProd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1378, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 28);
            this.toolStripLabel1.Text = "Search:";
            // 
            // txtSearchProd
            // 
            this.txtSearchProd.AutoSize = false;
            this.txtSearchProd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchProd.Name = "txtSearchProd";
            this.txtSearchProd.Size = new System.Drawing.Size(150, 27);
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel2.Controls.Add(this.toolStrip2);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel2.Location = new System.Drawing.Point(3, 366);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonPanel2.Size = new System.Drawing.Size(1378, 357);
            this.kryptonPanel2.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.txtSearchProdExistMPR,
            this.btnConfirm});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip2.Size = new System.Drawing.Size(1378, 27);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 24);
            this.toolStripLabel2.Text = "Search:";
            // 
            // txtSearchProdExistMPR
            // 
            this.txtSearchProdExistMPR.AutoSize = false;
            this.txtSearchProdExistMPR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchProdExistMPR.Name = "txtSearchProdExistMPR";
            this.txtSearchProdExistMPR.Size = new System.Drawing.Size(150, 27);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 27);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            this.kryptonGroupBox1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.dgvProds);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(1378, 330);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "Product list";
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 27);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            this.kryptonGroupBox2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.dgvProdOfMPRs);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(1378, 330);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "Products of MPRs";
            // 
            // dgvProds
            // 
            this.dgvProds.AllowUserToAddRows = false;
            this.dgvProds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProds.Location = new System.Drawing.Point(0, 0);
            this.dgvProds.Name = "dgvProds";
            this.dgvProds.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.dgvProds.RowHeadersWidth = 51;
            this.dgvProds.RowTemplate.Height = 24;
            this.dgvProds.Size = new System.Drawing.Size(1374, 302);
            this.dgvProds.TabIndex = 1;
            // 
            // dgvProdOfMPRs
            // 
            this.dgvProdOfMPRs.AllowUserToAddRows = false;
            this.dgvProdOfMPRs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProdOfMPRs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProdOfMPRs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProdOfMPRs.Location = new System.Drawing.Point(0, 0);
            this.dgvProdOfMPRs.Name = "dgvProdOfMPRs";
            this.dgvProdOfMPRs.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.dgvProdOfMPRs.RowHeadersWidth = 51;
            this.dgvProdOfMPRs.RowTemplate.Height = 24;
            this.dgvProdOfMPRs.Size = new System.Drawing.Size(1374, 302);
            this.dgvProdOfMPRs.TabIndex = 1;
            // 
            // btnAddProd
            // 
            this.btnAddProd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddProd.Image = global::StorageDLHI.App.Properties.Resources.add;
            this.btnAddProd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddProd.Name = "btnAddProd";
            this.btnAddProd.Size = new System.Drawing.Size(29, 24);
            this.btnAddProd.Text = "Add new product for create MPR";
            this.btnAddProd.Click += new System.EventHandler(this.btnAddProd_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::StorageDLHI.App.Properties.Resources.bin;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Image = global::StorageDLHI.App.Properties.Resources.right;
            this.btnConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 24);
            this.btnConfirm.Text = "Comfirm";
            // 
            // ucMPRMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.kryptonNavigator1);
            this.Name = "ucMPRMain";
            this.Size = new System.Drawing.Size(1386, 757);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            this.kryptonNavigator1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pageMPRs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageAddMPR)).EndInit();
            this.pageAddMPR.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.kryptonPanel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdOfMPRs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private ComponentFactory.Krypton.Navigator.KryptonPage pageMPRs;
        private ComponentFactory.Krypton.Navigator.KryptonPage pageAddMPR;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddProd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtSearchProd;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtSearchProdExistMPR;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvProds;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvProdOfMPRs;
        private System.Windows.Forms.ToolStripButton btnConfirm;
    }
}
