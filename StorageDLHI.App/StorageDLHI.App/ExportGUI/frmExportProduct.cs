using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.ExportDAO;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Shared;
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
        private Warehouses warehouse = new Warehouses();
        private DataTable dtWarehouseDetail = new DataTable();
        private DataTable dtDeliveryProd = new DataTable();

        public bool IsExported { get; set; } = true;
        
        public List<string> ListProductExport { get; set; } = new List<string>();

        public frmExportProduct()
        {
            InitializeComponent();
        }

        public frmExportProduct(Guid prodId, Int32 qty, string prodName, Warehouses warehouse)
        {
            InitializeComponent();
            this.prodId = prodId;
            this.Text = "Export product";
            this.warehouse = warehouse;

            dtWarehouseDetail.Columns.Add("ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("WAREHOUSE_ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("PRODUCT_ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("PRODUCT_IN_STOCK", typeof(Int32));

            dtDeliveryProd.Columns.Add(QueryStatement.PROPERTY_DELIVERY_DETAIL_ID, typeof(Guid));
            dtDeliveryProd.Columns.Add(QueryStatement.PROPERTY_DELIVERY_DETAIL_PRODUCT_ID, typeof(Guid));
            dtDeliveryProd.Columns.Add(QueryStatement.PROPERTY_DELIVERY_DETAIL_PRODUCT_PRODUCT_ID, typeof(Guid));
            dtDeliveryProd.Columns.Add(QueryStatement.PROPERTY_DELIVERY_DETAIL_FROM_WAREHOUSE_ID, typeof(Guid));
            dtDeliveryProd.Columns.Add(QueryStatement.PROPERTY_DELIVERY_DETAIL_QTY, typeof(Int32));

            lblWarehouse.Text = $"Q'ty of {this.warehouse.Warehouse_Name}:";

            LoadDataComboxBox();

            txtProdName.Text = prodName;
            txtRemainingQty.Text = qty.ToString("N0").Trim();

            txtQtyExport.Maximum = qty;
        }

        private async void LoadDataComboxBox()
        {
            cboWarehouse.DataSource = await WarehouseDAO.GetWarehouseForCboOfExportProd(this.warehouse.Id);
            cboWarehouse.DisplayMember = QueryStatement.PROPERTY_WAREHOUSE_NAME;
            cboWarehouse.ValueMember = QueryStatement.PROPERTY_WAREHOUSE_ID;
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvExportForWarehouse.Rows.Count <= 0)
            {
                MessageBoxHelper.ShowWarning("You have not updated the quantity for Import product. Please update quantity !");
                return;
            }

            Int32 totalExport = 0;
            // Create model to update qty prod
            foreach (DataGridViewRow item in dgvExportForWarehouse.Rows)
            {
                DataRow dataRow = dtWarehouseDetail.NewRow();
                dataRow[0] = Guid.NewGuid();
                dataRow[1] = Guid.Parse(item.Cells[4].Value.ToString());
                dataRow[2] = Guid.Parse(item.Cells[0].Value.ToString());
                dataRow[3] = Int32.Parse(item.Cells[2].Value.ToString());
                totalExport += Int32.Parse(item.Cells[2].Value.ToString());
                dtWarehouseDetail.Rows.Add(dataRow);
            }
            var whDetailModel = new Warehouse_Detail()
            {
                Warehosue_Id = this.warehouse.Id,
                ProductId = this.prodId,
                Product_In_Stock = totalExport
            };

            if (dtWarehouseDetail.Rows.Count <= 0)
            {
                MessageBoxHelper.ShowWarning("Please add product need to export !");
                return;
            }

            // create model insert Delivery
            var exportM = new Delivery_Products()
            {
                Id = Guid.NewGuid(),
                DeliveryDate = DateTime.Now,
                DeliveryDay = DateTime.Now.Day,
                DeliveryMonth = DateTime.Now.Month,
                DeliveryYear = DateTime.Now.Year,
                Delivery_Total_Qty = totalExport,
                From_Warehouse_Id = this.warehouse.Id,
                Staff_Id = ShareData.UserId,
            };
            foreach (DataGridViewRow item in dgvExportForWarehouse.Rows)
            {
                DataRow dataRow = dtDeliveryProd.NewRow();
                dataRow[0] = Guid.NewGuid();
                dataRow[1] = exportM.Id;
                dataRow[2] = Guid.Parse(item.Cells[0].Value.ToString());
                dataRow[3] = Guid.Parse(item.Cells[4].Value.ToString());
                dataRow[4]= Int32.Parse(item.Cells[2].Value.ToString());
                dtDeliveryProd.Rows.Add(dataRow);
            }

            if (await WarehouseDAO.UpdateQtyProdOfWarehouse(dtWarehouseDetail, whDetailModel)
                && await ShowDialogManager.WithLoader(() => ExportProductDAO.InsertDelivery(exportM, dtDeliveryProd)))
            {
                MessageBoxHelper.ShowInfo("Updated quantity product success !");
                this.Close();
            }
            else
            {
                MessageBoxHelper.ShowWarning("Updated quantity product fail !");
            }
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
