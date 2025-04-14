using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.ImportGUI;
using StorageDLHI.App.MenuGUI.MenuControl;
using StorageDLHI.App.MprGUI;
using StorageDLHI.App.PoGUI;
using StorageDLHI.App.SupplierGUI;
using StorageDLHI.App.WarehouseGUI;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MainGUI
{
    public partial class Main : KryptonForm
    {
        public Main()
        {
            InitializeComponent();

            ShareData.UserName = "David Hoang";
            ShareData.UserId = Guid.NewGuid();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetConnectionString(sender, e);
        }

        private void tlsPO_Click(object sender, EventArgs e)
        {
            ucPOMain ucPOMain = new ucPOMain();
            ucPOMain.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucPOMain);
            ucPOMain.BringToFront();
        }

        private void tlsMPR_Click(object sender, EventArgs e)
        {
            ucMPRMain ucMPRMain = new ucMPRMain();
            ucMPRMain.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucMPRMain);
            ucMPRMain.BringToFront();
        }

        private void tlsImport_Click(object sender, EventArgs e)
        {
            ucImportProd ucImportProd = new ucImportProd();
            ucImportProd.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucImportProd);
            ucImportProd.BringToFront();
        }

        private void tlsExport_Click(object sender, EventArgs e)
        {

        }

        private void tlsInventory_Click(object sender, EventArgs e)
        {

        }

        private void ResetConnectionString(object sender, EventArgs e)
        {
            Properties.Settings.Default.DbConnectionString = "";
            Properties.Settings.Default.Save();

            KryptonMessageBox.Show(NotificationManager.RESET_CONNECTION_STRING, "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void tlsMaterial_Click(object sender, EventArgs e)
        {
            ManageCommonUC ucItems = new ManageCommonUC();
            ucItems.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucItems);
            ucItems.BringToFront();
        }

        private void tlsSuppliers_Click(object sender, EventArgs e)
        {
            ucSuppliers ucSuppliers = new ucSuppliers();
            ucSuppliers.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucSuppliers);
            ucSuppliers.BringToFront();
        }

        private void tlsWarehouses_Click(object sender, EventArgs e)
        {
            frmCustomWarehouse frmCustomWarehouse = new frmCustomWarehouse(TitleManager.WAREHOUSE_ADD, true, new DAL.Models.Warehouses());
            frmCustomWarehouse.ShowDialog();
        }
    }
}
