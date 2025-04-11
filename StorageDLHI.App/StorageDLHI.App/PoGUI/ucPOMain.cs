using StorageDLHI.App.Common;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
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
using System.Security.Cryptography;
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
        private Int32 totalAmount = 0;


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
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_A, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_B, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_C, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_D, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_E, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_F, typeof(Int32));
            dtProdsOfAddPO.Columns.Add(QueryStatement.PROPERTY_PROD_G, typeof(Int32));
            dtProdsOfAddPO.Columns.Add("QTY", typeof(Int32));
            dtProdsOfAddPO.Columns.Add("PO_PRICE", typeof(Int32));
            dtProdsOfAddPO.Columns.Add("PO_AMOUNT", typeof(Int32));
            dtProdsOfAddPO.Columns.Add("PO_RECEVIE");
            dtProdsOfAddPO.Columns.Add("PO_REMARKS");

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

            frmAddPriceForProdPO frmAddPriceForProdPO = new frmAddPriceForProdPO();
            frmAddPriceForProdPO.ShowDialog();
            
            if (frmAddPriceForProdPO.Price == 0) { return; }

            tlsMPRNo.Text = $"MPR No: [{dgvMPRs.Rows[this.previousRowIndex].Cells[1].Value.ToString().Trim()}]\t";

            Guid prodId = Guid.Parse(dgvMPRDetail.Rows[rsl].Cells[2].Value.ToString());

            if (prodsAdded.Contains(prodId))
            {
                MessageBoxHelper.ShowWarning($"You added product [{dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString()}] into PO. ");
                return;
            }

            DataRow dataRow = dtProdsOfAddPO.NewRow();
            dataRow[0] = prodId;
            dataRow[1] = dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString().Trim();
            dataRow[2] = dgvMPRDetail.Rows[rsl].Cells[4].Value.ToString().Trim().ToUpper();
            dataRow[3] = "";
            dataRow[4] = dgvMPRDetail.Rows[rsl].Cells[5].Value.ToString().Trim().ToUpper();
            dataRow[5] = (dgvMPRDetail.Rows[rsl].Cells[6].Value.ToString().Trim());
            dataRow[6] = (dgvMPRDetail.Rows[rsl].Cells[7].Value.ToString().Trim());
            dataRow[7] = (dgvMPRDetail.Rows[rsl].Cells[8].Value.ToString().Trim());
            dataRow[8] = (dgvMPRDetail.Rows[rsl].Cells[9].Value.ToString().Trim());
            dataRow[9] = (dgvMPRDetail.Rows[rsl].Cells[10].Value.ToString().Trim());
            dataRow[10] = (dgvMPRDetail.Rows[rsl].Cells[11].Value.ToString().Trim());
            dataRow[11] = (dgvMPRDetail.Rows[rsl].Cells[12].Value.ToString().Trim());
            dataRow[12] = CheckOrReturnNumber(dgvMPRDetail.Rows[rsl].Cells[13].Value.ToString().Trim()); // Qty
            dataRow[13] = CheckOrReturnNumber(frmAddPriceForProdPO.Price.ToString()); // Price
            dataRow[14] = CheckOrReturnNumber(dgvMPRDetail.Rows[rsl].Cells[13].Value.ToString().Trim()) * frmAddPriceForProdPO.Price; // Amount
            dataRow[15] = frmAddPriceForProdPO.Recevie;
            dataRow[16] = frmAddPriceForProdPO.Remark;

            totalAmount += Int32.Parse(dgvMPRDetail.Rows[rsl].Cells[13].Value.ToString().Trim()) * frmAddPriceForProdPO.Price; // Amount
            dtProdsOfAddPO.Rows.Add(dataRow);
            prodsAdded.Add(prodId);
            dgvProdOfPO.Rows[0].Selected = true;
        }

        private Int32 CheckOrReturnNumber(string numberString)
        {
            return !string.IsNullOrEmpty(numberString.Trim())
                && numberString.Trim().Length > 0
                ? Int32.Parse(numberString.Trim()) : 0;
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

            int qty = CheckOrReturnNumber(dgvProdOfPO.Rows[rsl].Cells[12].Value.ToString());

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

            CustomProdOfPO customProdOfPO = new CustomProdOfPO()
            {
                Qty = qty > 0 ? qty : 1,
                Price = CheckOrReturnNumber(dgvProdOfPO.Rows[rsl].Cells[13].Value.ToString()) == 0 
                    ? 1 : CheckOrReturnNumber(dgvProdOfPO.Rows[rsl].Cells[13].Value.ToString()),
                Recevie = dgvProdOfPO.Rows[rsl].Cells[15].Value.ToString().Trim(),
                Remark = dgvProdOfPO.Rows[rsl].Cells[16].Value.ToString().Trim()
            };

            frmUpdateInfoProdForPO frmUp = new frmUpdateInfoProdForPO(TitleManager.PROD_UPDATE_TITLE, customProdOfPO, product);
            frmUp.ShowDialog();

            if (frmUp.prodOfPO.Qty == 0) { return; }
            totalAmount -= qty;

            var prodModify = frmUp.prod;

            //dgvMPRDetail.Rows[rsl].Cells[0].Value = prodModify.Id;
            dgvProdOfPO.Rows[rsl].Cells[5].Value = prodModify.A_Thinhness;
            dgvProdOfPO.Rows[rsl].Cells[6].Value = prodModify.B_Depth;
            dgvProdOfPO.Rows[rsl].Cells[7].Value = prodModify.C_Witdh;
            dgvProdOfPO.Rows[rsl].Cells[8].Value = prodModify.D_Web;
            dgvProdOfPO.Rows[rsl].Cells[9].Value = prodModify.E_Flag;
            dgvProdOfPO.Rows[rsl].Cells[10].Value = prodModify.F_Length;
            dgvProdOfPO.Rows[rsl].Cells[11].Value = prodModify.G_Weight;
            dgvProdOfPO.Rows[rsl].Cells[12].Value = frmUp.prodOfPO.Qty;
            dgvProdOfPO.Rows[rsl].Cells[13].Value = frmUp.prodOfPO.Price;
            dgvProdOfPO.Rows[rsl].Cells[14].Value = frmUp.prodOfPO.Price * frmUp.prodOfPO.Qty; // amount
            dgvProdOfPO.Rows[rsl].Cells[15].Value = frmUp.prodOfPO.Recevie;
            dgvProdOfPO.Rows[rsl].Cells[16].Value = frmUp.prodOfPO.Remark;
            totalAmount += frmUp.prodOfPO.Qty;
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
                Guid prodId = Guid.Parse(dgvMPRDetail.Rows[i].Cells[2].Value.ToString());
                DataRow dataRow = dtProdsOfAddPO.NewRow();
                dataRow[0] = prodId;
                dataRow[1] = dgvMPRDetail.Rows[i].Cells[3].Value.ToString().Trim();
                dataRow[2] = dgvMPRDetail.Rows[i].Cells[4].Value.ToString().Trim().ToUpper();
                dataRow[3] = "";
                dataRow[4] = dgvMPRDetail.Rows[i].Cells[5].Value.ToString().Trim().ToUpper();
                dataRow[5] = (dgvMPRDetail.Rows[i].Cells[6].Value.ToString().Trim());
                dataRow[6] = (dgvMPRDetail.Rows[i].Cells[7].Value.ToString().Trim());
                dataRow[7] = (dgvMPRDetail.Rows[i].Cells[8].Value.ToString().Trim());
                dataRow[8] = (dgvMPRDetail.Rows[i].Cells[9].Value.ToString().Trim());
                dataRow[9] = (dgvMPRDetail.Rows[i].Cells[10].Value.ToString().Trim());
                dataRow[10] = (dgvMPRDetail.Rows[i].Cells[11].Value.ToString().Trim());
                dataRow[11] = (dgvMPRDetail.Rows[i].Cells[12].Value.ToString().Trim());
                dataRow[12] = CheckOrReturnNumber(dgvMPRDetail.Rows[i].Cells[13].Value.ToString().Trim());

                dtProdsOfAddPO.Rows.Add(dataRow);
                prodsAdded.Add(prodId);
                totalAmount += CheckOrReturnNumber(dgvMPRDetail.Rows[i].Cells[13].Value.ToString().Trim());
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
                    totalAmount = 0;
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
            if (dgvProdOfPO.Rows.Count <= 0) 
            {
                MessageBoxHelper.ShowWarning("Please add product of MPR to create PO!");
                return;
            }

            foreach (DataRow dataRow in dtProdsOfAddPO.Rows)
            {
                if (string.IsNullOrEmpty(dataRow[13].ToString().Trim()))
                {
                    MessageBoxHelper.ShowWarning($"Please enter value [Price] for product [{dataRow[1].ToString().Trim()}] before create PO !");
                    return;
                }
            }

            int rsl = dgvProdOfPO.CurrentRow.Index;

            Pos mPO = new Pos()
            {
                Po_Mpr_No = dgvMPRs.Rows[this.previousRowIndex].Cells[1].Value.ToString().Trim(),
                Po_Wo_No = dgvMPRs.Rows[this.previousRowIndex].Cells[2].Value.ToString().Trim(),
                Po_Project_Name = dgvMPRs.Rows[this.previousRowIndex].Cells[3].Value.ToString().Trim(),
            };

            frmCustomInfoPO frmCustomInfoPO = new frmCustomInfoPO(TitleManager.PO_ADD, true, mPO, dtProdsOfAddPO, totalAmount);
            frmCustomInfoPO.ShowDialog();

            if (!frmCustomInfoPO.completed)
            {
                return;
            }

            tlsMPRNo.Text = "...";
            dtProdsOfAddPO.Clear();
            prodsAdded.Clear();
            dgvProdOfPO.Refresh();
            totalAmount = 0;
        }

        private void dgvProdOfPO_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProdOfPO.Columns["A_THINHNESS"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["B_DEPTH"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["C_WIDTH"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["D_WEB"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["E_FLAG"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["F_LENGTH"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["G_WEIGHT"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["QTY"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["PO_PRICE"].DefaultCellStyle.Format = "N0";
            dgvProdOfPO.Columns["PO_AMOUNT"].DefaultCellStyle.Format = "N0";

            //Common.Common.HideZeroValueColumns(dgvProdOfPO);
        }

        private void dgvMPRDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvMPRDetail.Columns["A_THINHNESS_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["B_DEPTH_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["C_WIDTH_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["D_WEB_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["E_FLAG_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["F_LENGTH_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["G_WEIGHT_M"].DefaultCellStyle.Format = "N0";
            dgvMPRDetail.Columns["MPR_QTY_M"].DefaultCellStyle.Format = "N0";
        }

        private void InitializeFooterGrid()
        {
            // Clone columns from main grid
            //foreach (DataGridViewColumn col in dgvProdOfPO.Columns)
            //{
            //    var footerCol = (DataGridViewColumn)col.Clone();
            //    footerCol.Width = col.Width;
            //    dataGridViewFooter.Columns.Add(footerCol);
            //}

            //// Add a single row
            //dataGridViewFooter.Rows.Add();

            //// Styling
            //dataGridViewFooter.ReadOnly = true;
            //dataGridViewFooter.AllowUserToAddRows = false;
            //dataGridViewFooter.AllowUserToDeleteRows = false;
            //dataGridViewFooter.AllowUserToResizeRows = false;
            //dataGridViewFooter.AllowUserToResizeColumns = false;
            //dataGridViewFooter.RowHeadersVisible = false;
            //dataGridViewFooter.ColumnHeadersVisible = false;
            //dataGridViewFooter.ScrollBars = ScrollBars.None;
            //dataGridViewFooter.DefaultCellStyle.BackColor = Color.LightYellow;
        }
    }
}
