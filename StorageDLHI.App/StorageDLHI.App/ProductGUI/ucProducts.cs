using StorageDLHI.App.CustomerGUI;
using StorageDLHI.App.ProjectGUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ProductGUI
{
    public partial class ucProducts : UserControl
    {
        public ucProducts()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddCustomer_Click(object sender, EventArgs e)
        {
            frmCustomerCRUD frmCustomerCRUD = new frmCustomerCRUD(true);
            frmCustomerCRUD.ShowDialog();
        }

        private void tlsAddProject_Click(object sender, EventArgs e)
        {
            frmProjectCRUD frmProjectCRUD = new frmProjectCRUD();
            frmProjectCRUD.ShowDialog();
        }
    }
}
