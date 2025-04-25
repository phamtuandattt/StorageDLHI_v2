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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim())
                || string.IsNullOrEmpty(txtWarehouseCode.Text.Trim())
                || string.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill in the information completely !");
                return;
            }

            if (_isAdd)
            {
                Warehouses model = new Warehouses()
                {
                    Id = Guid.NewGuid(),
                    Warehouse_Code = txtWarehouseCode.Text.Trim().ToUpper(),
                    Warehouse_Name = txtName.Text.Trim(),
                    Warehouse_Address = txtAddress.Text.Trim(),
                };
                
                if (await WarehouseDAO.Insert(model))
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

        private void txtWarehouseCode_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWarehouseCode.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtWarehouseCode.Text != cleaned)
            {
                int pos = txtWarehouseCode.SelectionStart - 1;
                txtWarehouseCode.Text = cleaned;
                txtWarehouseCode.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtName.Text, Infrastructor.Commons.Common.REGEX_VALID_NAME, "");
            if (txtName.Text != cleaned)
            {
                int pos = txtName.SelectionStart - 1;
                txtName.Text = cleaned;
                txtName.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtAddress.Text, Infrastructor.Commons.Common.REGEX_VALID_VIETNAM_ADDRESS, "");
            if (txtAddress.Text != cleaned)
            {
                int pos = txtAddress.SelectionStart - 1;
                txtAddress.Text = cleaned;
                txtAddress.SelectionStart = Math.Max(pos, 0);
            }
        }
    }
}
