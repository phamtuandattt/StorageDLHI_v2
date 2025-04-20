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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomInfoMpr : KryptonForm
    {
        private bool status = true;
        private DataTable dtProdOfMpr = new DataTable();
        private DataTable dtProdOfMprAdd = new DataTable(); 
        private Mprs dtMPRDetail = new Mprs();
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
            txtWoNo.Text = this.dtMPRDetail.Mpr_Wo_No.Trim();
            txtProjectName.Text = this.dtMPRDetail.Mpr_Project_Name_Code.Trim();
            txtPrepared.Text = this.dtMPRDetail.Mpr_Prepared.Trim();
            txtReviewed.Text = this.dtMPRDetail.Mpr_Reviewed.Trim();
            txtApproved.Text = this.dtMPRDetail.Mpr_Approved.Trim();
            dtPickerCreate.Value = this.dtMPRDetail.CreateDate;
            dtPickerDelivery.Value = this.dtMPRDetail.Expected_Delivery_Date;

            txtMPRNo.ReadOnly = true;
            txtWoNo.ReadOnly = true;
            txtProjectName.ReadOnly = true;
            dtPickerCreate.Enabled = false;
        }

        public frmCustomInfoMpr(string title, bool status, DataTable dtProdOfMpr)
        {
            InitializeComponent();
            this.Text = title;
            this.dtProdOfMpr = dtProdOfMpr;
            this.dtPickerCreate.MinDate = DateTime.Now;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            this.dtProdOfMprAdd = await MprDAO.GetMprDetailForm();
            if (status)
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
            else
            {
                Mprs mprs = new Mprs()
                {
                    Id = this.dtMPRDetail.Id,
                    Mpr_No = txtMPRNo.Text.Trim(),
                    Mpr_Wo_No = txtWoNo.Text.Trim(),
                    Mpr_Project_Name_Code = txtProjectName.Text.Trim(),
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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CanelOrConfirm = false;
            this.Close();
        }
    }
}
