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

namespace StorageDLHI.App.ImportGUI
{
    public partial class frmUpdateImportedProduct : KryptonForm
    {
        private Guid prodId = Guid.Empty;
        private List<string> prodAdded = new List<string>();


        public List<string> ListUpdateImportProd { get; set; } = new List<string>();
        public bool IsUpdated { get; set; } = true;

        public frmUpdateImportedProduct(Guid prodId, Int32 qty, string prodName)
        {
            InitializeComponent();

            this.prodId = prodId;

            cboWarehouse.DataSource = WarehouseDAO.GetWarehosueForCbo();
            cboWarehouse.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehouse.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;

            txtProdName.Text = prodName.Trim();
            txtRemainingQty.Text = qty.ToString();

            txtQtyImport.Maximum = qty;
        }

        private void UpdateQtyRemaining(bool IsAdd, int rsl)
        {
            if (IsAdd)
            {
                var qtyNew = (Int32.Parse(txtRemainingQty.Text.Trim()) - (Int32)txtQtyImport.Value);
                txtRemainingQty.Text = "" + qtyNew;
                txtQtyImport.Maximum = qtyNew;
                if (qtyNew <= 0)
                {
                    btnAdd.Enabled = false;
                }
            }
            else
            {
                var qtyNew = (Int32.Parse(dgvImportFor.Rows[0].Cells[2].Value.ToString().Trim()) + Int32.Parse(txtRemainingQty.Text.Trim()));
                txtRemainingQty.Text = "" + qtyNew;
                txtQtyImport.Maximum = qtyNew;
                txtQtyImport.Minimum = 1;
                if (qtyNew > 0)
                {
                    btnAdd.Enabled = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvImportFor.Rows.Count <= 0) return;
            int rsl = dgvImportFor.CurrentRow.Index;
            var prodString = this.prodId + "|" + dgvImportFor.Rows[rsl].Cells[4].Value.ToString().Trim();
            prodAdded.Remove(prodString);
            UpdateQtyRemaining(false, rsl);
            dgvImportFor.Rows.RemoveAt(rsl);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var prodString = this.prodId + "|" + cboWarehouse.SelectedValue.ToString();
            if (prodAdded.Contains(prodString))
            {
                if (!MessageBoxHelper.Confirm($"You imported {txtProdName.Text.Trim()} into warehouse {cboWarehouse.Text.Trim()}." +
                    $"Do you want to update quantity for product ?"))
                {
                    return;
                }
                foreach (DataGridViewRow item in dgvImportFor.Rows)
                {
                    if (item.Cells[4].Value.ToString().Equals(cboWarehouse.SelectedValue.ToString()))
                    {
                        item.Cells[2].Value = Int32.Parse(item.Cells[2].Value.ToString()) + txtQtyImport.Value; 
                        UpdateQtyRemaining(true, 0);
                        return;
                    }
                }
            }
            prodAdded.Add(prodString);
            this.dgvImportFor.Rows.Add(this.prodId, txtProdName.Text.Trim(), txtQtyImport.Text.Trim(), 
                    cboWarehouse.Text.Trim(), cboWarehouse.SelectedValue.ToString());
            UpdateQtyRemaining(true, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvImportFor.Rows.Count <= 0 )
            {
                MessageBoxHelper.ShowWarning("You have not updated the quantity for Import product. Please update quantity !");
                return; 
            }
            foreach (DataGridViewRow item in dgvImportFor.Rows)
            {
                var prodInfo = this.prodId + "|" + item.Cells[2].Value.ToString() +
                    "|" + item.Cells[3].Value.ToString() + "|" + item.Cells[4].Value.ToString();
                ListUpdateImportProd.Add(prodInfo);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsUpdated = false;
            this.Close();
        }
    }
}
