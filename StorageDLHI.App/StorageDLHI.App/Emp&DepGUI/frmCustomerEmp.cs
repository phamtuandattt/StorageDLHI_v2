using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.StaffDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Emp_DepGUI
{
    public partial class frmCustomerEmp : KryptonForm
    {
        private bool _isShowPwd = false;
        private DataTable dtDeps = new DataTable();
        private DataTable dtStaffRoles = new DataTable();

        public frmCustomerEmp()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            cboDep.ComboBox.ValueMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_ID;
            cboDep.ComboBox.DisplayMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_NAME_CODE;

            cboRole.ComboBox.ValueMember = QueryStatement.PROPERTY_STAFF_ROLE_ID;
            cboRole.ComboBox.DisplayMember = QueryStatement.PROPERTY_STAFF_ROLE_NAME;
        }

        private async void frmCustomerEmp_Load(object sender, EventArgs e)
        {
            await LoadDep();
            await LoadStaffRole();

            if (dtDeps.Rows.Count <= 0 || dtStaffRoles.Rows.Count <= 0)
            {
                MessageBoxHelper.ShowWarning("Please contact admin to resole problem !");
                return;
            }

            cboDep.DataSource = dtDeps;
            cboRole.DataSource = dtStaffRoles;
        }

        private async Task LoadDep()
        {
            this.dtDeps = await StaffDAO.GetDeps();    
        }

        private async Task LoadStaffRole()
        {
            this.dtStaffRoles = await StaffDAO.GetStaffRole();
        }

        private void btnShowPwd_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = _isShowPwd;
            btnShowPwd.Values.Image = _isShowPwd ? Properties.Resources.eye : Properties.Resources.hidden;
            _isShowPwd = !_isShowPwd;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var dbName = Properties.Settings.Default.Database.Trim();
            Staffs staffs = new Staffs()
            {
                Id = Guid.NewGuid(),
                Name = txtUserName.Text.Trim(),
                Staff_Pwd = txtPassword.Text.Trim(),
                Staff_Code = txtUserName.Text.Trim().ToUpper(),
                DeviceName = System.Environment.MachineName,
                DepartmentId = Guid.Parse(cboDep.SelectedValue.ToString().Trim()),
                Staff_RoleId = Guid.Parse(cboRole.SelectedValue.ToString().Trim())
            };

            if (await StaffDAO.CreateNewUser(staffs, dbName))
            {
                MessageBoxHelper.ShowInfo("User Created !");
                this.Close();
                return;
            }
            else
            {
                MessageBoxHelper.ShowError("Create user failed !");
            }
        }
    }
}
