using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.CustomerGUI;
using StorageDLHI.BLL.CustomerDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.BLL.ProjectDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ProjectGUI
{
    public partial class frmProjectCRUD : KryptonForm
    {
        public frmProjectCRUD()
        {
            InitializeComponent();
        }

        private async void frmProjectCRUD_Load(object sender, EventArgs e)
        {
            await LoadCboCus();

            txtName.Text = ShareData.UserId + " - " + ShareData.DepCode;
        }

        private async Task LoadCboCus()
        {
            var dtCus = await CustomerDAO.GetCusForCbo();
            if (dtCus != null && dtCus.Rows.Count > 0)
            {
                cboCustomer.DataSource = dtCus;
                cboCustomer.DisplayMember = QueryStatement.PROPERTY_CUSTOMER_NAME;
                cboCustomer.ValueMember = QueryStatement.PROPERTY_CUSTOMER_ID;
            }
            else
            {
                MessageBoxHelper.ShowWarning("Please add Customer before create project !");
                this.Close();
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Name of Project before create!");
                txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtProjectNo.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Project No of Project before create!");
                txtProjectNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtProjectCode.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill in Project Code of Project before create!");
                txtProjectCode.Focus();
                return;
            }

            Projects projects = new Projects()
            {
                Id = Guid.NewGuid(),
                Name = txtName.Text,
                Code = txtProjectCode.Text,
                ProjectNo = txtProjectNo.Text,
                ProductInfo = txtProjectInfo.Text,
                Weight = decimal.Parse(txtWeight.Text.Trim()),
                CustomerId = Guid.Parse(cboCustomer.SelectedValue.ToString().Trim()),
            };

            if (await ShowDialogManager.WithLoader(() => ProjectDAO.Insert(projects)))
            {
                MessageBoxHelper.ShowInfo("Add project success !");
                this.Close();
            }
            else
            {
                MessageBoxHelper.ShowError("Add project fail !");
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void lblAddCus_Click(object sender, EventArgs e)
        {
            frmCustomerCRUD frmCustomerCRUD = new frmCustomerCRUD(true);
            frmCustomerCRUD.ShowDialog();

            await LoadCboCus();
        }

        private void cboCustomer_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }
    }
}
