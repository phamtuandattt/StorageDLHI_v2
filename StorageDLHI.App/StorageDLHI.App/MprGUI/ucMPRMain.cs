using StorageDLHI.App.Common;
using StorageDLHI.App.ProductGUI;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace StorageDLHI.App.MprGUI
{
    public partial class ucMPRMain : UserControl
    {
        private int TotalProd = 0;
        private List<Guid> prodsAdded = new List<Guid>();
        private DataTable dtProds = new DataTable();
        private DataTable dtProdsOfMprs = new DataTable();

        public ucMPRMain()
        {
            InitializeComponent();
            LoadData();

            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_ID);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_NAME);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_DES_2);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_CODE);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_MATERIAL_CODE);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_A);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_B);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_C);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_D);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_E);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_F);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_G);
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_UNIT_CODE);
            dtProdsOfMprs.Columns.Add("QTY");
            dtProdsOfMprs.Columns.Add(QueryStatement.PROPERTY_PROD_USAGE);

            dgvProdExistMpr.DataSource = dtProdsOfMprs;
        }

        private void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR))
            {
                dtProds = ProductDAO.GetProductsForCreateMPR();
                CacheManager.Add(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR, dtProds);
                dgvProds.DataSource = dtProds;
            }
            else
            {
                dgvProds.DataSource = CacheManager.Get<DataTable>(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR);
                dtProds = CacheManager.Get<DataTable>(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR);
            }
        }

        private void btnAddProd_Click(object sender, EventArgs e)
        {
            frmCustomProd frmCustomProd = new frmCustomProd(TitleManager.PROD_ADD_TITLE, true, null);
            frmCustomProd.ShowDialog();

            LoadData();
            // Overwrite cache Products
            CacheManager.Add(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR, ProductDAO.GetProductsForCreateMPR());
        }

        private void dgvProds_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProds.Rows.Count <= 0) return;
            int rsl = dgvProds.CurrentRow.Index;

            Guid prodId = Guid.Parse(dgvProds.Rows[rsl].Cells[0].Value.ToString());

            if (prodsAdded.Contains(prodId))
            {
                if (!MessageBoxHelper.Confirm($"You added product [{dgvProds.Rows[rsl].Cells[1].Value.ToString()}]into MPRs. " +
                    $"\nDo you want to add or update quantity ?"))
                {
                    return;
                }
                // Remove exist row in MPRs
                // Also delete from the DataTable (dtProdsOfMprs)
                DataRow[] rowsToDelete = dtProdsOfMprs.Select($"ID = '{prodId}'");
                foreach (DataRow r in rowsToDelete)
                {
                    dtProdsOfMprs.Rows.Remove(r);
                }

                // Remove from tracking list
                prodsAdded.Remove(prodId);
            }

            frmGetQty frmGetQty = new frmGetQty();
            frmGetQty.ShowDialog();

            int qtyAdd = frmGetQty.Qty;
            if (qtyAdd <= 0)
            {
                return;
            }
            string usageNote = frmGetQty.UsageNote;

            DataRow dataRow = dtProdsOfMprs.NewRow();
            dataRow[0] = dgvProds.Rows[rsl].Cells[0].Value.ToString();
            dataRow[1] = dgvProds.Rows[rsl].Cells[1].Value.ToString().Trim();
            dataRow[2] = dgvProds.Rows[rsl].Cells[2].Value.ToString().Trim().ToUpper();
            dataRow[3] = dgvProds.Rows[rsl].Cells[3].Value.ToString().Trim().ToUpper();
            dataRow[4] = dgvProds.Rows[rsl].Cells[4].Value.ToString().Trim().ToUpper();
            dataRow[5] = dgvProds.Rows[rsl].Cells[6].Value.ToString().Trim();
            dataRow[6] = dgvProds.Rows[rsl].Cells[7].Value.ToString().Trim();
            dataRow[7] = dgvProds.Rows[rsl].Cells[8].Value.ToString().Trim();
            dataRow[8] = dgvProds.Rows[rsl].Cells[9].Value.ToString().Trim();
            dataRow[9] = dgvProds.Rows[rsl].Cells[10].Value.ToString().Trim();
            dataRow[10] = dgvProds.Rows[rsl].Cells[11].Value.ToString().Trim();
            dataRow[11] = dgvProds.Rows[rsl].Cells[12].Value.ToString().Trim();
            dataRow[12] = dgvProds.Rows[rsl].Cells[13].Value.ToString().Trim();
            dataRow[13] = qtyAdd;
            dataRow[14] = usageNote;

            dtProdsOfMprs.Rows.Add(dataRow);
            prodsAdded.Add(Guid.Parse(dgvProds.Rows[rsl].Cells[0].Value.ToString()));
            dgvProdExistMpr.Rows[0].Selected = true;
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

        private void dgvProds_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void dgvProdOfMPRs_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void updateProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProds.Rows.Count <= 0) return;
            int rsl = dgvProds.CurrentRow.Index;

            var imgArr = dgvProds.Rows[rsl].Cells[5].Value.ToString().Trim().Length > 0 && dgvProds.Rows[rsl].Cells[5].Value.ToString().Trim() != null
                ? (byte[])dgvProds.Rows[rsl].Cells[5].Value : new byte[100];

            Products prod = new Products()
            {
                Id = Guid.Parse(dgvProds.Rows[rsl].Cells[0].Value.ToString()),
                Product_Name = dgvProds.Rows[rsl].Cells[1].Value.ToString().Trim(),
                Product_Des_2 = dgvProds.Rows[rsl].Cells[2].Value.ToString().Trim().ToUpper(),
                Product_Code = dgvProds.Rows[rsl].Cells[3].Value.ToString().Trim().ToUpper(),
                Product_Material_Code = dgvProds.Rows[rsl].Cells[4].Value.ToString().Trim(),
                Image = imgArr, //5
                A_Thinhness = dgvProds.Rows[rsl].Cells[6].Value.ToString().Trim(),
                B_Depth = dgvProds.Rows[rsl].Cells[7].Value.ToString().Trim(),
                C_Witdh = dgvProds.Rows[rsl].Cells[8].Value.ToString().Trim(),
                D_Web = dgvProds.Rows[rsl].Cells[9].Value.ToString().Trim(),
                E_Flag = dgvProds.Rows[rsl].Cells[10].Value.ToString().Trim(),
                F_Length = dgvProds.Rows[rsl].Cells[11].Value.ToString().Trim(),
                G_Weight = dgvProds.Rows[rsl].Cells[12].Value.ToString().Trim(),
                // Unit Code: 13
                Used_Note = dgvProds.Rows[rsl].Cells[14].Value.ToString().Trim(),
                PictureLink = dgvProds.Rows[rsl].Cells[15].Value.ToString().Trim(),
                UnitId = Guid.Parse(dgvProds.Rows[rsl].Cells[16].Value.ToString().Trim()),
                Origin_Id = Guid.Parse(dgvProds.Rows[rsl].Cells[17].Value.ToString().Trim()),
                M_Type_Id = Guid.Parse(dgvProds.Rows[rsl].Cells[18].Value.ToString().Trim()),
                Stand_Id = Guid.Parse(dgvProds.Rows[rsl].Cells[19].Value.ToString().Trim()),
            };

            frmCustomProd frmCustomProd = new frmCustomProd(TitleManager.PROD_UPDATE_TITLE, false, prod);
            frmCustomProd.ShowDialog();

            // Overwrite cache Products
            CacheManager.Add(CacheKeys.PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR, ProductDAO.GetProductsForCreateMPR());
            LoadData();
        }


        // Selected row when right click
        private void dgvProds_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvProds.Rows.Count <= 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.dgvProds.ClearSelection();
                    this.dgvProds.Rows[rowSelected].Selected = true;
                }
            }
        }

        private void dgvProdExistMpr_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RenderNumbering(sender, e);
        }

        private void updateQuantityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdExistMpr.Rows.Count <= 0) return;
            int rsl = dgvProdExistMpr.CurrentRow.Index;

            if (!MessageBoxHelper.Confirm("Do you want to update quantity ?"))
            {
                return;
            }
            frmGetQty frmGetQty = new frmGetQty();
            frmGetQty.ShowDialog();

            dgvProdExistMpr.Rows[rsl].Cells[13].Value = frmGetQty.Qty;
        }

        private void dgvProdExistMpr_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvProdExistMpr.Rows.Count <= 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.dgvProdExistMpr.ClearSelection();
                    this.dgvProdExistMpr.Rows[rowSelected].Selected = true;
                }
            }
        }

        private void deleteProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdExistMpr.Rows.Count <= 0) return;
            int rsl = dgvProdExistMpr.CurrentRow.Index;

            // Ask for confirmation before deleting
            string prodName = dgvProdExistMpr.Rows[rsl].Cells[1].Value.ToString();
            if (!MessageBoxHelper.Confirm($"Do you want to delete product [{prodName}] out of MPRs ?"))
            {
                return;
            }

            DeleteProdOfMprs(rsl);
        }

        private void tlsDeleteProdExist_Click(object sender, EventArgs e)
        {
            if (dgvProdExistMpr.Rows.Count <= 0) return;
            int rsl = dgvProdExistMpr.CurrentRow.Index;
            // Ask for confirmation before deleting
            string prodName = dgvProdExistMpr.Rows[rsl].Cells[1].Value.ToString();
            if (!MessageBoxHelper.Confirm($"Do you want to delete product [{prodName}] out of MPRs ?"))
            {
                return;
            }
            DeleteProdOfMprs(rsl);
        }

        private void DeleteProdOfMprs(int rsl)
        {
            // Get the Guid of the product from the selected row
            Guid prodId = Guid.Parse(dgvProdExistMpr.Rows[rsl].Cells[0].Value.ToString());

            if (prodsAdded.Contains(prodId))
            {
                // Also delete from the DataTable (dtProdsOfMprs)
                DataRow[] rowsToDelete = dtProdsOfMprs.Select($"ID = '{prodId}'");
                foreach (DataRow row in rowsToDelete)
                {
                    dtProdsOfMprs.Rows.Remove(row);
                }

                // Remove from tracking list
                prodsAdded.Remove(prodId);
            }
        }

        private void txtSearchProd_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtSearchProd.Text.Length == 0)
            {
                //dgvProds.DataSource = this.dtProds;
                dgvProds.Refresh();
            }
            DataView dv = dtProds.DefaultView;
            dv.RowFilter = $"{QueryStatement.PROPERTY_PROD_NAME} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_DES_2} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_CODE} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_MATERIAL_CODE} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_A} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_B} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_C} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_D} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_E} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_F} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_G} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_USAGE} LIKE '%{txtSearchProd.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_UNIT_CODE} LIKE '%{txtSearchProd.Text}%' ";

            dgvProds.DataSource = dv;
        }

        private void txtSearchProdExistMPR_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtSearchProdExistMPR.Text.Length == 0)
            {
                dgvProdExistMpr.Refresh();
            }
            DataView dv = dtProdsOfMprs.DefaultView;
            dv.RowFilter = $"{QueryStatement.PROPERTY_PROD_NAME} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_DES_2} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_CODE} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_MATERIAL_CODE} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_A} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_B} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_C} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_D} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_E} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_F} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_G} LIKE '%{txtSearchProdExistMPR.Text}%' " +
                $"OR {QueryStatement.PROPERTY_PROD_UNIT_CODE} LIKE '%{txtSearchProdExistMPR.Text}%' ";

            dgvProdExistMpr.DataSource = dv;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dtProdsOfMprs.Rows.Count <= 0 && dgvProdExistMpr.Rows.Count <= 0)
            {
                MessageBoxHelper.ShowWarning("Please add product to create MPRs !");
                return; 
            }

            frmCustomInfoMpr frmCustomInfoMpr = new frmCustomInfoMpr(TitleManager.MPR_ADD_INFO, true, dtProdsOfMprs);
            frmCustomInfoMpr.ShowDialog();
        }
    }
}
