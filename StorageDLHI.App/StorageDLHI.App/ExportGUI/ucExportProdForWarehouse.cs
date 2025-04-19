using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ExportGUI
{
    public partial class ucExportProdForWarehouse : UserControl
    {
        private DataTable dtWarehouses = new DataTable();
        private DataTable dtWarehouseDetail = new DataTable();

        public ucExportProdForWarehouse()
        {
            InitializeComponent();
            LoadData();

            Common.Common.InitializeFooterGrid(dgvProdOfExport, dgvFooter);
        }

        private void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.WAREHOUSE_DATATABLE_ALL))
            {
                dtWarehouses = WarehouseDAO.GetWarehouses();
                CacheManager.Add(CacheKeys.WAREHOUSE_DATATABLE_ALL, WarehouseDAO.GetWarehouses());
                dgvWarehose.DataSource = dtWarehouses;
            }
            else
            {
                dtWarehouses = CacheManager.Get<DataTable>(CacheKeys.WAREHOUSE_DATATABLE_ALL);
                dgvWarehose.DataSource = dtWarehouses;
            }
            if (dtWarehouses.Rows.Count > 0)
            {
                LoadDetailByWId(0);
            }
        }

        private void LoadDetailByWId(int rsl)
        {
            Guid wId = Guid.Parse(dgvWarehose.Rows[rsl].Cells[0].Value.ToString());
            if (!CacheManager.Exists(string.Format(CacheKeys.WAREHOUSE_DETAIL_BY_ID, wId)))
            {
                dtWarehouseDetail = WarehouseDAO.GetWarehouseDetailByWarehouseId(wId);
                CacheManager.Add(string.Format(CacheKeys.WAREHOUSE_DETAIL_BY_ID, wId), WarehouseDAO.GetWarehouseDetailByWarehouseId(wId));
                dgvRemaningGoods.DataSource = dtWarehouseDetail;
            }
            else
            {
                dtWarehouseDetail = CacheManager.Get<DataTable>(string.Format(CacheKeys.WAREHOUSE_DETAIL_BY_ID, wId));
                dgvRemaningGoods.DataSource = dtWarehouseDetail;
            }
        }

        private void tlsReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvWarehose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvWarehose.Rows.Count <= 0) { return; }
            int rsl = dgvWarehose.CurrentRow.Index;
            LoadDetailByWId(rsl);
        }

        private void dgvWarehose_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvRemaningGoods_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvRemaningGoods_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvRemaningGoods.Columns["PRODUCT_IN_STOCK"].DefaultCellStyle.Format = "N0";
        }

        private void dgvRemaningGoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProdOfExport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvProdOfExport_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Apply the changed width to the corresponding column in the footer
            if (e.Column.Index < dgvFooter.Columns.Count)
            {
                dgvFooter.Columns[e.Column.Index].Width = e.Column.Width;
            }

            // Resize for DataGridViewMain and DataGridViewFooter the same
            Common.Common.AdjustFooterScrollbar(dgvProdOfExport, dgvFooter);
        }

        private void dgvProdOfExport_Scroll(object sender, ScrollEventArgs e)
        {
            dgvFooter.HorizontalScrollingOffset = dgvProdOfExport.HorizontalScrollingOffset;
        }

        private void dgvProdOfExport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProdOfExport.Columns["QTY_PROD_FOR_EXPORT"].DefaultCellStyle.Format = "N0";
        }

        private void UpdateFooter()
        {
            Int32 totalQty = 0;

            foreach (DataGridViewRow row in dgvProdOfExport.Rows)
            {
                if (Int32.TryParse(row.Cells[12].Value?.ToString(), out Int32 qty))
                {
                    totalQty += qty;
                }
            }

            dgvFooter.Rows[0].Cells[2].Value = "TOTAL";
            dgvFooter.Rows[0].Cells[12].Value = totalQty.ToString("N0");

            dgvFooter.Rows[0].Cells[2].Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Common.Common.StyleFooterCell(dgvFooter.Rows[0].Cells[12]);
        }

        private void dgvRemaningGoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRemaningGoods.Rows.Count < 0) { return; }
            int rsl = dgvRemaningGoods.CurrentRow.Index;

            Guid prodId = Guid.Parse(dgvRemaningGoods.Rows[rsl].Cells[1].Value.ToString());
            string prodName = dgvRemaningGoods.Rows[rsl].Cells[4].Value.ToString();
            Int32 qty = Int32.Parse(dgvRemaningGoods.Rows[rsl].Cells[14].Value.ToString());

            frmExportProduct frmExportProduct = new frmExportProduct(prodId, qty, prodName);
            frmExportProduct.ShowDialog();

            if (!frmExportProduct.IsExported)
            {
                return;
            }
        }
    }
}
