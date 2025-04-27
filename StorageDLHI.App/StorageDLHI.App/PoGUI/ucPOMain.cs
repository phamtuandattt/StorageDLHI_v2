using log4net.Appender;
using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.ImportDAO;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;

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
        private DataTable dtPos = new DataTable();
        private DataTable dtPoById = new DataTable();
        private DataTable dtMprDetailByIdTemporary = new DataTable();

        private Panel pnNoDataMprs = new Panel();
        private Panel pnNoDataMprsDetail = new Panel();
        private Panel pnNoDataPOs = new Panel();
        private Panel pnNoDataPODetail = new Panel();

        private int rslOld;
        private int previousRowIndex = -1;
        private Int32 totalAmount = 0;



        public ucPOMain()
        {
            InitializeComponent();

            ucPanelNoData ucNoDataMPRs = new ucPanelNoData("All MPR have been Make PO");
            pnNoDataMprs = ucNoDataMPRs.pnlNoData;
            dgvMPRs.Controls.Add(pnNoDataMprs);

            ucPanelNoData ucNoDataMPRDetail = new ucPanelNoData("No records found");
            pnNoDataMprsDetail = ucNoDataMPRDetail.pnlNoData;
            dgvMPRDetail.Controls.Add(pnNoDataMprsDetail);

            ucPanelNoData ucNoDataPOs = new ucPanelNoData("No records found");
            pnNoDataPOs = ucNoDataPOs.pnlNoData;
            dgvPOList.Controls.Add(pnNoDataPOs);

            ucPanelNoData ucNoDataPODetail = new ucPanelNoData("No records found");
            pnNoDataPODetail = ucNoDataPODetail.pnlNoData;
            dgvPODetail.Controls.Add(pnNoDataPODetail);

            LoadData();

            //// Modify columns name DataTable dtMprDetailTemporary
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_A].ColumnName = "A_THINH_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_B].ColumnName = "B_DEPTH_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_C].ColumnName = "C_WIDTH_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_D].ColumnName = "D_WEB_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_E].ColumnName = "E_FLAG_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_F].ColumnName = "F_LENGTH_M";
            //dtMprDetailByIdTemporary.Columns[QueryStatement.PROPERTY_PROD_G].ColumnName = "G_WEIGHT_M";
            //dtMprDetailByIdTemporary.Columns["MPR_QTY"].ColumnName = "MPR_QTY_M";

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

            
            //dtMprDetailByIdTemporary.Clear();

            Common.Common.InitializeFooterGrid(dgvProdOfPO, dgvFooter); 

            if (dgvMPRs.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataMprs);
                Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
        }

        private async void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.POS_DATATABLE_ALL_PO))
            {
                dtPos = await PoDAO.GetPOs();
                CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, dtPos);
                dgvPOList.DataSource = dtPos;
            }
            else
            {
                dtPos = CacheManager.Get<DataTable>(CacheKeys.POS_DATATABLE_ALL_PO);
                dgvPOList.DataSource = CacheManager.Get<DataTable>(CacheKeys.POS_DATATABLE_ALL_PO);
            }

            if (dtPos != null && dgvPOList.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOList.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)))
                {
                    dtPoById = await PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById);
                    dgvPODetail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPODetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
            }


            if (!CacheManager.Exists(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS))
            {
                dtMprs = await ShowDialogManager.WithLoader(() => MprDAO.GetMprsForMakePO());
                CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, dtMprs);
                dgvMPRs.DataSource = dtMprs;
            }
            else
            {
                dtMprs = await ShowDialogManager.WithLoader(() => MprDAO.GetMprsForMakePO());
                dgvMPRs.DataSource = CacheManager.Get<DataTable>(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS);
            }

            if (dtMprs.Rows.Count > 0 && dtMprs != null && dgvMPRs.Rows.Count > 0)
            {
                var mprId = Guid.Parse(dgvMPRs.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId)))
                {
                    dtMprDetailById = await MprDAO.GetMprDetailByMpr(mprId);
                    CacheManager.Add(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId), dtMprDetailById);
                    dgvMPRDetail.DataSource = dtMprDetailById;
                }
                else
                {
                    dtMprDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
                    dgvMPRDetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
                }

                dtMprDetailByIdTemporary = dtMprDetailById.Copy();
                dtMprDetailByIdTemporary.Clear();

                Common.Common.HideNoDataPanel(pnNoDataMprs);
                Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, await MprDAO.GetMprsForMakePO());
            LoadData();
            if (dgvMPRs.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataMprs);
                Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
        }

        private void btnClearProdsOfPO_Click(object sender, EventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0) return;
            int rsl = dgvProdOfPO.CurrentRow.Index;

            if (!MessageBoxHelper.Confirm($"Do you want delete all product of PO?"))
            {
                return;
            }

            // Add again prod into dgvMprDetail
            foreach (DataRow item in dtMprDetailByIdTemporary.Rows)
            {
                dtMprDetailById.ImportRow(item);
            }

            tlsMPRNo.Text = "...";
            dtProdsOfAddPO.Clear();
            prodsAdded.Clear();
            dgvProdOfPO.Refresh();
            dgvMPRs.Enabled = true;

            var mprId = Guid.Parse(dgvMPRs.Rows[0].Cells[0].Value.ToString());
            dtMprDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
            //dgvMPRDetail.DataSource = dtMprDetailById;
            //dgvMPRDetail.Refresh();

            UpdateFooter();
        }

        private async void dgvMPRs_CellClick(object sender, DataGridViewCellEventArgs e)
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
            if (!CacheManager.Exists(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId)))
            {
                dtMprDetailById = await MprDAO.GetMprDetailByMpr(mprId);
                CacheManager.Add(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId), dtMprDetailById);
                dgvMPRDetail.DataSource = dtMprDetailById;
            }
            else
            {
                dgvMPRDetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
            }
            UpdateFooter();
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

            //// Add current row in dtMPRDetailTemporary
            //foreach (DataGridViewCell cell in dgvMPRDetail.CurrentRow.Cells)
            //{
            //    if (dtMprDetailByIdTemporary.Columns.Contains(cell.OwningColumn.Name))
            //    {
            //        rowCopy[cell.OwningColumn.Name] = cell.Value ?? DBNull.Value;
            //    }
            //}
            //dtMprDetailByIdTemporary.Rows.Add(rowCopy);

            DataRow rowCopy = dtMprDetailByIdTemporary.NewRow();
            for (int i = 0; i < dgvMPRDetail.ColumnCount; i++)
            {
                rowCopy[i] = dgvMPRDetail.CurrentRow.Cells[i].Value;
            }
            dtMprDetailByIdTemporary.Rows.Add(rowCopy);


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
            UpdateFooter();
            dgvMPRs.Enabled = false;

            // Get Cache before delete row in dgvMPRDetail
            var mprId = Guid.Parse(dgvMPRs.CurrentRow.Cells[0].Value.ToString());
            var dtMprDetailCoppy = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId)).Copy();
            dgvMPRDetail.Rows.RemoveAt(rsl);
            // Set data for cache after delete 
            CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, mprId), dtMprDetailCoppy);
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
            totalAmount += frmUp.prodOfPO.Price * frmUp.prodOfPO.Qty;
            UpdateFooter();
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

            // Get Cache 
            var mprId = Guid.Parse(dgvMPRs.CurrentRow.Cells[0].Value.ToString());
            var dtMprDetailCoppy = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId)).Copy();


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

                DataRow rowCopy = dtMprDetailByIdTemporary.NewRow();
                for (int j = 0; j < dgvMPRDetail.ColumnCount; j++)
                {
                    rowCopy[j] = dgvMPRDetail.Rows[i].Cells[j].Value;
                }
                dtMprDetailByIdTemporary.Rows.Add(rowCopy);

                dgvMPRDetail.Rows.RemoveAt(i);
            }

            // Set data for cache after delete 
            
            CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, mprId), dtMprDetailCoppy);

            dgvProdOfPO.Rows[0].Selected = true;
            dgvMPRs.Enabled = false;
            UpdateFooter();
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

        private async void btnAddPO_Click(object sender, EventArgs e)
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

            Guid mprID = Guid.Parse(dgvMPRs.Rows[this.previousRowIndex].Cells[0].Value.ToString().Trim());

            frmCustomInfoPO frmCustomInfoPO = new frmCustomInfoPO(TitleManager.PO_ADD, true, mPO, dtProdsOfAddPO, totalAmount, mprID);
            frmCustomInfoPO.ShowDialog();

            if (!frmCustomInfoPO.completed && !frmCustomInfoPO.isHandle)
            {
                return;
            }

            tlsMPRNo.Text = "...";
            dtProdsOfAddPO.Clear();
            prodsAdded.Clear();
            dgvProdOfPO.Refresh();
            totalAmount = 0;
            UpdateFooter();
            dgvMPRs.Enabled = true;
            // Update data in cache
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, await PoDAO.GetPOs());
            CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, await MprDAO.GetMprsForMakePO());
            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, await ImportProductDAO.GetImportProducts());

            LoadData();
        }

        private void UpdateFooter()
        {
            Int32 totalQty = 0;
            Int32 totalPrice = 0;
            Int32 totalAmount = 0;

            foreach (DataGridViewRow row in dgvProdOfPO.Rows)
            {
                if (Int32.TryParse(row.Cells[12].Value?.ToString(), out Int32 qty))
                {
                    totalQty += qty;
                }

                if (Int32.TryParse(row.Cells[13].Value?.ToString(), out Int32 price))
                {
                    totalPrice += price;
                }

                if (Int32.TryParse(row.Cells[14].Value?.ToString(), out Int32 amount))
                {
                    totalAmount += amount;
                }
            }

            dgvFooter.Rows[0].Cells[1].Value = "TOTAL";
            dgvFooter.Rows[0].Cells[12].Value = totalQty;
            dgvFooter.Rows[0].Cells[13].Value = totalPrice.ToString("N0");
            dgvFooter.Rows[0].Cells[14].Value = totalAmount.ToString("N0");

            dgvFooter.Rows[0].Cells[1].Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Common.Common.StyleFooterCell(dgvFooter.Rows[0].Cells[12]);
            Common.Common.StyleFooterCell(dgvFooter.Rows[0].Cells[13]);
            Common.Common.StyleFooterCell(dgvFooter.Rows[0].Cells[14]);
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

        private void dgvProdOfPO_Scroll(object sender, ScrollEventArgs e)
        {
            dgvFooter.HorizontalScrollingOffset = dgvProdOfPO.HorizontalScrollingOffset;
        }

        private void dgvProdOfPO_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Apply the changed width to the corresponding column in the footer
            if (e.Column.Index < dgvFooter.Columns.Count)
            {
                dgvFooter.Columns[e.Column.Index].Width = e.Column.Width;
            }

            // Resize for DataGridViewMain and DataGridViewFooter the same
            Common.Common.AdjustFooterScrollbar(dgvProdOfPO, dgvFooter);
        }

        private void dgvPOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPOList.Columns["PO_TOTAL_AMOUNT"].DefaultCellStyle.Format = "N0";
        }

        private void dgvPODetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPODetail.Columns["PO_DETAIL_QTY"].DefaultCellStyle.Format = "N0";
            dgvPODetail.Columns["PO_DETAIL_PRICE"].DefaultCellStyle.Format = "N0";
            dgvPODetail.Columns["PO_DETAIL_AMOUNT"].DefaultCellStyle.Format = "N0";
        }

        private void dgvPOList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvPODetail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private async void dgvPOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPOList.CurrentCell == null) return;
            int currentRowIndex = dgvPOList.CurrentCell.RowIndex;

            if (dtPos != null && dgvPOList.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOList.Rows[currentRowIndex].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)))
                {
                    dtPoById = await PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById);
                    dgvPODetail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPODetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
            }
        }

        private void tlsReloadPOs_Click(object sender, EventArgs e)
        {
            CacheManager.Remove(CacheKeys.POS_DATATABLE_ALL_PO);
            lblDateTimeSeacrh.Text = "";
            LoadData();
            if (dgvPOList.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOList, pnNoDataPOs);
                Common.Common.ShowNoDataPanel(dgvPODetail, pnNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataPOs);
                Common.Common.HideNoDataPanel(pnNoDataPODetail);
            }
        }

        private void txtSearchPO_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchPO.Text))
            {
                dgvPOList.Refresh();
            }
            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_PO_NO,
                QueryStatement.PROPERTY_PO_MPR_NO,
                QueryStatement.PROPERTY_PO_WO_NO,
                QueryStatement.PROPERTY_PO_PROJECT_NAME,
                QueryStatement.PROPERTY_PO_PREPARED,
                QueryStatement.PROPERTY_PO_REVIEWED,
                QueryStatement.PROPERTY_PO_AGREEMENT,
                QueryStatement.PROPERTY_PO_APPROVED,
                QueryStatement.PROPERTY_PO_PAYMENT_TERM,
                QueryStatement.PROPERTY_PO_DISPATCH_BOX,
            };

            dgvPOList.DataSource = Common.Common.Search(txtSearchPO.Text.Trim(), dtPos, lstProperty);

            if (dgvPOList.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOList, pnNoDataPOs);
                Common.Common.ShowNoDataPanel(dgvPODetail, pnNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataPOs);
                Common.Common.HideNoDataPanel(pnNoDataPODetail);
            }
        }

        private void tlsSearchDate_Click(object sender, EventArgs e)
        {
            if (dgvPOList.Rows.Count <= 0)
            {
                return;
            }
            frmSeacrhPOFromDate frmSeacrhPOFromDate = new frmSeacrhPOFromDate();
            frmSeacrhPOFromDate.ShowDialog();

            if (!frmSeacrhPOFromDate.IsSearch)
            {
                dgvPOList.Refresh();
                return;
            }

            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_PO_PO_CREATE_DATE,
                QueryStatement.PROPERTY_PO_PO_EXPECTED_DELIVERY_DATE
            };

            DateTime fDate = frmSeacrhPOFromDate.FromDate;
            DateTime tDate = frmSeacrhPOFromDate.ToDate;

            lblDateTimeSeacrh.Text = $"From: {fDate.ToString("dd/MM/yyyy")} To: {tDate.ToString("dd/MM/yyyy")}";
            dgvPOList.DataSource = Common.Common.SearchDate(fDate, tDate, dtPos, lstProperty);
            tlsClearSearchDate.Visible = true;

            if (dgvPOList.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOList, pnNoDataPOs);
                Common.Common.ShowNoDataPanel(dgvPODetail, pnNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataPOs);
                Common.Common.HideNoDataPanel(pnNoDataPODetail);
            }
        }

        private void tlsClearSearchDate_Click(object sender, EventArgs e)
        {
            lblDateTimeSeacrh.Text = "";
            dgvPOList.Refresh();
            dgvPOList.DataSource = CacheManager.Get<DataTable>(CacheKeys.POS_DATATABLE_ALL_PO).Copy();

            if (dgvPOList.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOList, pnNoDataPOs);
                Common.Common.ShowNoDataPanel(dgvPODetail, pnNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataPOs);
                Common.Common.HideNoDataPanel(pnNoDataPODetail);
            }
            tlsClearSearchDate.Visible = false;
        }

        private void tlsExportPO_Click(object sender, EventArgs e)
        {
            if (dgvPOList.Rows.Count <= 0) { return; }
            int rsl = dgvPOList.CurrentRow.Index;

            Pos mPO = new Pos()
            {
                Id = Guid.Parse(dgvPOList.Rows[rsl].Cells[0].Value.ToString().Trim()),
                Po_No = dgvPOList.Rows[rsl].Cells[1].Value.ToString().Trim(),
                Po_Mpr_No = dgvPOList.Rows[rsl].Cells[2].Value.ToString().Trim(),
                Po_Wo_No = dgvPOList.Rows[rsl].Cells[3].Value.ToString().Trim(),
                Po_Project_Name = dgvPOList.Rows[rsl].Cells[4].Value.ToString().Trim(),

                Po_CreateDate = DateTime.Parse(dgvPOList.Rows[rsl].Cells[6].Value.ToString().Trim()),
                Po_Expected_Delivery_Date = DateTime.Parse(dgvPOList.Rows[rsl].Cells[7].Value.ToString().Trim()),
                Po_Prepared = dgvPOList.Rows[rsl].Cells[8].Value.ToString().Trim(),
                Po_Reviewed = dgvPOList.Rows[rsl].Cells[9].Value.ToString().Trim(),
                Po_Agrement = dgvPOList.Rows[rsl].Cells[10].Value.ToString().Trim(),
                Po_Approved = dgvPOList.Rows[rsl].Cells[11].Value.ToString().Trim(),
                Po_Payment_Term = dgvPOList.Rows[rsl].Cells[12].Value.ToString().Trim(),

                SupplierId = Guid.Parse(dgvPOList.Rows[rsl].Cells[18].Value.ToString().Trim())
            };

            DataTable dtForExport = new DataTable();
            dtForExport.Columns.Add("PROD_NAME"); // 0
            dtForExport.Columns.Add("MATERIAL"); // 1
            dtForExport.Columns.Add("A"); // 2
            dtForExport.Columns.Add("B");
            dtForExport.Columns.Add("C");
            dtForExport.Columns.Add("QTY"); // 5
            dtForExport.Columns.Add("UNIT"); // 6
            dtForExport.Columns.Add("G"); // 7
            dtForExport.Columns.Add("MPR_NO_REV");
            dtForExport.Columns.Add("REQ_DATE");
            dtForExport.Columns.Add("PLACE_OF_ENTRY");
            dtForExport.Columns.Add("PRICE"); // 11
            dtForExport.Columns.Add("AMOUNT"); // 12
            dtForExport.Columns.Add("RECEIVE");
            dtForExport.Columns.Add("REMARKS");

            foreach (DataGridViewRow item in dgvPODetail.Rows)
            {
                DataRow dataRow = dtForExport.NewRow();
                dataRow[0] = item.Cells[3].Value.ToString().Trim();
                dataRow[1] = item.Cells[5].Value.ToString().Trim();
                dataRow[2] = item.Cells[6].Value.ToString().Trim();
                dataRow[3] = item.Cells[7].Value.ToString().Trim();
                dataRow[4] = item.Cells[8].Value.ToString().Trim();
                dataRow[5] = item.Cells[13].Value.ToString().Trim(); //
                dataRow[6] = item.Cells[16].Value.ToString().Trim();
                dataRow[7] = item.Cells[12].Value.ToString().Trim();
                dataRow[8] = "";
                dataRow[9] = "";
                dataRow[10] = "";
                dataRow[11] = item.Cells[14].Value.ToString().Trim();
                dataRow[12] = item.Cells[15].Value.ToString().Trim();
                dataRow[13] = "";
                dataRow[14] = "";

                dtForExport.Rows.Add(dataRow);
            }
            
            frmCustomPrintPO print = new frmCustomPrintPO(mPO, dtForExport);
            print.ShowDialog();

            if (!print.IsPrinted)
            {
                return;
            }
        }

        private void removeProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0) { return; }
            int rsl = dgvProdOfPO.CurrentRow.Index;
            Guid prodId = new Guid(dgvProdOfPO.Rows[rsl].Cells[0].Value.ToString()); // ProdId

            // Add again prod into dgvMprDetail
            foreach (DataRow item in dtMprDetailByIdTemporary.Rows)
            {
                if (item[2].ToString().Equals(prodId.ToString()))
                {
                    dtMprDetailById.ImportRow(item);
                    break;
                }
            }

            // Remove row in dgvProOfPO
            dtProdsOfAddPO.Rows.RemoveAt(rsl);

            if (dtProdsOfAddPO.Rows.Count == 0)
            {
                tlsMPRNo.Text = "...";
                dtProdsOfAddPO.Clear();
                prodsAdded.Clear();
                dgvProdOfPO.Refresh();
                dgvMPRs.Enabled = true;
                
                var mprId = Guid.Parse(dgvMPRs.Rows[0].Cells[0].Value.ToString());
                dtMprDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
                UpdateFooter();
            }

            prodsAdded.Remove(prodId);
            UpdateFooter();
        }

        private void tlsSearchMprForMakePO_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(tlsSearchMprForMakePO.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (tlsSearchMprForMakePO.Text != cleaned)
            {
                int pos = tlsSearchMprForMakePO.SelectionStart - 1;
                tlsSearchMprForMakePO.Text = cleaned;
                tlsSearchMprForMakePO.SelectionStart = Math.Max(pos, 0);
                return;
            }

            if (string.IsNullOrEmpty(tlsSearchMprForMakePO.Text))
            {
                dgvMPRs.Refresh();
            }
            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_MPR_MPR_NO,
                QueryStatement.PROPERTY_MPR_MPR_WO_NO,
                QueryStatement.PROPERTY_MPR_MPR_PROJECT_NAME
            };

            dgvMPRs.DataSource = Common.Common.Search(tlsSearchMprForMakePO.Text.Trim(), dtMprs.Copy(), lstProperty);

            if (dgvMPRs.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnNoDataMprs);
                Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
        }
    }
}
