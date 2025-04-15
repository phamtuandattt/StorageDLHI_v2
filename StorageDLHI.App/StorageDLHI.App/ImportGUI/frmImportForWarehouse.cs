using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
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

namespace StorageDLHI.App.ImportGUI
{
    public partial class frmImportForWarehouse : KryptonForm
    {
        public bool IsAdd { get; set; } = true;
        public Int32 Qty { get; set; } = 0;
        public Warehouses Warehouse { get; set; } = new Warehouses();

        public frmImportForWarehouse()
        {
            InitializeComponent();
            cboWarehosue.DataSource = WarehouseDAO.GetWarehosueForCbo();
            cboWarehosue.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehosue.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Warehouse.Warehouse_Name = cboWarehosue.Text.Trim();
            Warehouse.Id = Guid.Parse(cboWarehosue.SelectedValue.ToString());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsAdd = false;
            this.Close();
        }
    }
}
