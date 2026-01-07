using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Adapters;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomInfoMpr : KryptonForm
    {
        private bool status = true;
        private bool isPrint = false;
        private DataTable dtProdOfMpr = new DataTable();
        private DataTable dtProdOfMprAdd = new DataTable(); 
        private Mprs dtMPRDetail = new Mprs();
        private DataTable dtForPrint = new DataTable();

        public bool CanelOrConfirm { get; set; } = true; // true is confirm || cancel


        public frmCustomInfoMpr()
        {
            InitializeComponent();

            this.dtPickerCreate.MinDate = DateTime.Now;
        }

        public frmCustomInfoMpr(string title, bool status, Mprs dtMPRDetail)
        {
            InitializeComponent();
            this.Text = title;
            this.status = status;
            this.dtMPRDetail = dtMPRDetail;

            txtMPRNo.Text = this.dtMPRDetail.Mpr_No.Trim();
            //txtWoNo.Text = this.dtMPRDetail.Mpr_Wo_No.Trim();
            //txtProjectName.Text = this.dtMPRDetail.Mpr_Project_Name_Code.Trim();
            txtPrepared.Text = this.dtMPRDetail.Mpr_Prepared.Trim();
            txtReviewed.Text = this.dtMPRDetail.Mpr_Reviewed.Trim();
            txtApproved.Text = this.dtMPRDetail.Mpr_Approved.Trim();
            dtPickerCreate.Value = this.dtMPRDetail.CreateDate;
            dtPickerDelivery.Value = this.dtMPRDetail.Expected_Delivery_Date;

            txtMPRNo.ReadOnly = true;
            txtWoNo.ReadOnly = true;
            txtProjectName.ReadOnly = true;
            dtPickerCreate.Enabled = false;


            HideRow(1);
            ResizeFormToFitTable();
            
        }

        public frmCustomInfoMpr(string title, bool status, DataTable dtProdOfMpr)
        {
            InitializeComponent();
            this.Text = title;
            this.dtProdOfMpr = dtProdOfMpr;
            this.dtPickerCreate.MinDate = DateTime.Now;

            if (status)
            {
                HideRow(1);
                ResizeFormToFitTable();
            }
        }

        public frmCustomInfoMpr(string title, bool status, bool isPrint, Mprs dtMPRDetail, DataTable dtForPrint) // status = false; isPrint = true
        {
            InitializeComponent();
            this.Text = title;
            this.dtForPrint = dtForPrint;
            this.isPrint = isPrint;
            this.dtMPRDetail = dtMPRDetail;

            txtMPRNo.Text = this.dtMPRDetail.Mpr_No.Trim();
            //txtWoNo.Text = this.dtMPRDetail.Mpr_Wo_No.Trim();
            //txtProjectName.Text = this.dtMPRDetail.Mpr_Project_Name_Code.Trim();
            txtPrepared.Text = this.dtMPRDetail.Mpr_Prepared.Trim();
            txtReviewed.Text = this.dtMPRDetail.Mpr_Reviewed.Trim();
            txtApproved.Text = this.dtMPRDetail.Mpr_Approved.Trim();
            dtPickerCreate.Value = this.dtMPRDetail.CreateDate;
            dtPickerDelivery.Value = this.dtMPRDetail.Expected_Delivery_Date;

            txtMPRNo.ReadOnly = true;
            txtWoNo.ReadOnly = true;
            txtProjectName.ReadOnly = true;
            dtPickerCreate.Enabled = false;

            if (!status && isPrint)
            {
                //ShowSecondRow(1);
                HideRow(1);
                btnSave.Text = "PRINT";
                ResizeFormToFitTable();
            }
        }

        private void HideRow(int rowIndex)
        {
            //876, 260
            // Optionally hide controls in that column
            foreach (Control c in tblLayoutMain.Controls)
            {
                if (tblLayoutMain.GetRow(c) == rowIndex)
                {
                    c.Visible = false;
                }
            }
            tblLayoutMain.RowStyles[rowIndex].SizeType = SizeType.Absolute;
            tblLayoutMain.RowStyles[rowIndex].Height = 0;
        }

        private void ShowSecondRow(int rowToShow, int height = 250)
        {
            // 906, 586
            foreach (Control ctrl in tblLayoutMain.Controls)
            {
                if (tblLayoutMain.GetRow(ctrl) == rowToShow)
                {
                    ctrl.Visible = true;
                }
            }

            tblLayoutMain.RowStyles[rowToShow].SizeType = SizeType.Absolute;
            tblLayoutMain.RowStyles[rowToShow].Height = height;

            tblLayoutMain.Dock = DockStyle.Fill;
            this.Size = new Size(906, 586); // Restore size
        }

        private void ResizeFormToFitTable()
        {
            // Turn off Dock temporarily so the table can shrink to its real size
            tblLayoutMain.Dock = DockStyle.None;

            // Let layout recalculate
            tblLayoutMain.PerformLayout();

            // Resize the form to match the new size of the TableLayoutPanel
            this.ClientSize = tblLayoutMain.PreferredSize;

            // (Optional) Prevent resizing beyond this size
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMPRNo.Text.Trim())
                || string.IsNullOrEmpty(txtWoNo.Text.Trim())
                || string.IsNullOrEmpty(txtProjectName.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill in the information completely !");
                return;
            }

            this.dtProdOfMprAdd = await MprDAO.GetMprDetailForm();
            if (status && !isPrint)
            {
                Mprs mprs = new Mprs()
                {
                    Id = Guid.NewGuid(),
                    Mpr_No = txtMPRNo.Text.Trim(),
                    //Mpr_Wo_No = txtWoNo.Text.Trim(),
                    //Mpr_Project_Name_Code = txtProjectName.Text.Trim(),
                    CreateDate = DateTime.Parse(dtPickerCreate.Value.ToString("dd/MM/yyyy")),
                    Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                    Mpr_Prepared = txtPrepared.Text.Trim(),
                    Mpr_Reviewed = txtReviewed.Text.Trim(),
                    Mpr_Approved = txtApproved.Text.Trim(),
                    Staff_Id = ShareData.UserId,
                    IsMakePO = false,
                };

                foreach (DataRow dr in dtProdOfMpr.Rows)
                {
                    DataRow new_r = dtProdOfMprAdd.NewRow();
                    new_r[0] = Guid.NewGuid();
                    new_r[1] = mprs.Id;
                    new_r[2] = dr[0];
                    new_r[3] = Int32.Parse(dr[13].ToString().Trim());
                    new_r[4] = dr[14].ToString().Trim();
                    new_r[5] = "";
                    new_r[6] = 0;
                    new_r[7] = "";
                    new_r[8] = "";
                    new_r[9] = mprs.Expected_Delivery_Date;
                    new_r[10] = "";

                    dtProdOfMprAdd.Rows.Add(new_r);
                }

                if (await MprDAO.InsertMpr(mprs))
                {
                    if (MprDAO.InsertMprDetail(this.dtProdOfMprAdd))
                    {
                        MessageBoxHelper.ShowInfo("Create MPRs success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Create MPRs faild !");
                        MprDAO.DeleteMpr(mprs.Id);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Create MPRs faild !");
                }
            }
            else if (!status && !isPrint)
            {
                Mprs mprs = new Mprs()
                {
                    Id = this.dtMPRDetail.Id,
                    Mpr_No = txtMPRNo.Text.Trim(),
                    //Mpr_Wo_No = txtWoNo.Text.Trim(),
                    //Mpr_Project_Name_Code = txtProjectName.Text.Trim(),
                    CreateDate = DateTime.Parse(dtPickerCreate.Value.ToString("dd/MM/yyyy")),
                    Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                    Mpr_Prepared = txtPrepared.Text.Trim(),
                    Mpr_Reviewed = txtReviewed.Text.Trim(),
                    Mpr_Approved = txtApproved.Text.Trim(),
                };

                if (await MprDAO.UpdateMprInfo(mprs))
                {
                    MessageBoxHelper.ShowInfo("Update MPRs info success !");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Update MPRs info faild !");
                }
            }
            else
            {
                Guid mprId = dtMPRDetail.Id;
                string mpr_no = dtMPRDetail.Mpr_No.Trim();
                //string wo_no = dtMPRDetail.Mpr_Wo_No.Trim();
                //string project_name = dtMPRDetail.Mpr_Project_Name_Code.Trim();

                var placeholders = new Dictionary<string, string>
                {
                    //{ Common.DictionaryKey.MPR_NO, mpr_no },
                    //{ Common.DictionaryKey.WO_NO, wo_no },
                    //{ Common.DictionaryKey.PROJECT_NAME, project_name },
                    { Common.DictionaryKey.DATE_EXPORT, DateTime.Now.ToString("dd/MM/yyyy") },
                    { Common.DictionaryKey.PREPARED, dtMPRDetail.Mpr_Prepared },
                    { Common.DictionaryKey.REVIEWED, dtMPRDetail.Mpr_Reviewed },
                    { Common.DictionaryKey.APPROVED, dtMPRDetail.Mpr_Approved }
                };

                string templatePath = Common.PathManager.GetPathTemplate(Common.PathManager.MPR_TEMPLATE_FILE_NAME);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Excel File",
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"Report_{mpr_no}_{DateTime.Now.ToString("dd.MM.yyyy")}.xlsx",
                    DefaultExt = "xlsx",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveFileDialog.FileName;

                    Common.Common.ExportToExcelTemplate(templatePath, outputPath, dtForPrint, placeholders, Enums.ExportToExcel.MPRs);

                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CanelOrConfirm = false;
            this.Close();
        }

        private void tblLayoutCustom_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMPRNo_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtMPRNo.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtMPRNo.Text != cleaned)
            {
                int pos = txtMPRNo.SelectionStart - 1;
                txtMPRNo.Text = cleaned;
                txtMPRNo.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtWoNo_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWoNo.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtWoNo.Text != cleaned)
            {
                int pos = txtWoNo.SelectionStart - 1;
                txtWoNo.Text = cleaned;
                txtWoNo.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtProjectName.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtProjectName.Text != cleaned)
            {
                int pos = txtProjectName.SelectionStart - 1;
                txtProjectName.Text = cleaned;
                txtProjectName.SelectionStart = Math.Max(pos, 0);
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
    }
}
