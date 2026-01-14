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
            LoadTaxs();
            LoadUnits();
            LoadCosts();
        }

        private async void LoadOrigins ()
        {
            if (!CacheManager.Exists(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN))
            {
                var dtOr = await MaterialDAO.GetOrigins();
                CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, dtOr);
                dgvOrigins.DataSource = dtOr;
            }
            else
            {
                dgvOrigins.DataSource = CacheManager.Get<DataTable>(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN);
            }
        }

        private async void LoadMaterialTypes ()
        {
            if (!CacheManager.Exists(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE))
            {
                var dtType = await MaterialDAO.GetMaterialTypes();
                CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, dtType);
                dgvMaterialTypes.DataSource = dtType;
            }
            else
            {
                dgvMaterialTypes.DataSource = CacheManager.Get<DataTable>(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE);
            }
        }

        private async void LoadStandards()
        {
            if (!CacheManager.Exists(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD))
            {
                var dtSt = await MaterialDAO.GetMaterialStandards();
                CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, dtSt);
                dgvStandards.DataSource = dtSt;
            }
            else
            {
                dgvStandards.DataSource = CacheManager.Get<DataTable>(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD);
            }
        }

        private async void LoadTaxs()
        {
            if (!CacheManager.Exists(CacheKeys.TAX_DATATABLE_ALLTAX))
            {
                var dtTax = await MaterialDAO.GetTaxs();
                CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, dtTax);
                dgvTax.DataSource = dtTax;
            }
            else
            {
                dgvTax.DataSource = CacheManager.Get<DataTable>(CacheKeys.TAX_DATATABLE_ALLTAX);
            }
        }

        private async void LoadUnits()
        {
            if (!CacheManager.Exists(CacheKeys.UNIT_DATATABLE_ALLUNIT))
            {
                var dtUnit = await MaterialDAO.GetUnits();
                CacheManager.Add(CacheKeys.UNIT_DATATABLE_ALLUNIT, dtUnit);
                dgvUnits.DataSource = dtUnit;
            }
            else
            {
                dgvUnits.DataSource = CacheManager.Get<DataTable>(CacheKeys.UNIT_DATATABLE_ALLUNIT);
            }
        }

        private async void LoadCosts()
        {
            if (!CacheManager.Exists(CacheKeys.COST_DATATABLE_ALLCOST))
            {
                var dtCost = await ShowDialogManager.WithLoader(() => MaterialDAO.GetCosts());
                CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, dtCost);
                dgvCost.DataSource = dtCost;
            }
            else
            {
                dgvCost.DataSource = CacheManager.Get<DataTable>(CacheKeys.COST_DATATABLE_ALLCOST);
            }
        }

        private async void btnAddOrigins_Click(object sender, EventArgs e)
        {
            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.ORIGIN_ADD_TITLE, Enums.Materials.Origins, true, null);
            frmAdd.ShowDialog();

            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN, await MaterialDAO.GetOrigins());

            LoadData();
        }

        private async void tlsAddMaterialTypes_Click(object sender, EventArgs e)
        {
            //frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.MATERIAL_TYPE_ADD_TITLE, Enums.Materials.Types, true, null);
            //frmAdd.ShowDialog();

            frmAddMaterial_V2 frmAddMaterial_V2 = new frmAddMaterial_V2();
            frmAddMaterial_V2.ShowDialog();

            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, await MaterialDAO.GetMaterialTypes());
            LoadData();
        }

        private async void tlsAddStandard_Click(object sender, EventArgs e)
        {
            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.STANDARD_ADD_TITLE, Enums.Materials.Standards, true, null);
            frmAdd.ShowDialog();
            // Overwrite cache after add item
            CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, await MaterialDAO.GetMaterialStandards());
            LoadData();
        }

        private async void tlsAddTax_Click(object sender, EventArgs e)
        {
            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.TAX_ADD_TITLE, Enums.TaxUnitCost.Tax, true, null);
            frm.ShowDialog();
            CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, await MaterialDAO.GetTaxs());
            LoadData();

        }

        private async void tlsAddCost_Click(object sender, EventArgs e)
        {
            //frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.COST_ADD_TITLE, Enums.TaxUnitCost.Cost, true, null);
            //frm.ShowDialog();
            frmAddCost frmAdd = new frmAddCost(TitleManager.COST_ADD_TITLE, Enums.TaxUnitCost.Cost, true, null);
            frmAdd.ShowDialog();
            CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, await MaterialDAO.GetCosts());
            LoadData();
        }

        private async void tlsAddUnits_Click(object sender, EventArgs e)
        {
            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.UNIT_ADD_TITLE, Enums.TaxUnitCost.Unit, true, null);
            frm.ShowDialog();
            CacheManager.Add(CacheKeys.UNIT_DATATABLE_ALLUNIT, await MaterialDAO.GetUnits());
            LoadData();
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

        private async void dgvOrigins_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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

            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.ORIGIN_UPDATE_TITLE, Enums.Materials.Origins, false, dtos);
            frmAdd.ShowDialog();
            CacheManager.Add(CacheKeys.ORIGIN_DATATABLE_ALLORIGIN,await MaterialDAO.GetOrigins());
            LoadData();
        }

        private async void dgvStandards_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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

            frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.MATERIAL_TYPE_UPDATE_TITLE, Enums.Materials.Standards, false, dtos);
            frmAdd.ShowDialog();
            CacheManager.Add(CacheKeys.STANDARD_DATATABLE_ALLSTANDARD, await MaterialDAO.GetMaterialStandards());
            LoadData();
        }

        private async void dgvMaterialTypes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvMaterialTypes.Rows.Count <= 0) return;
            //int rsl = dgvMaterialTypes.CurrentRow.Index;

            //Material_Types material_Types = new Material_Types()
            //{
            //    Id = Guid.Parse(dgvMaterialTypes.Rows[rsl].Cells[0].Value.ToString()),
            //    Type_Code = dgvMaterialTypes.Rows[rsl].Cells[1].Value.ToString(),
            //    Type_Des = dgvMaterialTypes.Rows[rsl].Cells[2].Value.ToString()
            //};

            //MaterialCommonDto dtos = new MaterialCommonDto()
            //{
            //    MaterialType = material_Types,
            //};

            //frmAddMaterials frmAdd = new frmAddMaterials(TitleManager.STANDARD_UPDATE_TITLE, Enums.Materials.Types, false, dtos);
            //frmAdd.ShowDialog();

            //Material_Type_Detail model = new Material_Type_Detail()
            //{
            //    Material_Types_Id = Guid.Parse(dgvMaterialTypes.Rows[rsl].Cells[3].Value.ToString().Trim())
            //};

            //frmAddMaterial_V2 frmAddMaterial_V2 = new frmAddMaterial_V2(true, model);
            //frmAddMaterial_V2.ShowDialog();

            //CacheManager.Add(CacheKeys.MATERIAL_TYPE_DATATABLE_ALLTYPE, await MaterialDAO.GetMaterialTypes());
            //LoadData();
        }

        private async void dgvCost_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCost.Rows.Count <= 0) return;
            int rsl = dgvCost.CurrentRow.Index;

            Costs costs = new Costs()
            {
                Id = Guid.Parse(dgvCost.Rows[rsl].Cells[0].Value.ToString()),
                Cost_Name = dgvCost.Rows[rsl].Cells[1].Value.ToString(),
                Currency_code = dgvCost.Rows[rsl].Cells[2].Value.ToString(),
                Currency_Value = decimal.Parse( dgvCost.Rows[rsl].Cells[3].Value.ToString()),
                Currency = dgvCost.Rows[rsl].Cells[4].Value.ToString(),
            };

            TaxUnitCostDto dtos = new TaxUnitCostDto()
            {
                Costs = costs,
            };

            //frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.COST_UPDATE_TITLE, TaxUnitCost.Cost, false, dtos);
            //frm.ShowDialog();
            frmAddCost frmAdd = new frmAddCost(TitleManager.COST_UPDATE_TITLE, Enums.TaxUnitCost.Cost, false, dtos);
            frmAdd.ShowDialog();
            CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, await MaterialDAO.GetCosts());
            LoadData();
        }

        private async void dgvUnits_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUnits.Rows.Count <= 0) return;
            int rsl = dgvUnits.CurrentRow.Index;

            Units units = new Units()
            {
                Id = Guid.Parse(dgvUnits.Rows[rsl].Cells[0].Value.ToString()),
                Unit_Code = dgvUnits.Rows[rsl].Cells[1].Value.ToString()
            };

            TaxUnitCostDto dtos = new TaxUnitCostDto()
            {
                Units = units,
            };

            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.UNIT_ADD_TITLE, TaxUnitCost.Unit, false, dtos);
            frm.ShowDialog();
            CacheManager.Add(CacheKeys.UNIT_DATATABLE_ALLUNIT, await MaterialDAO.GetUnits());
            LoadData();
        }

        private async void dgvTax_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTax.Rows.Count <= 0) return;
            int rsl = dgvTax.CurrentRow.Index;

            Taxs taxs = new Taxs()
            {
                Id = Guid.Parse(dgvTax.Rows[rsl].Cells[0].Value.ToString()),
                Tax_Percent = dgvTax.Rows[rsl].Cells[1].Value.ToString(),
                Tax_Value = float.Parse(dgvTax.Rows[rsl].Cells[2].Value.ToString())
            };

            TaxUnitCostDto dtos = new TaxUnitCostDto()
            {
                Taxs = taxs,
            };

            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.TAX_UPDATE_TITLE, TaxUnitCost.Tax, false, dtos);
            frm.ShowDialog();
            CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, await MaterialDAO.GetTaxs());
            LoadData();
        }

        private void dgvCost_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvCost.Columns["CURRENCY_VALUE"].DefaultCellStyle.Format = "N2";
        }
    }
}
