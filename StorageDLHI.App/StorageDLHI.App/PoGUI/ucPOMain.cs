using StorageDLHI.App.Common;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.Models;
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

namespace StorageDLHI.App.PoGUI
{
    public partial class ucPOMain : UserControl
    {
        private int TotalProd = 0;
        private List<Guid> prodsAdded = new List<Guid>();
        private DataTable dtMprs = new DataTable();
        private DataTable dtMprDetailById = new DataTable();

        private DataTable dtProds = new DataTable();
        private DataTable dtProdsOfAddPO = new DataTable();

        private int rslOld;
        private int previousRowIndex = -1;


        public ucPOMain()
        {
            InitializeComponent();
            LoadData();

            // Create columns DataTable ProdsOfAddPO
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_ID);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_NAME);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_DES_2);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_CODE);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_MATERIAL_CODE);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_A);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_B);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_C);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_D);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_E);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_F);
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_G);
            dtProdsOfAddPO.Columns.Add("QTY");

            dgvProdOfPO.DataSource = dtProdsOfAddPO;

        }

        private void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.MPRS_DATATABLE_ALL_MPRS))
            {
                dtMprs = MprDAO.GetMprs();
                CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS, dtMprs);
                dgvMPRs.DataSource = dtMprs;
            }
            else
            {
                dgvMPRs.DataSource = CacheManager.Get<DataTable>(CacheKeys.MPRS_DATATABLE_ALL_MPRS);
            }

            if (dtMprs.Rows.Count > 0 && dtMprs != null)
            {
                var mprId = Guid.Parse(dgvMPRs.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId)))
                {
                    dtMprDetailById = MprDAO.GetMprDetailByMpr(mprId);
                    CacheManager.Add(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId), dtMprDetailById);
                    dgvMPRDetail.DataSource = dtMprDetailById;
                }
                else
                {
                    dgvMPRDetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId));
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {

        }

        private void btnClearProdsOfPO_Click(object sender, EventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0) return;
            int rsl = dgvProdOfPO.CurrentRow.Index;

            if (!MessageBoxHelper.Confirm($"Do you want delete all product of PO?"))
            {
                return;
            }

            tlsMPRNo.Text = "...";
            dtProdsOfAddPO.Clear();
            prodsAdded.Clear();
            dgvProdOfPO.Refresh();
        }

        private void dgvMPRs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMPRs.Rows.Count <= 0) { return; }
            int rsl = dgvMPRs.CurrentRow.Index;

            if (dgvProdOfPO.Rows.Count > 0) 
            {
                if (MessageBoxHelper.Confirm($"You are performing PO creation for MPR No: [{dgvMPRs.Rows[this.previousRowIndex].Cells[1].Value.ToString().Trim()}].\n" +
                    $"Do you want to cancel the current operation?"))
                {
                    tlsMPRNo.Text = "...";
                    dtProdsOfAddPO.Clear();
                    prodsAdded.Clear();
                    dgvProdOfPO.Refresh();
                    dgvMPRs.Rows[rsl].Selected = true;
                }
                else
                {
                    tlsMPRNo.Text = $"MPR No: [{dgvMPRs.Rows[this.previousRowIndex].Cells[1].Value.ToString().Trim()}]\t";
                    dgvMPRs.Rows[this.previousRowIndex].Selected = true;
                    return;
                }
            }
            
            var mprId = Guid.Parse(dgvMPRs.Rows[rsl].Cells[0].Value.ToString());
            if (!CacheManager.Exists(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId)))
            {
                dtMprDetailById = MprDAO.GetMprDetailByMpr(mprId);
                CacheManager.Add(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId), dtMprDetailById);
                dgvMPRDetail.DataSource = dtMprDetailById;
            }
            else
            {
                dgvMPRDetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID, mprId));
            }
        }

        private void dgvMPRDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMPRDetail.Rows.Count <= 0) return;
            int rsl = dgvMPRDetail.CurrentRow.Index;
            tlsMPRNo.Text = $"MPR No: [{dgvMPRs.Rows[this.previousRowIndex].Cells[1].Value.ToString().Trim()}]\t";

            Guid prodId = Guid.Parse(dgvMPRDetail.Rows[rsl].Cells[0].Value.ToString());

            if (prodsAdded.Contains(prodId))
            {
                MessageBoxHelper.ShowWarning($"You added product [{dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString()}] into PO. ");
                return;
                // Remove exist row in PO
                // Also delete from the DataTable (dtProdsOfAddPO)
                //DataRow[] rowsToDelete = dtProdsOfAddPO.Select($"ID = '{prodId}'");
                //foreach (DataRow r in rowsToDelete)
                //{
                //    dtProdsOfAddPO.Rows.Remove(r);
                //}

                //// Remove from tracking list
                //prodsAdded.Remove(prodId);
            }

            DataRow dataRow = dtProdsOfAddPO.NewRow();
            dataRow[0] = prodId;
            dataRow[1] = dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString().Trim();
            dataRow[2] = dgvMPRDetail.Rows[rsl].Cells[4].Value.ToString().Trim().ToUpper();
            dataRow[3] = "";
            dataRow[4] = dgvMPRDetail.Rows[rsl].Cells[5].Value.ToString().Trim().ToUpper();
            dataRow[5] = dgvMPRDetail.Rows[rsl].Cells[6].Value.ToString().Trim();
            dataRow[6] = dgvMPRDetail.Rows[rsl].Cells[7].Value.ToString().Trim();
            dataRow[7] = dgvMPRDetail.Rows[rsl].Cells[8].Value.ToString().Trim();
            dataRow[8] = dgvMPRDetail.Rows[rsl].Cells[9].Value.ToString().Trim();
            dataRow[9] = dgvMPRDetail.Rows[rsl].Cells[10].Value.ToString().Trim();
            dataRow[10] = dgvMPRDetail.Rows[rsl].Cells[11].Value.ToString().Trim();
            dataRow[11] = dgvMPRDetail.Rows[rsl].Cells[12].Value.ToString().Trim();
            dataRow[12] = dgvMPRDetail.Rows[rsl].Cells[13].Value.ToString().Trim();

            dtProdsOfAddPO.Rows.Add(dataRow);
            prodsAdded.Add(prodId);
            dgvProdOfPO.Rows[0].Selected = true;
        }

        private void dgvMPRs_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvMPRDetail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvProdOfPO_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void updateProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0) return;
            int rsl = dgvProdOfPO.CurrentRow.Index;

            int qty = int.Parse(dgvProdOfPO.Rows[rsl].Cells[12].Value.ToString());

            Products product = new Products()
            {
                Id = Guid.Parse(dgvProdOfPO.Rows[rsl].Cells[0].Value.ToString()),
                A_Thinhness = dgvProdOfPO.Rows[rsl].Cells[5].Value.ToString(),
                B_Depth = dgvProdOfPO.Rows[rsl].Cells[6].Value.ToString(),
                C_Witdh = dgvProdOfPO.Rows[rsl].Cells[7].Value.ToString(),
                D_Web = dgvProdOfPO.Rows[rsl].Cells[8].Value.ToString(),
                E_Flag = dgvProdOfPO.Rows[rsl].Cells[9].Value.ToString(),
                F_Length = dgvProdOfPO.Rows[rsl].Cells[10].Value.ToString(),
                G_Weight = dgvProdOfPO.Rows[rsl].Cells[11].Value.ToString(),
            };

            frmUpdateInfoProdForPO frmUp = new frmUpdateInfoProdForPO(TitleManager.PROD_UPDATE_TITLE, qty, product);
            frmUp.ShowDialog();

            if (frmUp.qty == 0) { return; }

            var prodModify = frmUp.prod;


            //dgvMPRDetail.Rows[rsl].Cells[0].Value = prodModify.Id;
            dgvProdOfPO.Rows[rsl].Cells[5].Value = prodModify.A_Thinhness;
            dgvProdOfPO.Rows[rsl].Cells[6].Value = prodModify.B_Depth;
            dgvProdOfPO.Rows[rsl].Cells[7].Value = prodModify.C_Witdh;
            dgvProdOfPO.Rows[rsl].Cells[8].Value = prodModify.D_Web;
            dgvProdOfPO.Rows[rsl].Cells[9].Value = prodModify.E_Flag;
            dgvProdOfPO.Rows[rsl].Cells[10].Value = prodModify.F_Length;
            dgvProdOfPO.Rows[rsl].Cells[11].Value = prodModify.G_Weight;
            dgvProdOfPO.Rows[rsl].Cells[12].Value = frmUp.qty;
        }

        private void dgvProdOfPO_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.dgvProdOfPO.ClearSelection();
                    this.dgvProdOfPO.Rows[rowSelected].Selected = true;
                }
            }
        }

        private void btnAddAllProdIntoPO_Click(object sender, EventArgs e)
        {
            if (dgvMPRDetail.Rows.Count <= 0) return;
            int rsl = dgvMPRDetail.CurrentRow.Index;
            this.rslOld = dgvMPRs.CurrentRow.Index;
            tlsMPRNo.Text = $"MPR No: [{dgvMPRs.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";

            dtProdsOfAddPO.Clear();
            prodsAdded.Clear();
            dgvProdOfPO.Refresh();

            for (int i = 0; i < dgvMPRDetail.Rows.Count; i++)
            {
                Guid prodId = Guid.Parse(dgvMPRDetail.Rows[i].Cells[0].Value.ToString());
                DataRow dataRow = dtProdsOfAddPO.NewRow();
                dataRow[0] = prodId;
                dataRow[1] = dgvMPRDetail.Rows[i].Cells[3].Value.ToString().Trim();
                dataRow[2] = dgvMPRDetail.Rows[i].Cells[4].Value.ToString().Trim().ToUpper();
                dataRow[3] = "";
                dataRow[4] = dgvMPRDetail.Rows[i].Cells[5].Value.ToString().Trim().ToUpper();
                dataRow[5] = dgvMPRDetail.Rows[i].Cells[6].Value.ToString().Trim();
                dataRow[6] = dgvMPRDetail.Rows[i].Cells[7].Value.ToString().Trim();
                dataRow[7] = dgvMPRDetail.Rows[i].Cells[8].Value.ToString().Trim();
                dataRow[8] = dgvMPRDetail.Rows[i].Cells[9].Value.ToString().Trim();
                dataRow[9] = dgvMPRDetail.Rows[i].Cells[10].Value.ToString().Trim();
                dataRow[10] = dgvMPRDetail.Rows[i].Cells[11].Value.ToString().Trim();
                dataRow[11] = dgvMPRDetail.Rows[i].Cells[12].Value.ToString().Trim();
                dataRow[12] = dgvMPRDetail.Rows[i].Cells[13].Value.ToString().Trim();

                dtProdsOfAddPO.Rows.Add(dataRow);
                prodsAdded.Add(prodId);
            }

            dgvProdOfPO.Rows[0].Selected = true;
        }

        private void dgvMPRs_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvMPRs.CurrentCell == null) return;

            int currentRowIndex = dgvMPRs.CurrentCell.RowIndex;

            // Prevent running on first load
            if (previousRowIndex == -1)
            {
                previousRowIndex = currentRowIndex;
                return;
            }

            bool isAllowed = true;
            if (dgvProdOfPO.Rows.Count > 0)
            {
                if (MessageBoxHelper.Confirm($"You are performing PO creation for MPR No: [{dgvMPRs.Rows[previousRowIndex].Cells[1].Value.ToString().Trim()}].\n" +
                        $"Do you want to cancel the current operation?"))
                {
                    tlsMPRNo.Text = "...";
                    dtProdsOfAddPO.Clear();
                    prodsAdded.Clear();
                    dgvProdOfPO.Refresh();
                }
                else
                {
                    tlsMPRNo.Text = $"MPR No: [{dgvMPRs.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";
                    isAllowed = false;
                    return;
                }
            }

            if (!isAllowed)
            {
                // ❌ Revert to previous row
                this.rslOld = previousRowIndex;
                dgvMPRs.Rows[previousRowIndex].Selected = true;
            }
            else
            {
                // ✅ Save this as the new previous row
                previousRowIndex = currentRowIndex;
            }
        }

        private void btnAddPO_Click(object sender, EventArgs e)
        {
            frmCustomInfoPO frmCustomInfoPO = new frmCustomInfoPO();
            frmCustomInfoPO.ShowDialog();
        }
    }
}
