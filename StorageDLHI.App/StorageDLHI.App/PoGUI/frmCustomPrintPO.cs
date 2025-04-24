using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            string templatePath = Common.PathManager.PO_TEMAPLATE_PATH;

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
    }
}
