using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
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
        private DataTable dtProdOfMprAdd = MprDAO.GetMprDetailForm();

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

            if (MprDAO.InsertMpr(mprs))
            {
                if (MprDAO.InsertMprDetail(this.dtProdOfMprAdd))
                {
                    MessageBoxHelper.ShowInfo("Create MPRs success !");
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
