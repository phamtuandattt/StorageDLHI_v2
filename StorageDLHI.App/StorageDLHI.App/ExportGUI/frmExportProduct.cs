using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.WarehouseDAO;
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

namespace StorageDLHI.App.ExportGUI
{
    public partial class frmExportProduct : KryptonForm
    {
        private Guid prodId = Guid.Empty;
        private List<string> prodAdded = new List<string>();

        public bool IsExported { get; set; } = true;

        public frmExportProduct()
        {
            InitializeComponent();
        }

        public frmExportProduct(Guid prodId, Int32 qty, string prodName)
        {
            InitializeComponent();
            this.prodId = prodId;
            this.Text = "Export product";
            
            cboWarehouse.DataSource = WarehouseDAO.GetWarehosueForCbo();
            cboWarehouse.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehouse.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;

            txtProdName.Text = prodName;
            txtRemainingQty.Text = qty.ToString("N0").Trim();

            txtQtyExport.Maximum = qty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsExported = false;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvExportForWarehouse.Rows.Count <= 0) return;
            int rsl = dgvExportForWarehouse.CurrentRow.Index;
            var prodString = this.prodId + "|" + dgvExportForWarehouse.Rows[rsl].Cells[4].Value.ToString().Trim();
            prodAdded.Remove(prodString);
            UpdateQtyRemaining(false, rsl);
            dgvExportForWarehouse.Rows.RemoveAt(rsl);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var prodString = this.prodId + "|" + cboWarehouse.SelectedValue.ToString();
            if (prodAdded.Contains(prodString))
            {
                if (!MessageBoxHelper.Confirm($"You exported {txtProdName.Text.Trim()} into warehouse {cboWarehouse.Text.Trim()}." +
                    $"Do you want to update quantity for product ?"))
                {
                    return;
                }
                foreach (DataGridViewRow item in dgvExportForWarehouse.Rows)
                {
                    if (item.Cells[4].Value.ToString().Equals(cboWarehouse.SelectedValue.ToString()))
                    {
                        item.Cells[2].Value = Int32.Parse(item.Cells[2].Value.ToString()) + txtQtyExport.Value;
                        UpdateQtyRemaining(true, 0);
                        return;
                    }
                }
            }
            prodAdded.Add(prodString);
            this.dgvExportForWarehouse.Rows.Add(this.prodId, txtProdName.Text.Trim(), Int32.Parse(txtQtyExport.Value.ToString().Trim()),
                    cboWarehouse.Text.Trim(), Guid.Parse(cboWarehouse.SelectedValue.ToString()));
            UpdateQtyRemaining(true, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void UpdateQtyRemaining(bool IsAdd, int rsl)
        {
            if (IsAdd)
            {
                var qtyNew = (Convert.ToInt32(txtRemainingQty.Text.Trim().Replace(",", "")) - (Int32)txtQtyExport.Value);
                txtRemainingQty.Text = "" + qtyNew;
                txtQtyExport.Maximum = qtyNew;
                if (qtyNew <= 0)
                {
                    btnAdd.Enabled = false;
                }
            }
            else
            {
                var qtyNew = (Int32.Parse(dgvExportForWarehouse.Rows[0].Cells[2].Value.ToString().Trim()) + Int32.Parse(txtRemainingQty.Text.Trim()));
                txtRemainingQty.Text = qtyNew.ToString("N0");
                txtQtyExport.Maximum = qtyNew;
                txtQtyExport.Minimum = 1;
                if (qtyNew > 0)
                {
                    btnAdd.Enabled = true;
                }
            }
        }
    }
}
