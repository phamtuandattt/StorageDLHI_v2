using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.PoGUI
{
    public partial class frmCustomInfoPO : KryptonForm
    {
        private Pos mPO = new Pos();
        private DataTable dtPODetail = new DataTable();
        public bool completed { get; set; } = true;
        public bool isHandle { get; set; } = false;

        private DataTable dtProdOfPO_UpdateDB = new DataTable();
        private double totalAmount = 0;
        private Guid mprID = Guid.Empty;

        public frmCustomInfoPO()
        {
            InitializeComponent();
            LoadData();
        }

        public frmCustomInfoPO(string title, bool status, Pos mPO, DataTable dtPODetail, double totalAmount, Guid mprID)
        {
            InitializeComponent();
            LoadData();
            this.Text = title;
            this.mPO = mPO;
            this.dtPODetail = dtPODetail;
            this.totalAmount = totalAmount;
            this.mprID = mprID;

            txtMPRNo.Text = this.mPO.Po_Mpr_No;
            txtWoNo.Text = this.mPO.Po_Wo_No;
            txtProjectName.Text = this.mPO.Po_Project_Name;
            txtPrepared.Text = ShareData.UserName;

            if (status)
            {
                HideColumn(1);
                ResizeFormToFitTable();
            }
            else
            {
                ShowSecondColumn(1);
                ResizeFormToFitTable();
            }
        }

        private void HideColumn(int columnIndex)
        {
            // Optionally hide controls in that column
            foreach (Control c in tblLayouMain.Controls)
            {
                if (tblLayouMain.GetColumn(c) == columnIndex)
                {
                    c.Visible = false;
                }
            }
            tblLayouMain.ColumnStyles[columnIndex].SizeType = SizeType.Absolute;
            tblLayouMain.ColumnStyles[columnIndex].Width = 0;
        }

        private void ShowSecondColumn(int columnToShow, int width = 581)
        {
            foreach (Control ctrl in tblLayouMain.Controls)
            {
                if (tblLayouMain.GetColumn(ctrl) == columnToShow)
                {
                    ctrl.Visible = true;
                }
            }

            tblLayouMain.ColumnStyles[columnToShow].SizeType = SizeType.Absolute;
            tblLayouMain.ColumnStyles[columnToShow].Width = width;

            tblLayouMain.Dock = DockStyle.Fill;
            this.Size = new Size(1247, 581); // Restore size
        }


        private void ResizeFormToFitTable()
        {
            // Turn off Dock temporarily so the table can shrink to its real size
            tblLayouMain.Dock = DockStyle.None;

            // Let layout recalculate
            tblLayouMain.PerformLayout();

            // Resize the form to match the new size of the TableLayoutPanel
            this.ClientSize = tblLayouMain.PreferredSize;

            // (Optional) Prevent resizing beyond this size
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private async void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.COST_DATATABLE_ALLCOST))
            {
                var dtCost = await MaterialDAO.GetCosts();
                LoadDataCombox(cboCost, dtCost, QueryStatement.PROPERTY_COST_NAME, QueryStatement.PROPERTY_COST_ID);
                CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, dtCost);
            }
            else
            {
                var dtCostFromCache = CacheManager.Get<DataTable>(CacheKeys.COST_DATATABLE_ALLCOST);
                LoadDataCombox(cboCost, dtCostFromCache, QueryStatement.PROPERTY_COST_NAME, QueryStatement.PROPERTY_COST_ID);
            }

            if (!CacheManager.Exists(CacheKeys.TAX_DATATABLE_ALLTAX))
            {
                var dtTax = await MaterialDAO.GetTaxs();
                LoadDataCombox(cboTax, dtTax, QueryStatement.PROPERTY_TAX_PERCENT, QueryStatement.PROPERTY_TAX_ID);
                CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, dtTax);
            }
            else
            {
                var dtTaxFromCache = CacheManager.Get<DataTable>(CacheKeys.TAX_DATATABLE_ALLTAX);
                LoadDataCombox(cboTax, dtTaxFromCache, QueryStatement.PROPERTY_TAX_PERCENT, QueryStatement.PROPERTY_TAX_ID);
            }

            if (!CacheManager.Exists(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER))
            {
                var dtSupp = await SupplierDAO.GetSuppliers();
                LoadDataCombox(cboSuppplier, dtSupp, QueryStatement.PROPERTY_SUPPLIER_NAME, QueryStatement.PROPERTY_SUPPLIER_ID);
                CacheManager.Add(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER, dtSupp);
            }
            else
            {
                var dtSuppFromCache = CacheManager.Get<DataTable>(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER);
                LoadDataCombox(cboSuppplier, dtSuppFromCache, QueryStatement.PROPERTY_SUPPLIER_NAME, QueryStatement.PROPERTY_SUPPLIER_ID);
            }

        }

        private void LoadDataCombox(KryptonComboBox comboBox, DataTable dataTable, string displayMember, string valueMemeber)
        {
            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMemeber;
            if (dataTable.Rows.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var paymentTerm = "";
            if (radOption1.Checked)
            {
                paymentTerm = radOption1.Text.Trim() + " " + txtWithIn.Text.Trim() + " " + lblWithin.Text.Trim();
            }
            if (radOption2.Checked)
            {
                paymentTerm = radOption2.Text.Trim() + " " + txtDepo1.Text.Trim() + ", " + txtDepo2.Text.Trim() + " " + txtDepo3.Text.Trim(); 
            }
            if (radOption3.Checked)
            {
                paymentTerm = txtOther.Text.Trim();
            }

            Pos pos = new Pos()
            {
                Id = Guid.NewGuid(),
                Po_No = txtPONo.Text.Trim().ToUpper(),
                Po_Mpr_No = txtMPRNo.Text.Trim().ToUpper(),
                Po_Wo_No = txtWoNo.Text.Trim().ToUpper(),
                Po_Project_Name = txtProjectName.Text.Trim().ToUpper(),
                Po_Rev_Total = 0,
                Po_CreateDate = DateTime.Parse(dtPickerCreate.Value.ToString("dd/MM/yyyy")),
                Po_Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                Po_Prepared = txtPrepared.Text.Trim(),
                Po_Reviewed = txtReviewed.Text.Trim(),
                Po_Agrement = txtAggrement.Text.Trim(),
                Po_Approved = txtApproved.Text.Trim(),
                Po_Payment_Term = paymentTerm,
                Po_Dispatch_Box = "",
                Po_Total_Amount = totalAmount,
                CostId = Guid.Parse(cboCost.SelectedValue.ToString()),
                TaxId = Guid.Parse(cboTax.SelectedValue.ToString()),
                SupplierId = Guid.Parse(cboSuppplier.SelectedValue.ToString()),
                Staff_Id = ShareData.UserId,
                IsImported = false
            };

            // Convert dtProdOfAddPO to dtProdOfPO_UpdateDB
            dtProdOfPO_UpdateDB = await PoDAO.GetPODetailForm();
            foreach (DataRow item in this.dtPODetail.Rows)
            {
                DataRow dataRow = dtProdOfPO_UpdateDB.NewRow();
                dataRow[0] = Guid.NewGuid().ToString();
                dataRow[1] = pos.Id;
                dataRow[2] = Guid.Parse(item[0].ToString());
                dataRow[3] = Int32.Parse(item[12].ToString());
                dataRow[4] = Int32.Parse(item[13].ToString());
                dataRow[5] = Int32.Parse(item[14].ToString());
                dataRow[6] = DateTime.Now.ToString("dd/MM/yyyy");
                dataRow[7] = item[15].ToString();
                dataRow[8] = item[16].ToString();

                dtProdOfPO_UpdateDB.Rows.Add(dataRow);
            }

            if (await PoDAO.InsertPO(pos))
            {
                if (PoDAO.InsertPODetail(dtProdOfPO_UpdateDB) && await MprDAO.UpdateMprIsMakePO(this.mprID))
                {
                    // Update IsMakePO in table MPR
                    MessageBoxHelper.ShowInfo("Add PO success !");
                    this.completed = true;
                    this.isHandle = true;
                    this.Close();
                }
                else
                {
                    PoDAO.DeletePO(pos.Id);
                    MessageBoxHelper.ShowWarning("Add PO fail !");
                }
            }
            else
            {
                MessageBoxHelper.ShowWarning("Add PO fail !");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.completed = false;
            this.Close();
        }

        private void radOption1_CheckedChanged(object sender, EventArgs e)
        {
            txtWithIn.Enabled = true;
            txtWithIn.Focus();
            txtDepo1.Enabled = false;
            txtDepo2.Enabled = false;
            txtDepo3.Enabled = false;
            txtOther.Enabled = false;
        }

        private void radOption2_CheckedChanged(object sender, EventArgs e)
        {
            txtWithIn.Enabled = false;
            txtDepo1.Enabled = true;
            txtDepo1.Focus();
            txtDepo2.Enabled = true;
            txtDepo3.Enabled = true;
            txtOther.Enabled = false;
        }

        private void radOption3_CheckedChanged(object sender, EventArgs e)
        {
            txtWithIn.Enabled = false;
            txtDepo1.Enabled = false;
            txtDepo2.Enabled = false;
            txtDepo3.Enabled = false;
            txtOther.Enabled = true;
            txtOther.Focus();
        }

        private void cboTax_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboCost_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboSuppplier_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void frmCustomInfoPO_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.completed = false;
        }

        private void txtPONo_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtPONo.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtPONo.Text != cleaned)
            {
                int pos = txtPONo.SelectionStart - 1;
                txtPONo.Text = cleaned;
                txtPONo.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtPrepared_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtPrepared.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtPrepared.Text != cleaned)
            {
                int pos = txtPrepared.SelectionStart - 1;
                txtPrepared.Text = cleaned;
                txtPrepared.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtReviewed_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtReviewed.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtReviewed.Text != cleaned)
            {
                int pos = txtReviewed.SelectionStart - 1;
                txtReviewed.Text = cleaned;
                txtReviewed.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtAggrement_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtAggrement.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtAggrement.Text != cleaned)
            {
                int pos = txtAggrement.SelectionStart - 1;
                txtAggrement.Text = cleaned;
                txtAggrement.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtApproved_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtApproved.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtApproved.Text != cleaned)
            {
                int pos = txtApproved.SelectionStart - 1;
                txtApproved.Text = cleaned;
                txtApproved.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtWithIn_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWithIn.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtWithIn.Text != cleaned)
            {
                int pos = txtWithIn.SelectionStart - 1;
                txtWithIn.Text = cleaned;
                txtWithIn.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtDepo1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDepo2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDepo3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOther_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
