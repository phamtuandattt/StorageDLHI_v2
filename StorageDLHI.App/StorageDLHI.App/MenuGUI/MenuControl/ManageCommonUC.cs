using StorageDLHI.BLL.SupplierDAO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MenuGUI.MenuControl
{
    public partial class ManageCommonUC : UserControl
    {
        public ManageCommonUC()
        {
            InitializeComponent();

            var data = SupplierDAO.GetSuppliers();
            dgvOrigins.DataSource = data;

            var dataa = SupplierDAO.GetSupplierBanks();
            dgvUnits.DataSource = dataa;


        }

        private void btnAddOrigins_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddMaterialTypes_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddStandard_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddTax_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddCost_Click(object sender, EventArgs e)
        {

        }

        private void tlsAddUnits_Click(object sender, EventArgs e)
        {

        }

        private void dgvOrigins_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvMaterialTypes_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvStandards_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvUnits_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvCost_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvTax_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
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
    }
}
