using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.CustomerGUI;
using StorageDLHI.BLL.CustomerDAO;
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

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
