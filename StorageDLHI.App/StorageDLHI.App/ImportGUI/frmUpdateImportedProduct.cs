using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.WarehouseDAO;
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
    public partial class frmUpdateImportedProduct : KryptonForm
    {
        private List<string> prodAdded = new List<string>();
        private DataTable dtWarehouseForComboBox = new DataTable();

        public Guid ProdId { get; set; } = Guid.Empty;

        public List<string> ListUpdateImportProd { get; set; } = new List<string>();
        public bool IsUpdated { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; } = false;

        public frmUpdateImportedProduct(Guid prodId, Int32 qty, string prodName)
        {
            InitializeComponent();

            this.ProdId = prodId;

            LoadDataForCombobox();

            txtProdName.Text = prodName.Trim();
            txtRemainingQty.Text = qty.ToString();

            txtQtyImport.Maximum = qty;
        }

        private async void LoadDataForCombobox()
        {
            cboWarehouse.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehouse.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;
            if (!CacheManager.Exists(CacheKeys.WAREHOUSE_DATATABLE_ALL_FOR_COMBOXBOX))
            {
                dtWarehouseForComboBox = await WarehouseDAO.GetWarehosueForCbo();
                if (dtWarehouseForComboBox.Rows.Count > 0)
                {
                    cboWarehouse.DataSource = dtWarehouseForComboBox;
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Please add Warehouse before Import products !");
                    IsUpdated = false;
                    this.Close();
                }
            }
            else
            {
                dtWarehouseForComboBox = CacheManager.Get<DataTable>(CacheKeys.WAREHOUSE_DATATABLE_ALL_FOR_COMBOXBOX);
                cboWarehouse.DataSource = dtWarehouseForComboBox;
            }
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
            var prodString = this.ProdId + "|" + dgvImportFor.Rows[rsl].Cells[4].Value.ToString().Trim();
            prodAdded.Remove(prodString);
            UpdateQtyRemaining(false, rsl);
            dgvImportFor.Rows.RemoveAt(rsl);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var prodString = this.ProdId + "|" + cboWarehouse.SelectedValue.ToString();
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
            this.dgvImportFor.Rows.Add(this.ProdId, txtProdName.Text.Trim(), Int32.Parse(txtQtyImport.Value.ToString().Trim()), 
                    cboWarehouse.Text.Trim(), Guid.Parse(cboWarehouse.SelectedValue.ToString()));
            UpdateQtyRemaining(true, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(txtRemainingQty.Text.Trim()) > 0)
            {
                MessageBoxHelper.ShowWarning("Please enter all products !");
                return;
            }    
            if (dgvImportFor.Rows.Count <= 0 )
            {
                MessageBoxHelper.ShowWarning("You have not updated the quantity for Import product. Please update quantity !");
                return; 
            }
            foreach (DataGridViewRow item in dgvImportFor.Rows)
            {
                var prodInfo = this.ProdId + "|" + Int32.Parse(item.Cells[2].Value.ToString()) +
                    "|" + item.Cells[3].Value.ToString() + "|" + Guid.Parse(item.Cells[4].Value.ToString());
                ListUpdateImportProd.Add(prodInfo);
            }

            IsCompleted = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsUpdated = false;
            this.Close();
        }

        private void dgvImportFor_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }
    }
}
