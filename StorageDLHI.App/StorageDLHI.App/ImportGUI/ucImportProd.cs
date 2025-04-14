using StorageDLHI.App.Common;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
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

namespace StorageDLHI.App.ImportGUI
{
    public partial class ucImportProd : UserControl
    {
        private DataTable dtPos = new DataTable();
        private DataTable dtPoById = new DataTable();
        private DataTable dtProdForImport = new DataTable();
        private List<Guid> prodsAdded = new List<Guid>();


        private bool isSyncingScroll = false;
        private int rslOld;
        private int previousRowIndex = -1;
        private Int32 totalAmount = 0;

        public ucImportProd()
        {
            InitializeComponent();
            LoadData();

            // Create columns DataTable ProdsOfAddPO
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_ID);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_NAME);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_DES_2);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_CODE);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_MATERIAL_CODE);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_A);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_B);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_C);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_D);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_E);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_F);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_PROD_G);
            dtProdForImport.Columns.Add("QTY_PROD_FOR_IMPORT", typeof(Int32));
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_WAREHOUSE_NAME);
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_WAREHOUSE_DETAIL_ID);

            dgvProdForImport.DataSource = dtProdForImport;
            Common.Common.InitializeFooterGrid(dgvProdForImport, dgvFooter);
            Common.Common.InitializeFooterGrid(dgvPODetail, dgvFooterOfPODetail);
        }

        private void LoadData()
        {
            if (!CacheManager.Exists(CacheKeys.POS_DATATABLE_ALL_PO))
            {
                dtPos = PoDAO.GetPOs();
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
                    dtPoById = PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById);
                    dgvPODetail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPODetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
                dgvPOList.Rows[0].Selected = true;
                UpdateFooterOfPoDetail();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, PoDAO.GetPOs());
            LoadData();
        }

        private void dgvPOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPOList.CurrentCell == null) return;
            int currentRowIndex = dgvPOList.CurrentCell.RowIndex;

            if (dtPos != null && dgvPOList.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOList.Rows[currentRowIndex].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)))
                {
                    dtPoById = PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById);
                    dgvPODetail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPODetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
            }
            UpdateFooterOfPoDetail();
        }

        private void btnAddImport_Click(object sender, EventArgs e)
        {

            dgvPOList.Enabled = true;
        }

        private void btnAddAllProdToImport_Click(object sender, EventArgs e)
        {
            if (dgvPODetail.Rows.Count <= 0) return;
            int rsl = dgvPODetail.CurrentRow.Index;
            this.rslOld = dgvPOList.CurrentRow.Index;
            tlsPONo.Text = $"MPR No: [{dgvPOList.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";

            dtProdForImport.Clear();
            prodsAdded.Clear();
            dgvProdForImport.Refresh();

            for (int i = 0; i < dgvPODetail.Rows.Count; i++)
            {
                Guid prodId = Guid.Parse(dgvPODetail.Rows[i].Cells[2].Value.ToString());
                DataRow dataRow = dtProdForImport.NewRow();
                dataRow[0] = prodId;
                dataRow[1] = dgvPODetail.Rows[i].Cells[3].Value.ToString().Trim();
                dataRow[2] = dgvPODetail.Rows[i].Cells[4].Value.ToString().Trim().ToUpper();
                dataRow[3] = "";
                dataRow[4] = dgvPODetail.Rows[i].Cells[5].Value.ToString().Trim().ToUpper();
                dataRow[5] = (dgvPODetail.Rows[i].Cells[6].Value.ToString().Trim());
                dataRow[6] = (dgvPODetail.Rows[i].Cells[7].Value.ToString().Trim());
                dataRow[7] = (dgvPODetail.Rows[i].Cells[8].Value.ToString().Trim());
                dataRow[8] = (dgvPODetail.Rows[i].Cells[9].Value.ToString().Trim());
                dataRow[9] = (dgvPODetail.Rows[i].Cells[10].Value.ToString().Trim());
                dataRow[10] = (dgvPODetail.Rows[i].Cells[11].Value.ToString().Trim());
                dataRow[11] = (dgvPODetail.Rows[i].Cells[12].Value.ToString().Trim()); 
                dataRow[12] = CheckOrReturnNumber(dgvPODetail.Rows[i].Cells[13].Value.ToString().Trim()); // Qty
                dataRow[13] = ""; // Warehouse name
                dataRow[14] = "";

                dtProdForImport.Rows.Add(dataRow);
                prodsAdded.Add(prodId);
                totalAmount += CheckOrReturnNumber(dgvPODetail.Rows[i].Cells[13].Value.ToString().Trim());
            }

            dgvProdForImport.Rows[0].Selected = true;
            dgvPOList.Enabled = false;
            UpdateFooter();
        }

        private Int32 CheckOrReturnNumber(string numberString)
        {
            return !string.IsNullOrEmpty(numberString.Trim())
                && numberString.Trim().Length > 0
                ? Int32.Parse(numberString.Trim()) : 0;
        }

        private void UpdateFooterOfPoDetail()
        {
            Int32 totalQty = 0;
            Int32 totalPrice = 0;
            Int32 totalAmount = 0;

            foreach (DataGridViewRow row in dgvPODetail.Rows)
            {
                if (Int32.TryParse(row.Cells[13].Value?.ToString(), out Int32 qty))
                {
                    totalQty += qty;
                }

                if (Int32.TryParse(row.Cells[14].Value?.ToString(), out Int32 price))
                {
                    totalPrice += price;
                }

                if (Int32.TryParse(row.Cells[15].Value?.ToString(), out Int32 amount))
                {
                    totalAmount += amount;
                }
            }

            dgvFooterOfPODetail.Rows[0].Cells[3].Value = "TOTAL";
            dgvFooterOfPODetail.Rows[0].Cells[13].Value = totalQty;
            dgvFooterOfPODetail.Rows[0].Cells[14].Value = totalPrice.ToString("N0");
            dgvFooterOfPODetail.Rows[0].Cells[15].Value = totalAmount.ToString("N0");

            dgvFooterOfPODetail.Rows[0].Cells[3].Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Common.Common.StyleFooterCell(dgvFooterOfPODetail.Rows[0].Cells[13]);
            Common.Common.StyleFooterCell(dgvFooterOfPODetail.Rows[0].Cells[14]);
            Common.Common.StyleFooterCell(dgvFooterOfPODetail.Rows[0].Cells[15]);
        }

        private void UpdateFooter()
        {
            Int32 totalQty = 0;

            foreach (DataGridViewRow row in dgvProdForImport.Rows)
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

        private void dgvPOList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvPODetail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvProdForImport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvPODetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPODetail.Columns["PO_DETAIL_QTY"].DefaultCellStyle.Format = "N0";
            dgvPODetail.Columns["PO_DETAIL_PRICE"].DefaultCellStyle.Format = "N0";
            dgvPODetail.Columns["PO_DETAIL_AMOUNT"].DefaultCellStyle.Format = "N0";
        }

        private void dgvProdForImport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProdForImport.Columns["QTY_PROD_FOR_IMPORT"].DefaultCellStyle.Format = "N0";
        }

        private void dgvProdForImport_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Apply the changed width to the corresponding column in the footer
            if (e.Column.Index < dgvFooter.Columns.Count)
            {
                dgvFooter.Columns[e.Column.Index].Width = e.Column.Width;
            }

            // Resize for DataGridViewMain and DataGridViewFooter the same
            Common.Common.AdjustFooterScrollbar(dgvProdForImport, dgvFooter);
        }

        private void dgvProdForImport_Scroll(object sender, ScrollEventArgs e)
        {
            dgvFooter.HorizontalScrollingOffset = dgvProdForImport.HorizontalScrollingOffset;
        }

        private void dgvPODetail_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Apply the changed width to the corresponding column in the footer
            if (e.Column.Index < dgvFooterOfPODetail.Columns.Count)
            {
                dgvFooterOfPODetail.Columns[e.Column.Index].Width = e.Column.Width;
            }

            // Resize for DataGridViewMain and DataGridViewFooter the same
            Common.Common.AdjustFooterScrollbar(dgvPODetail, dgvFooterOfPODetail);
        }

        private void dgvPODetail_Scroll(object sender, ScrollEventArgs e)
        {
            //dgvFooterOfPODetail.HorizontalScrollingOffset = dgvPODetail.HorizontalScrollingOffset;
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && !isSyncingScroll)
            {
                isSyncingScroll = true;
                dgvFooterOfPODetail.HorizontalScrollingOffset = dgvPODetail.HorizontalScrollingOffset;
                isSyncingScroll = false;
            }
        }

        private void dgvFooterOfPODetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && !isSyncingScroll)
            {
                isSyncingScroll = true;
                dgvPODetail.HorizontalScrollingOffset = dgvFooterOfPODetail.HorizontalScrollingOffset;
                isSyncingScroll = false;
            }
        }


        private void dgvPOList_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvPOList.CurrentCell == null) return;

            int currentRowIndex = dgvPOList.CurrentCell.RowIndex;

            // Prevent running on first load
            if (previousRowIndex == -1)
            {
                previousRowIndex = currentRowIndex;
                return;
            }

            bool isAllowed = true;
            if (dgvProdForImport.Rows.Count > 0)
            {
                if (MessageBoxHelper.Confirm($"You are performing PO creation for MPR No: [{dgvPOList.Rows[previousRowIndex].Cells[1].Value.ToString().Trim()}].\n" +
                        $"Do you want to cancel the current operation?"))
                {
                    tlsPONo.Text = "...";
                    dtProdForImport.Clear();
                    prodsAdded.Clear();
                    dgvProdForImport.Refresh();
                    totalAmount = 0;
                }
                else
                {
                    tlsPONo.Text = $"MPR No: [{dgvPOList.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";
                    isAllowed = false;
                    return;
                }
            }

            if (!isAllowed)
            {
                // ❌ Revert to previous row
                this.rslOld = previousRowIndex;
                dgvPOList.Rows[previousRowIndex].Selected = true;
            }
            else
            {
                // ✅ Save this as the new previous row
                previousRowIndex = currentRowIndex;
            }
        }

        private void btnClearProdsOfImport_Click(object sender, EventArgs e)
        {

            dgvPOList.Enabled = true;
        }
    }
}
