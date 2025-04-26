using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.Infrastructor.Shared;
using StorageDLHI.Infrastructor;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using StorageDLHI.App.Common;

namespace StorageDLHI.App.PoGUI
{
    public partial class frmCustomPrintPO : KryptonForm
    {
        private Pos pos = new Pos();
        private DataTable dtForPrint = new DataTable();

        public bool IsPrinted { get; set; } = false;

        public frmCustomPrintPO()
        {
            InitializeComponent();
        }

        public frmCustomPrintPO(Pos pos, DataTable dtForPrint)
        {
            InitializeComponent();
            this.pos = pos;
            this.dtForPrint = dtForPrint; 

            dtPickerCreate.Value = pos.Po_CreateDate;
            dtPickerCreate.Enabled = false;
            dtPickerDelivery.Value = pos.Po_Expected_Delivery_Date;
            txtMPRNo.Text = pos.Po_Mpr_No;
            txtWoNo.Text = pos.Po_Wo_No;
            txtProjectName.Text = pos.Po_Project_Name;
            txtPONo.Text = pos.Po_No;
            txtPrepared.Text = pos.Po_Prepared;
            txtReviewed.Text = pos.Po_Reviewed;
            txtAggrement.Text = pos.Po_Agrement;
            txtApproved.Text = pos.Po_Approved;
            txtPaymentTerm.Text = pos.Po_Payment_Term;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsPrinted = false;
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var supl = await SupplierDAO.GetSupplier(this.pos.SupplierId);


            var placeholders = new Dictionary<string, string>
            {
                { Common.DictionaryKey.MPR_NO, txtMPRNo.Text.Trim() },
                { Common.DictionaryKey.WO_NO, txtWoNo.Text.Trim() },
                { Common.DictionaryKey.PROJECT_NAME, txtProjectName.Text.Trim() },
                { Common.DictionaryKey.PO_NO, txtPONo.Text.Trim() },
                { Common.DictionaryKey.BUYER, txtBuyer.Text.Trim() },
                { Common.DictionaryKey.SUPPLIER_NAME, supl.Name },
                { Common.DictionaryKey.SUPPLIER_CERT, supl.Cert},
                { Common.DictionaryKey.SUPPLIER_TEL, supl.Phone },
                { Common.DictionaryKey.SUPPLIER_EMAIL, supl.Email },
                { Common.DictionaryKey.PAYMENT_TERM, txtPaymentTerm.Text.Trim() },

                { Common.DictionaryKey.DATE_EXPORT, DateTime.Now.ToString("dd/MM/yyyy") },
                { Common.DictionaryKey.PREPARED, txtPrepared.Text.Trim() },
                { Common.DictionaryKey.REVIEWED, txtReviewed.Text.Trim() },
                { Common.DictionaryKey.AGREEMENT, txtAggrement.Text.Trim() },
                { Common.DictionaryKey.APPROVED, txtApproved.Text.Trim() }
            };

            string templatePath = Common.PathManager.GetPathTemplate(Common.PathManager.PO_TEMPLATE_FILE_NAME);

            if (string.IsNullOrEmpty(templatePath))
            {
                MessageBoxHelper.ShowWarning($"Template file not found: {templatePath}");
                LoggerConfig.Logger.Info($"Template file not found: {templatePath} by {ShareData.UserName}");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Excel File",
                Filter = "Excel Files|*.xlsx",
                FileName = $"Report_{txtPONo.Text.Trim()}_{DateTime.Now.ToString("dd.MM.yyyy")}.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string outputPath = saveFileDialog.FileName;

                Common.Common.ExportToExcelTemplate(templatePath, outputPath, this.dtForPrint, placeholders, Enums.ExportToExcel.PO);

                this.Close();
            }

            IsPrinted = true;
            this.Close();
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

        private void txtBuyer_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtBuyer.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtBuyer.Text != cleaned)
            {
                int pos = txtBuyer.SelectionStart - 1;
                txtBuyer.Text = cleaned;
                txtBuyer.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtPaymentTerm_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtPaymentTerm.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtPaymentTerm.Text != cleaned)
            {
                int pos = txtPaymentTerm.SelectionStart - 1;
                txtPaymentTerm.Text = cleaned;
                txtPaymentTerm.SelectionStart = Math.Max(pos, 0);
            }
        }
    }
}
