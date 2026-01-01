using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.CustomerGUI;
using StorageDLHI.App.Emp_DepGUI;
using StorageDLHI.App.ExportGUI;
using StorageDLHI.App.ImportGUI;
using StorageDLHI.App.MenuGUI.MenuControl;
using StorageDLHI.App.MprGUI;
using StorageDLHI.App.PoGUI;
using StorageDLHI.App.ProductGUI;
using StorageDLHI.App.ProjectGUI;
using StorageDLHI.App.SupplierGUI;
using StorageDLHI.App.WarehouseGUI;
using StorageDLHI.BLL.StaffDAO;
using StorageDLHI.Infrastructor;
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
        private CustomToolStripRenderer _renderer;

        public Main()
        {
            InitializeComponent();

            _renderer = new CustomToolStripRenderer();
            tlsMenuButton.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            tlsMenuButton.Renderer = _renderer;

            //_renderer.ActiveButton = tlsMenuButton.Items[0] as ToolStripButton;
            //tlsMenuButton.Invalidate();


            foreach (ToolStripItem item in tlsMenuButton.Items)
            {
                if (item is ToolStripButton btn)
                {
                    btn.MouseEnter += ToolStripButton_MouseEnter;
                    btn.MouseLeave += ToolStripButton_MouseLeave;
                }
            }
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            var infos = Properties.Settings.Default.RememberLogin.Split('|');
            ShareData.UserId = Guid.Parse(infos[0]);
            ShareData.UserName = infos[1];

            LoggerConfig.Logger.Info($"Login by: {infos[1]} - {infos[2]} - {infos[0]}");

            await GetEmpLogin(Guid.Parse(infos[0]));

            if (ShareData.DepCode.Trim() == "AD")
            {
                empDepToolStripMenuItem.Visible = true;
            }
        }

        private async Task GetEmpLogin(Guid empId)
        {
            var empLogin = await StaffDAO.GetEmpLogin(empId);
            ShareData.DepCode = empLogin.DepCode;
        }

        private void ToolStripButton_MouseLeave(object sender, EventArgs e)
        {
            if (_renderer.HoveredButton != null)
            {
                _renderer.HoveredButton = null;
                tlsMenuButton.Invalidate();
            }
        }

        private void ToolStripButton_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as ToolStripButton;
            if (_renderer.HoveredButton != btn)
            {
                _renderer.HoveredButton = btn;
                tlsMenuButton.Invalidate();
            }
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
            //SetActiveButton(tlsPO);
            var btn = sender as ToolStripButton;
            _renderer.ActiveButton = btn;
            tlsMenuButton.Invalidate();

            ucPOMain ucPOMain = new ucPOMain();
            ucPOMain.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucPOMain);
            ucPOMain.BringToFront();
        }

        private void tlsMPR_Click(object sender, EventArgs e)
        {
            //SetActiveButton(tlsMPR);
            var btn = sender as ToolStripButton;
            _renderer.ActiveButton = btn;
            tlsMenuButton.Invalidate();

            ucMPRMain ucMPRMain = new ucMPRMain();
            ucMPRMain.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucMPRMain);
            ucMPRMain.BringToFront();
        }

        private void tlsImport_Click(object sender, EventArgs e)
        {
            //SetActiveButton(tlsImport);
            var btn = sender as ToolStripButton;
            _renderer.ActiveButton = btn;
            tlsMenuButton.Invalidate();

            ucImportProd ucImportProd = new ucImportProd();
            ucImportProd.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucImportProd);
            ucImportProd.BringToFront();
        }

        private void tlsExport_Click(object sender, EventArgs e)
        {
            //SetActiveButton(tlsExport);
            var btn = sender as ToolStripButton;
            _renderer.ActiveButton = btn;
            tlsMenuButton.Invalidate();

            ucExportProdForWarehouse ucExport = new ucExportProdForWarehouse();
            ucExport.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucExport);
            ucExport.BringToFront();
        }

        private void tlsInventory_Click(object sender, EventArgs e)
        {
            //SetActiveButton(tlsInventory);
            var btn = sender as ToolStripButton;
            _renderer.ActiveButton = btn;
            tlsMenuButton.Invalidate();
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

        private void reloadCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.Common.ReloadAllCache();
        }


        private void SetActiveButton(ToolStripButton activeButton)
        {
            foreach (ToolStripItem item in tlsMenuButton.Items)
            {
                if (item is ToolStripButton btn)
                {
                    if (btn == activeButton)
                    {
                        //// Apply "active" style
                        //btn.BackColor = Color.FromArgb(179, 215, 243);
                        ////btn.ForeColor = Color.White;
                        //btn.Font = new Font(btn.Font, FontStyle.Bold);
                        _renderer.ActiveButton = activeButton;
                        tlsMenuButton.Invalidate();
                    }
                    else
                    {
                        // Reset to normal style
                        btn.BackColor = Color.White;
                        btn.ForeColor = SystemColors.ControlText;
                        btn.Font = new Font(btn.Font, FontStyle.Regular);
                    }
                }
            }
        }


        private void projectManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProjectCRUD projectCRUD = new frmProjectCRUD();
            projectCRUD.ShowDialog();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomerCRUD frmCustomerCRUD = new frmCustomerCRUD(true);
            frmCustomerCRUD.ShowDialog();
        }

        private void empDepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucEmpDepManage ucEmpDep = new ucEmpDepManage();
            ucEmpDep.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucEmpDep);
            ucEmpDep.BringToFront();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucProducts ucProducts = new ucProducts();
            ucProducts.Dock = DockStyle.Fill;
            pnMain.Controls.Add(ucProducts);
            ucProducts.BringToFront();
        }
    }
}
