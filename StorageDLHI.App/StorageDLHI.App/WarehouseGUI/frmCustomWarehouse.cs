using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.WarehouseGUI
{
    public partial class frmCustomWarehouse : KryptonForm
    {
        private Warehouses _warehouse = new Warehouses();
        private bool _isAdd = true;

        public frmCustomWarehouse()
        {
            InitializeComponent();
        }

        public frmCustomWarehouse(string title, bool status, Warehouses warehouses)
        {
            InitializeComponent();
            this.Text = title;
            this._isAdd = status;
            this._warehouse = warehouses;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_isAdd)
            {
                Warehouses model = new Warehouses()
                {
                    Id = Guid.NewGuid(),
                    Warehouse_Code = txtWarehouseCode.Text.Trim().ToUpper(),
                    Warehouse_Name = txtName.Text.Trim(),
                    Warehouse_Address = txtAddress.Text.Trim(),
                };
                
                if (WarehouseDAO.Insert(model))
                {
                    MessageBoxHelper.ShowInfo("Add Warehouse success !");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Add Warehouse fail !");
                }
            }
            else
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
