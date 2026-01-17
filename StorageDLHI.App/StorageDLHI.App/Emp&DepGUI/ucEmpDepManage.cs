using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.BLL.StaffDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Emp_DepGUI
{
    public partial class ucEmpDepManage : UserControl
    {
        private Panel pnNoDataStaff = new System.Windows.Forms.Panel();

        public ucEmpDepManage()
        {
            InitializeComponent();
            ucPanelNoData ucPanelNoData = new ucPanelNoData("No result match !");
            pnNoDataStaff = ucPanelNoData.pnlNoData;
            dgvEmpsOfDep.Controls.Add(pnNoDataStaff);
        }

        private async void ucEmpDepManage_Load(object sender, EventArgs e)
        {
            await LoadDeps();
            var dtStaffs = await LoadEmpOfDep(Guid.Parse(cboDep.ComboBox.SelectedValue.ToString().Trim()));
            if (dtStaffs != null && dtStaffs.Rows.Count > 0)
            {
                dgvEmpsOfDep.DataSource = dtStaffs;
                Common.Common.HideNoDataPanel(pnNoDataStaff);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvEmpsOfDep, pnNoDataStaff);
            }
        }

        private async Task LoadDeps()
        {
            var dt = await StaffDAO.GetDeps();
            cboDep.ComboBox.DisplayMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_NAME_CODE;
            cboDep.ComboBox.ValueMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_ID;
            cboDep.ComboBox.DataSource = dt;
            cboDep.ComboBox.SelectedIndex = 0;
        }

        private async Task<DataTable> LoadEmpOfDep(Guid DepId)
        {
            if (!string.IsNullOrEmpty(DepId.ToString().Trim())) 
            {
                return await StaffDAO.GetStaffsOfDeP(DepId);
            }
            return null;
        }

        private async void cboDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDep.Items.Count > 0)
            {
                var dtStaffs = await LoadEmpOfDep(Guid.Parse(cboDep.ComboBox.SelectedValue.ToString().Trim()));
                if (dtStaffs != null && dtStaffs.Rows.Count > 0)
                {
                    dgvEmpsOfDep.DataSource = dtStaffs;
                    Common.Common.HideNoDataPanel(pnNoDataStaff);
                }
                else
                {
                    Common.Common.ShowNoDataPanel(dgvEmpsOfDep, pnNoDataStaff);
                }

            }
        }

        private void dgvEmpsOfDep_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void tlsRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            frmCustomerEmp frmCustomerEmp = new frmCustomerEmp();
            frmCustomerEmp.ShowDialog();
        }

        private async void updateEmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEmpsOfDep.Rows.Count <= 0) return;
            var rsl = dgvEmpsOfDep.CurrentRow.Index;

            var mS = new Staffs()
            {
                Id = Guid.Parse(dgvEmpsOfDep.Rows[rsl].Cells[0].Value.ToString().Trim()),
                Name = dgvEmpsOfDep.Rows[rsl].Cells[2].Value.ToString().Trim(),
                DepartmentId = Guid.Parse(dgvEmpsOfDep.Rows[rsl].Cells[5].Value.ToString().Trim()),
                Staff_RoleId = Guid.Parse(dgvEmpsOfDep.Rows[rsl].Cells[3].Value.ToString().Trim())
            };

            frmCustomerEmp frmCustomerEmp = new frmCustomerEmp(true, mS);
            frmCustomerEmp.ShowDialog();

            var dtStaffs = await LoadEmpOfDep(Guid.Parse(cboDep.ComboBox.SelectedValue.ToString().Trim()));
            if (dtStaffs != null && dtStaffs.Rows.Count > 0)
            {
                dgvEmpsOfDep.DataSource = dtStaffs;
                Common.Common.HideNoDataPanel(pnNoDataStaff);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvEmpsOfDep, pnNoDataStaff);
            }
        }
    }
}
