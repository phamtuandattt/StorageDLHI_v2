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

        }

        private void dgvWarehose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvWarehose.Rows.Count <= 0) { return; }
            int rsl = dgvWarehose.CurrentRow.Index;
            LoadDetailByWId(rsl);
        }
    }
}
