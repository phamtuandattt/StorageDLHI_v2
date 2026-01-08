using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.ProjectDAO;
using StorageDLHI.BLL.StaffDAO;
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
using System.Threading.Tasks;
using System.Web.Management;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomInfoMPR_V2 : KryptonForm
    {
        private DataTable dtProjects = new DataTable();
        private bool _formIsLoad = false;
        private Projects pModel = null;

        private bool status = true;
        private bool isPrint = false;
        private DataTable dtProdOfMpr = new DataTable();
        private DataTable dtProdOfMprAdd = new DataTable();
        private Mprs dtMprs = new Mprs();
        private DataTable dtForPrint = new DataTable();

        public bool CanelOrConfirm { get; set; } = true; // true is confirm || cancel

        public frmCustomInfoMPR_V2()
        {
            InitializeComponent();
        }

        public frmCustomInfoMPR_V2(string title, bool status, DataTable dtProdOfMpr)
        {
            InitializeComponent();
            this.Text = title;
            this.dtProdOfMpr = dtProdOfMpr;
            this.dtPickerCreate.MinDate = DateTime.Now;
        }

        public frmCustomInfoMPR_V2(string title, bool status, bool isPrint, Mprs dtMprs, DataTable dtForPrint) // status = false; isPrint = true
        {
            InitializeComponent();
            //LoadDataProject();
            //LoadUser();
            this.Text = title;
            this.dtForPrint = dtForPrint;
            this.isPrint = isPrint;
            this.dtMprs = dtMprs;

            txtMPRNo.Text = this.dtMprs.Mpr_No.Trim();
            txtPrepared.Text = this.dtMprs.Mpr_Prepared.Trim();
            cboReview.SelectedValue = this.dtMprs.ReviewedBy;
            cboApproved.SelectedValue = this.dtMprs.ApprovedBy;
            dtPickerCreate.Value = this.dtMprs.CreateDate;
            dtPickerDelivery.Value = this.dtMprs.Expected_Delivery_Date;
            cboProject.SelectedValue = Guid.Parse(this.dtMprs.Project_Id.ToString());

            txtMPRNo.ReadOnly = true;
            txtWoNo.ReadOnly = true;
            //txtProjectName.ReadOnly = true;
            dtPickerCreate.Enabled = false;
        }

        private void frmCustomInfoMPR_V2_Load(object sender, EventArgs e)
        {
            LoadDataProject();
            LoadUser();            
            this.pModel = new Projects();
            txtPrepared.Text = ShareData.UserName;
            this.dtPickerCreate.MinDate = DateTime.Now;
            this.dtPickerDelivery.MinDate = DateTime.Now;

            _formIsLoad = true;
        }

        private async void LoadUser()
        {
            if (!CacheManager.Exists(CacheKeys.USER_DATATABLE_USER_MANAGER_FOR_COMBOBOX))
            {
                var dtS = await StaffDAO.GetStaffManager();
                cboReview.DisplayMember = QueryStatement.PROPERTY_STAFF_NAME;
                cboReview.ValueMember = QueryStatement.PROPERTY_STAFF_ID;
                cboApproved.DisplayMember = QueryStatement.PROPERTY_STAFF_NAME;
                cboApproved.ValueMember = QueryStatement.PROPERTY_STAFF_ID;

                cboReview.DataSource = dtS;
                cboApproved.DataSource = dtS.Copy();

                CacheManager.Add(CacheKeys.USER_DATATABLE_USER_MANAGER_FOR_COMBOBOX, dtS);
            }
            else
            {
                cboReview.DisplayMember = QueryStatement.PROPERTY_STAFF_NAME;
                cboReview.ValueMember = QueryStatement.PROPERTY_STAFF_ID;
                cboApproved.DisplayMember = QueryStatement.PROPERTY_STAFF_NAME;
                cboApproved.ValueMember = QueryStatement.PROPERTY_STAFF_ID;
                cboReview.DataSource = CacheManager.Get<DataTable>(CacheKeys.USER_DATATABLE_USER_MANAGER_FOR_COMBOBOX);
                cboApproved.DataSource = CacheManager.Get<DataTable>(CacheKeys.USER_DATATABLE_USER_MANAGER_FOR_COMBOBOX).Copy();
            }
        }

        private async void LoadDataProject()
        {
            // ----- LOAD DATA FOR PROD OF CREATE MPR
            if (!CacheManager.Exists(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX)
                && !CacheManager.Exists(CacheKeys.PROJECT_DATATABLE))
            {
                var dtCommon = await ProjectDAO.GetProjects();
                if (dtCommon != null && dtCommon.dtProjects != null && dtCommon.dtProjectForCombox != null 
                    && dtCommon.dtProjects.AsEnumerable().Any() && dtCommon.dtProjectForCombox.AsEnumerable().Any())
                {
                    var dtCombobox = dtCommon.dtProjectForCombox;
                    dtProjects = dtCommon.dtProjects;

                    cboProject.DisplayMember = QueryStatement.PROPERTY_PROJECT_NAME;
                    cboProject.ValueMember = QueryStatement.PROPERTY_PROJECT_ID;
                    cboProject.DataSource = dtCombobox;

                    CacheManager.Add(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX, dtCombobox);
                    CacheManager.Add(CacheKeys.PROJECT_DATATABLE, dtProjects);
                }
                else
                {
                    MessageBoxHelper.ShowError("Please add project before create MPRs !");
                    return;
                }
                //this.Close();
            }
            else
            {
                cboProject.DisplayMember = QueryStatement.PROPERTY_PROJECT_NAME;
                cboProject.ValueMember = QueryStatement.PROPERTY_PROJECT_ID;
                cboProject.DataSource = CacheManager.Get<DataTable>(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX);
                this.dtProjects = CacheManager.Get<DataTable>(CacheKeys.PROJECT_DATATABLE);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CanelOrConfirm = false;
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMPRNo.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please enter MPRNo before create !");
                return;
            }

            this.dtProdOfMprAdd = await MprDAO.GetMprDetailForm();
            if (status && !isPrint)
            {
                Mprs mprs = new Mprs()
                {
                    Id = Guid.NewGuid(),
                    Mpr_No = txtMPRNo.Text.Trim(),
                    CreateDate = DateTime.Parse(dtPickerCreate.Value.ToString("dd/MM/yyyy")),
                    Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                    Mpr_Prepared = txtPrepared.Text.Trim(),
                    Mpr_Reviewed = cboReview.Text.Trim(),
                    Mpr_Approved = cboApproved.Text.Trim(),
                    Staff_Id = ShareData.UserId,
                    IsMakePO = false,
                    Project_Id = this.pModel.Id,
                    Mpr_Rev_Total = "",
                    IsCancel = false,
                    CancelBy = "",
                    ReviewedBy = Guid.Parse(cboReview.SelectedValue.ToString()),
                    ApprovedBy = Guid.Parse(cboApproved.SelectedValue.ToString()),
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

                if (await MprDAO.InsertMpr_V2(mprs))
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
                    Id = this.dtMprs.Id,
                    Mpr_No = txtMPRNo.Text.Trim(),
                    Expected_Delivery_Date = DateTime.Parse(dtPickerDelivery.Value.ToString("dd/MM/yyyy")),
                    Mpr_Reviewed = cboReview.Text.Trim(),
                    Mpr_Approved = cboApproved.Text.Trim(),
                    ReviewedBy = Guid.Parse(cboReview.SelectedValue.ToString().Trim()),
                    ApprovedBy = Guid.Parse(cboApproved.SelectedValue.ToString().Trim()),
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
                Guid mprId = this.dtMprs.Id;
                string mpr_no = this.dtMprs.Mpr_No.Trim();
                string wo_no = "";
                string project_name = "";
                var projectId = this.dtMprs.Project_Id;
                foreach (DataRow dataRow in dtProjects.Rows)
                {
                    if (dataRow[0].ToString().Trim().Equals(projectId.ToString()))
                    {
                        project_name = dataRow[QueryStatement.PROPERTY_PROJECT_NAME].ToString().Trim();
                        wo_no = dataRow[QueryStatement.PROPERTY_PROJECT_WO].ToString().Trim();
                        break;
                    }
                }

                var placeholders = new Dictionary<string, string>
                {
                    { Common.DictionaryKey.MPR_NO, mpr_no },
                    { Common.DictionaryKey.WO_NO, wo_no },
                    { Common.DictionaryKey.PROJECT_NAME, project_name },
                    { Common.DictionaryKey.DATE_EXPORT, DateTime.Now.ToString("dd/MM/yyyy") },
                    { Common.DictionaryKey.PREPARED, this.dtMprs.Mpr_Prepared },
                    { Common.DictionaryKey.REVIEWED, this.dtMprs.Mpr_Reviewed },
                    { Common.DictionaryKey.APPROVED, this.dtMprs.Mpr_Approved }
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

        private void cboProject_Validating(object sender, CancelEventArgs e)
        {
            if (cboProject.Items.Count <= 0) return;
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_formIsLoad)
            {
                return;
            }

            var projectId = cboProject.SelectedValue.ToString().Trim();
            foreach (DataRow dataRow in dtProjects.Rows)
            {
                if (dataRow[QueryStatement.PROPERTY_PROJECT_ID].ToString().Trim().Equals(projectId))
                {
                    this.pModel.Id = Guid.Parse(dataRow[QueryStatement.PROPERTY_PROJECT_ID].ToString().Trim());
                    this.pModel.Name = dataRow[QueryStatement.PROPERTY_PROJECT_NAME].ToString().Trim();
                    this.pModel.Code = dataRow[QueryStatement.PROPERTY_PROJECT_CODE].ToString().Trim();
                    this.pModel.ProjectNo = dataRow[QueryStatement.PROPERTY_PROJECT_NO].ToString().Trim();
                    this.pModel.WorkOrderNo = dataRow[QueryStatement.PROPERTY_PROJECT_WO].ToString().Trim();

                    txtWoNo.Text = this.pModel.WorkOrderNo;

                    return;
                }
            }
        }

        private void cboReview_Validating(object sender, CancelEventArgs e)
        {
            if (cboReview.Items.Count <= 0) return;
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboApproved_Validating(object sender, CancelEventArgs e)
        {
            if (cboApproved.Items.Count <= 0) return;
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }
    }
}
