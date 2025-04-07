using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomInfoMpr : KryptonForm
    {
        private bool status = true;
        private DataTable dtProdOfMpr = new DataTable();
        public frmCustomInfoMpr()
        {
            InitializeComponent();

            this.dtPickerCreate.MinDate = DateTime.Now;
        }

        public frmCustomInfoMpr(string title, bool status, DataTable dtProdOfMpr)
        {
            InitializeComponent();
            this.Text = title;
            this.dtProdOfMpr = dtProdOfMpr;
            this.dtPickerCreate.MinDate = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Mprs mprs = new Mprs()
            {
                Id = Guid.NewGuid(),
                Mpr_No = txtMPRNo.Text.Trim(),
                Mpr_Wo_No = txtWoNo.Text.Trim(),
                Mpr_Project_Name_Code = txtProjectName.Text.Trim(),
                CreateDate = DateTime.Parse(dtPickerCreate.Value.ToString("dd/MM/yyyy")),
                Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                Mpr_Prepared = txtPrepared.Text.Trim(),
                Mpr_Reviewed = txtReviewed.Text.Trim(),
                Mpr_Approved = txtApproved.Text.Trim(),
            };

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
