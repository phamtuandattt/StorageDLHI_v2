using StorageDLHI.App.Common;
using StorageDLHI.App.ProductGUI;
using StorageDLHI.BLL.ProductDAO;
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
    public partial class ucMPRMain : UserControl
    {
        public ucMPRMain()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dgvProds.DataSource = ProductDAO.GetProductsForCreateMPR();
        }

        private void btnAddProd_Click(object sender, EventArgs e)
        {
            frmCustomProd frmCustomProd = new frmCustomProd(TitleManager.PROD_ADD_TITLE, true, null);
            frmCustomProd.ShowDialog();

            LoadData();
        }


    }
}
