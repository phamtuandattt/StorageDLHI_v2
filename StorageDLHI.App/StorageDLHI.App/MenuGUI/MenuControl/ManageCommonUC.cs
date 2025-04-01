using StorageDLHI.App.Common;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.Infrastructor.Caches;
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
            LoadData();
        }

        private void LoadData()
        {
            LoadOrigins();
            LoadMaterialTypes();
            LoadStandards();
        }

        private void LoadOrigins ()
        {
            if (!CacheManager.Exists(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN))
            {
                var dtOr = MaterialDAO.GetOrigins();
                CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, dtOr);
                dgvOrigins.DataSource = dtOr;
            }
            else
            {
                dgvOrigins.DataSource = CacheManager.Get<DataTable>(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN);
            }
        }

        private void LoadMaterialTypes ()
        {
            if (!CacheManager.Exists(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE))
            {
                var dtType = MaterialDAO.GetMaterialTypes();
                CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, dtType);
                dgvMaterialTypes.DataSource = dtType;
            }
            else
            {
                dgvMaterialTypes.DataSource = CacheManager.Get<DataTable>(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE);
            }
        }

        private void LoadStandards()
        {
            if (!CacheManager.Exists(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD))
            {
                var dtSt = MaterialDAO.GetMaterialStandards();
                CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, dtSt);
                dgvStandards.DataSource = dtSt;
            }
            else
            {
                dgvStandards.DataSource = CacheManager.Get<DataTable>(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD);
            }
        }

        private void btnAddOrigins_Click(object sender, EventArgs e)
        {
            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.ORIGIN_ADD_TITLE, Enums.Materials.Origins, true, null);
            frmAdd.ShowDialog();

            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, MaterialDAO.GetOrigins());

            LoadData();
        }

        private void tlsAddMaterialTypes_Click(object sender, EventArgs e)
        {
            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.MATERIAL_TYPE_ADD_TITLE, Enums.Materials.Types, true, null);
            frmAdd.ShowDialog();
            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, MaterialDAO.GetMaterialTypes());
            LoadData();
        }

        private void tlsAddStandard_Click(object sender, EventArgs e)
        {
            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.STANDARD_ADD_TITLE, Enums.Materials.Standards, true, null);
            frmAdd.ShowDialog();
            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, MaterialDAO.GetMaterialStandards());
            LoadData();
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

        private void tlsLoadOrigins_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tlsLoadTypes_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tlsLoadStandard_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvOrigins_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrigins.Rows.Count <= 0) return;
            int rsl = dgvOrigins.CurrentRow.Index;

            Origins origins = new Origins()
            {
                Id = Guid.Parse(dgvOrigins.Rows[rsl].Cells[0].Value.ToString()),
                Origin_Code = dgvOrigins.Rows[rsl].Cells[1].Value.ToString(),
                Origin_Des = dgvOrigins.Rows[rsl].Cells[2].Value.ToString()
            };

            MaterialCommonDto dtos = new MaterialCommonDto()
            {
                Origins = origins,
            };

            frmAddMaterials frmAdd = new frmAddMaterials("Update Origin", Enums.Materials.Origins, false, dtos);
            frmAdd.ShowDialog();
            LoadData();
        }

        private void dgvStandards_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStandards.Rows.Count <= 0) return;
            int rsl = dgvStandards.CurrentRow.Index;

            Material_Standards material_Standards = new Material_Standards()
            {
                Id = Guid.Parse(dgvStandards.Rows[rsl].Cells[0].Value.ToString()),
                Standard_Code = dgvStandards.Rows[rsl].Cells[1].Value.ToString(),
                Standard_Des = dgvStandards.Rows[rsl].Cells[2].Value.ToString()
            };

            MaterialCommonDto dtos = new MaterialCommonDto()
            {
                MaterialStandard = material_Standards,
            };

            frmAddMaterials frmAdd = new frmAddMaterials("Update Material Standards", Enums.Materials.Standards, false, dtos);
            frmAdd.ShowDialog();
            LoadData();
        }

        private void dgvMaterialTypes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMaterialTypes.Rows.Count <= 0) return;
            int rsl = dgvMaterialTypes.CurrentRow.Index;

            Material_Types material_Types = new Material_Types()
            {
                Id = Guid.Parse(dgvMaterialTypes.Rows[rsl].Cells[0].Value.ToString()),
                Type_Code = dgvMaterialTypes.Rows[rsl].Cells[1].Value.ToString(),
                Type_Des = dgvMaterialTypes.Rows[rsl].Cells[2].Value.ToString()
            };

            MaterialCommonDto dtos = new MaterialCommonDto()
            {
                MaterialType = material_Types,
            };

            frmAddMaterials frmAdd = new frmAddMaterials("Update Material Type", Enums.Materials.Types, false, dtos);
            frmAdd.ShowDialog();
            LoadData();
        }
    }
}
