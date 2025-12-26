using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ImportGUI
{
    public partial class frmImportForWarehouse : KryptonForm
    {
        public bool IsAdd { get; set; } = true;
        public Int32 Qty { get; set; } = 0;
        public Warehouses Warehouse { get; set; } = new Warehouses();

        private DataTable dtWarehouseForComboBox = new DataTable();

        public frmImportForWarehouse(string title, Int32 maxQty)
        {
            InitializeComponent();
            cboWarehosue.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehosue.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;
            LoadDataForCombobox();
            if (dtWarehouseForComboBox.Rows.Count < 0 )
            {
                MessageBoxHelper.ShowWarning("Please add Warehouse before Import products !");
                IsAdd = false;
                this.Close();   
            }
            this.Text = title;
            txtQtyProd.Maximum = maxQty;
        }

        private async void LoadDataForCombobox()
        {
            if (!CacheManager.Exists(CacheKeys.WAREHOUSE_DATATABLE_ALL_FOR_COMBOXBOX))
            {
                dtWarehouseForComboBox = await WarehouseDAO.GetWarehosueForCbo();
                if (dtWarehouseForComboBox.Rows.Count > 0)
                {
                    cboWarehosue.DataSource = dtWarehouseForComboBox;
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Please add Warehouse before Import products !");
                    IsAdd = false;
                    this.Close();
                }
            }
            else
            {
                dtWarehouseForComboBox = CacheManager.Get<DataTable>(CacheKeys.WAREHOUSE_DATATABLE_ALL_FOR_COMBOXBOX);
                cboWarehosue.DataSource= dtWarehouseForComboBox;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Warehouse.Warehouse_Name = cboWarehosue.Text.Trim();
            Warehouse.Id = Guid.Parse(cboWarehosue.SelectedValue.ToString());
            Qty = (Int32)txtQtyProd.Value;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsAdd = false;
            this.Close();
        }

        private void cboWarehosue_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }
    }
}
