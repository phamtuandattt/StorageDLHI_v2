using StorageDLHI.BLL.SupplierDAO;
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

namespace StorageDLHI.App.SupplierGUI
{
    public partial class ucSuppliers : UserControl
    {
        public ucSuppliers()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER))
            {
                var dtSp = SupplierDAO.GetSuppliers();
                CacheManager.Add(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER, dtSp);
                dgvSuppliers.DataSource = dtSp;
            }
            else
            {
                dgvSuppliers.DataSource = CacheManager.Get<DataTable>(CacheKeys.SUPPLIER_DATATABLE_ALL_SUPPLIER);
            }
        }

        private void LoadBankBySupplier(Guid supplierId)
        {
            if (!CacheManager.Exists(string.Format(CacheKeys.BANK_DETAIL_SUPPLIER_ID, supplierId)))
            {
                var dtBankBySup = SupplierDAO.GetBankBySupplier(supplierId);
                CacheManager.Add(string.Format(CacheKeys.BANK_DETAIL_SUPPLIER_ID, supplierId), dtBankBySup);
                dgvBanks.DataSource = dtBankBySup;
            }
            else
            {
                dgvBanks.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.BANK_DETAIL_SUPPLIER_ID, supplierId));
            }
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {

        }

        private void tlsLoadSupplier_Click(object sender, EventArgs e)
        {

        }

        private void btnAddBank_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadBank_Click(object sender, EventArgs e)
        {

        }

        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSuppliers.Rows.Count <= 0) return;
            int rsl = dgvSuppliers.CurrentRow.Index;

            LoadBankBySupplier(Guid.Parse(dgvSuppliers.Rows[rsl].Cells[0].Value.ToString()));
        }

        private void RenderNumbering(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dgvSuppliers_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }
    }
}
