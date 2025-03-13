namespace StorageDLHI.App
{
    partial class Supplier_bank_GUI
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
            this.txtAcc = new System.Windows.Forms.TextBox();
            this.dd = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBeneficial = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.grdBanks = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdBanks)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAcc
            // 
            this.txtAcc.Location = new System.Drawing.Point(195, 31);
            this.txtAcc.Name = "txtAcc";
            this.txtAcc.Size = new System.Drawing.Size(246, 22);
            this.txtAcc.TabIndex = 0;
            // 
            // dd
            // 
            this.dd.AutoSize = true;
            this.dd.Location = new System.Drawing.Point(54, 37);
            this.dd.Name = "dd";
            this.dd.Size = new System.Drawing.Size(91, 16);
            this.dd.TabIndex = 1;
            this.dd.Text = "Bank account:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(195, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(246, 22);
            this.txtName.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Bank Name:";
            // 
            // txtBeneficial
            // 
            this.txtBeneficial.Location = new System.Drawing.Point(195, 87);
            this.txtBeneficial.Name = "txtBeneficial";
            this.txtBeneficial.Size = new System.Drawing.Size(246, 22);
            this.txtBeneficial.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Bank beneficial:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(465, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 80);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "ADD";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(674, 29);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(85, 80);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "LOAD";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(570, 29);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(85, 80);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "MODIFY";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // grdBanks
            // 
            this.grdBanks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBanks.Location = new System.Drawing.Point(12, 155);
            this.grdBanks.Name = "grdBanks";
            this.grdBanks.RowHeadersWidth = 51;
            this.grdBanks.RowTemplate.Height = 24;
            this.grdBanks.Size = new System.Drawing.Size(776, 283);
            this.grdBanks.TabIndex = 3;
            // 
            // Supplier_bank_GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grdBanks);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dd);
            this.Controls.Add(this.txtBeneficial);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtAcc);
            this.Name = "Supplier_bank_GUI";
            this.Text = "Supplier_bank_GUI";
            ((System.ComponentModel.ISupportInitialize)(this.grdBanks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAcc;
        private System.Windows.Forms.Label dd;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBeneficial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.DataGridView grdBanks;
    }
}