using StorageDLHI.App.Common;
using StorageDLHI.App.PoGUI;
using StorageDLHI.BLL.ImportDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        private List<string> prodsAdded = new List<string>();
        private DataTable dtProdForImportForUpdateDB = new DataTable();
        private DataTable dtImportProducts = new DataTable();
        private DataTable dtImportProductDetailById = new DataTable();

        private bool isSyncingScroll = false;
        private int rslOld;
        private int previousRowIndex = -1;
        private Int32 totalAmount = 0;

        public ucImportProd()
        {
            InitializeComponent();
            LoadData();

            // Create columns DataTable ProdForImport
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
            dtProdForImport.Columns.Add("QTY_PROD_FOR_IMPORT", typeof(Int32)); // 12
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_WAREHOUSE_NAME); // 13
            dtProdForImport.Columns.Add(QueryStatement.PROPERTY_WAREHOUSE_DETAIL_ID); // 14

            dgvProdForImport.DataSource = dtProdForImport;
            Common.Common.InitializeFooterGrid(dgvProdForImport, dgvFooter);
            Common.Common.InitializeFooterGrid(dgvPO_Detail, dgvFooterOfPODetail);
            UpdateFooterOfPoDetail();

            dtProdForImportForUpdateDB = ImportProductDAO.GetImportProductDetailForm();
        }

        private void LoadData()
        {
            // Load data common
            if (!CacheManager.Exists(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL))
            {
                dtImportProducts = ImportProductDAO.GetImportProducts();
                CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, dtImportProducts);
                dgvImports.DataSource = dtImportProducts;
            }
            else
            {
                dtImportProducts = CacheManager.Get<DataTable>(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL);
                dgvImports.DataSource = dtImportProducts;
            }

            if (dgvImports.Rows.Count > 0)
            {
                Guid imId = Guid.Parse(dgvImports.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId)))
                {
                    dtImportProductDetailById = ImportProductDAO.GetImportProductDetailByID(imId);
                    CacheManager.Add(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId), dtImportProductDetailById);
                    dgvImportDetail.DataSource = dtImportProductDetailById;
                }
                else
                {
                    dtImportProductDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId));
                    dgvImportDetail.DataSource = dtImportProductDetailById;
                }
            }

            // Load data for Import
            if (!CacheManager.Exists(CacheKeys.POS_DATATABLE_ALL_PO))
            {
                dtPos = PoDAO.GetPosForImportProduct();
                CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, dtPos.Copy());
                dgvPOs.DataSource = dtPos;
            }
            else
            {
                dtPos = CacheManager.Get<DataTable>(CacheKeys.POS_DATATABLE_ALL_PO);
                dgvPOs.DataSource = CacheManager.Get<DataTable>(CacheKeys.POS_DATATABLE_ALL_PO);
            }

            if (dtPos != null && dgvPOs.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOs.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)))
                {
                    dtPoById = PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById.Copy());
                    dgvPO_Detail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPO_Detail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
                dgvPOs.Rows[0].Selected = true;
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, PoDAO.GetPosForImportProduct());
            LoadData();
        }

        private void btnAddImport_Click(object sender, EventArgs e)
        {
            if (dgvPO_Detail.Rows.Count > 0)
            {
                MessageBoxHelper.ShowWarning("Please import all goods according to PO !");
                return;
            }

            Int32 totalQty = 0;

            foreach (DataGridViewRow row in dgvProdForImport.Rows)
            {
                if (Int32.TryParse(row.Cells[12].Value?.ToString(), out Int32 qty))
                {
                    totalQty += qty;
                }
            }

            Import_Products import_Products = new Import_Products()
            {
                Id = Guid.NewGuid(),
                FromPONo = dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[1].Value.ToString().Trim(),
                ImportDate = DateTime.Now,
                ImportDay = DateTime.Now.Day,
                ImportMonth = DateTime.Now.Month,
                ImportYear = DateTime.Now.Year,
                Import_Total_Qty = totalQty,
                Staff_Id = ShareData.UserId,
            };

            // Convert dtProdForImport to dtProdForImportUpdateDB
            foreach (DataRow item in dtProdForImport.Rows)
            {
                DataRow newRow = dtProdForImportForUpdateDB.NewRow();
                newRow[0] = Guid.NewGuid();
                newRow[1] = import_Products.Id;
                newRow[2] = item[0];
                newRow[3] = item[14];
                newRow[4] = item[12];

                dtProdForImportForUpdateDB.Rows.Add(newRow);
            }

            if (ImportProductDAO.Insert(import_Products))
            {
                if (ImportProductDAO.InsertImportProdDetail(dtProdForImportForUpdateDB) &&
                    PoDAO.UpdateIsImportedForPO(true, Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString())))
                {
                    MessageBoxHelper.ShowInfo("Successfully imported goods to warehouses");
                }
                else
                {
                    ImportProductDAO.DeleteImportProduct(import_Products.Id);
                    MessageBoxHelper.ShowWarning("Unsuccessfully imported goods to warehouses");
                }
            }
            else
            {
                MessageBoxHelper.ShowWarning("Unsuccessfully imported goods to warehouses");
            }

            dtProdForImport.Rows.Clear();

            prodsAdded.Clear();
            dgvPOs.Enabled = true;

            UpdateFooter();
            UpdateFooterOfPoDetail();
            dgvPOs.Enabled = true;

            dtPos = PoDAO.GetPosForImportProduct();
            CacheManager.Add(CacheKeys.POS_DATATABLE_ALL_PO, dtPos.Copy());
            dgvPOs.DataSource = dtPos;

            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, ImportProductDAO.GetImportProducts());
            LoadData();
        }

        private void btnAddAllProdToImport_Click(object sender, EventArgs e)
        {
            if (dgvPO_Detail.Rows.Count <= 0) return;
            int rsl = dgvPO_Detail.CurrentRow.Index;
            this.rslOld = dgvPOs.CurrentRow.Index;
            tlsPONo.Text = $"PO No: [{dgvPOs.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";

            dtProdForImport.Clear();
            prodsAdded.Clear();
            dgvProdForImport.Refresh();

            for (int i = 0; i < dgvPO_Detail.Rows.Count; i++)
            {
                Guid prodId = Guid.Parse(dgvPO_Detail.Rows[i].Cells[2].Value.ToString());
                DataRow dataRow = dtProdForImport.NewRow();
                dataRow[0] = prodId;
                dataRow[1] = dgvPO_Detail.Rows[i].Cells[3].Value.ToString().Trim();
                dataRow[2] = dgvPO_Detail.Rows[i].Cells[4].Value.ToString().Trim().ToUpper();
                dataRow[3] = "";
                dataRow[4] = dgvPO_Detail.Rows[i].Cells[5].Value.ToString().Trim().ToUpper();
                dataRow[5] = (dgvPO_Detail.Rows[i].Cells[6].Value.ToString().Trim());
                dataRow[6] = (dgvPO_Detail.Rows[i].Cells[7].Value.ToString().Trim());
                dataRow[7] = (dgvPO_Detail.Rows[i].Cells[8].Value.ToString().Trim());
                dataRow[8] = (dgvPO_Detail.Rows[i].Cells[9].Value.ToString().Trim());
                dataRow[9] = (dgvPO_Detail.Rows[i].Cells[10].Value.ToString().Trim());
                dataRow[10] = (dgvPO_Detail.Rows[i].Cells[11].Value.ToString().Trim());
                dataRow[11] = (dgvPO_Detail.Rows[i].Cells[12].Value.ToString().Trim()); 
                dataRow[12] = CheckOrReturnNumber(dgvPO_Detail.Rows[i].Cells[13].Value.ToString().Trim()); // Qty
                dataRow[13] = ""; // Warehouse name
                dataRow[14] = "";

                dtProdForImport.Rows.Add(dataRow);
                //prodsAdded.Add(prodId);
                totalAmount += CheckOrReturnNumber(dgvPO_Detail.Rows[i].Cells[13].Value.ToString().Trim());
            }

            dgvProdForImport.Rows[0].Selected = true;
            dgvPOs.Enabled = false;
            UpdateFooter();

            // Get data from cache before delete dtPoById
            Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
            var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)).Copy();

            // Remove all dgvPODetail
            dtPoById.Rows.Clear();

            // Set data for cache after delete dtPoById
            CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById_new);
            UpdateFooterOfPoDetail();
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

            foreach (DataGridViewRow row in dgvPO_Detail.Rows)
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

        private void dgvProdForImport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
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

        private void dgvFooterOfPODetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && !isSyncingScroll)
            {
                isSyncingScroll = true;
                dgvPO_Detail.HorizontalScrollingOffset = dgvFooterOfPODetail.HorizontalScrollingOffset;
                isSyncingScroll = false;
            }
        }

        private void btnClearProdsOfImport_Click(object sender, EventArgs e)
        {
            if (dgvProdForImport.Rows.Count <= 0) return;
            if (!MessageBoxHelper.Confirm($"You are in the process of importing products for PO No.: [{tlsPONo.Text.Trim()}].\n" +
                        $"Do you want to cancel the current operation?"))
            {
                return;
            }

            Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
            dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
            dgvPO_Detail.DataSource = dtPoById;

            dtProdForImport.Rows.Clear();

            prodsAdded.Clear();
            dgvPOs.Enabled = true;

            UpdateFooter();
            UpdateFooterOfPoDetail();
        }

        private void dgvPOs_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvPOs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPOs.CurrentCell == null) return;
            int currentRowIndex = dgvPOs.CurrentCell.RowIndex;

            if (dtPos != null && dgvPOs.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOs.Rows[currentRowIndex].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)))
                {
                    dtPoById = PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById);
                    dgvPO_Detail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                    dgvPO_Detail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId));
                }
            }
            UpdateFooterOfPoDetail();
        }

        private void dgvPO_Detail_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // Apply the changed width to the corresponding column in the footer
            if (e.Column.Index < dgvFooterOfPODetail.Columns.Count)
            {
                dgvFooterOfPODetail.Columns[e.Column.Index].Width = e.Column.Width;
            }

            // Resize for DataGridViewMain and DataGridViewFooter the same
            Common.Common.AdjustFooterScrollbar(dgvPO_Detail, dgvFooterOfPODetail);
        }

        private void dgvPO_Detail_Scroll(object sender, ScrollEventArgs e)
        {
            //dgvFooterOfPODetail.HorizontalScrollingOffset = dgvPODetail.HorizontalScrollingOffset;
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && !isSyncingScroll)
            {
                isSyncingScroll = true;
                dgvFooterOfPODetail.HorizontalScrollingOffset = dgvPO_Detail.HorizontalScrollingOffset;
                isSyncingScroll = false;
            }
        }

        private void dgvPO_Detail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPO_Detail.Columns["PO_DETAIL_QTY"].DefaultCellStyle.Format = "N0";
            dgvPO_Detail.Columns["PO_DETAIL_PRICE"].DefaultCellStyle.Format = "N0";
            dgvPO_Detail.Columns["PO_DETAIL_AMOUNT"].DefaultCellStyle.Format = "N0";
        }

        private void dgvPO_Detail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvPO_Detail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rsl = dgvPO_Detail.CurrentRow.Index;
            this.rslOld = dgvPOs.CurrentRow.Index;
            tlsPONo.Text = $"PO No: [{dgvPOs.Rows[this.rslOld].Cells[1].Value.ToString().Trim()}]\t";

            Guid prodId = Guid.Parse(dgvPO_Detail.Rows[rsl].Cells[2].Value.ToString());
            var maxQty = Int32.Parse(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString().Trim());
            frmImportForWarehouse frmImportForWarehouse = new frmImportForWarehouse(maxQty);
            frmImportForWarehouse.ShowDialog();
            if (!frmImportForWarehouse.IsAdd)
            {
                return;
            }

            Warehouses wModel = frmImportForWarehouse.Warehouse;

            if (prodsAdded.Contains(prodId + "|" + wModel.Id))
            {
                if (!MessageBoxHelper.Confirm($"You imported product [{dgvPO_Detail.Rows[rsl].Cells[3].Value.ToString()}] for Warehose [{wModel.Warehouse_Name}] ! \n" +
                    $"Do you want update Qty for product ?"))
                {
                    return;
                }

                // Update Qty for Prod of PO Detail
                dgvPO_Detail.Rows[rsl].Cells[13].Value = Int32.Parse(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString()) - frmImportForWarehouse.Qty;

                // Update Qty for Prod of dtWarehouse
                foreach (DataGridViewRow item in dgvProdForImport.Rows)
                {
                    if (item.Cells[0].Value.ToString().Trim().Equals(prodId.ToString()) &&
                        item.Cells[14].Value.ToString().Trim().Equals(wModel.Id.ToString()))
                    {
                        item.Cells[12].Value = frmImportForWarehouse.Qty;
                        UpdateFooterOfPoDetail();
                        break;
                    }
                }
            }
            else
            {
                // Get data from cache before delete Prod of PO Detail
                Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
                var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)).Copy();
                // Update Qty for Prod of PO Detail
                dgvPO_Detail.Rows[rsl].Cells[13].Value = Int32.Parse(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString()) - frmImportForWarehouse.Qty;
                CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById_new);

                DataRow dataRow = dtProdForImport.NewRow();
                dataRow[0] = prodId;
                dataRow[1] = dgvPO_Detail.Rows[rsl].Cells[3].Value.ToString().Trim();
                dataRow[2] = dgvPO_Detail.Rows[rsl].Cells[4].Value.ToString().Trim().ToUpper();
                dataRow[3] = "";
                dataRow[4] = dgvPO_Detail.Rows[rsl].Cells[5].Value.ToString().Trim().ToUpper();
                dataRow[5] = (dgvPO_Detail.Rows[rsl].Cells[6].Value.ToString().Trim());
                dataRow[6] = (dgvPO_Detail.Rows[rsl].Cells[7].Value.ToString().Trim());
                dataRow[7] = (dgvPO_Detail.Rows[rsl].Cells[8].Value.ToString().Trim());
                dataRow[8] = (dgvPO_Detail.Rows[rsl].Cells[9].Value.ToString().Trim());
                dataRow[9] = (dgvPO_Detail.Rows[rsl].Cells[10].Value.ToString().Trim());
                dataRow[10] = (dgvPO_Detail.Rows[rsl].Cells[11].Value.ToString().Trim());
                dataRow[11] = (dgvPO_Detail.Rows[rsl].Cells[12].Value.ToString().Trim());
                dataRow[12] = CheckOrReturnNumber(frmImportForWarehouse.Qty.ToString()); // Qty
                dataRow[13] = wModel.Warehouse_Name; // Warehouse name
                dataRow[14] = wModel.Id;
                dtProdForImport.Rows.Add(dataRow);
                prodsAdded.Add(prodId + "|" + wModel.Id);
            }

            totalAmount += CheckOrReturnNumber(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString().Trim());

            dgvProdForImport.Rows[0].Selected = true;
            dgvPOs.Enabled = false;
            UpdateFooter();
            UpdateFooterOfPoDetail();

            if (Int32.Parse(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString().Trim()) <= 0)
            {
                // Get data from cache before delete Prod of PO Detail
                Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
                var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAL_BY_ID, poId)).Copy();

                dgvPO_Detail.Rows.RemoveAt(rsl);

                // Set data for cache after delete dtPoById
                CacheManager.Add(string.Format(CacheKeys.PO_DETAL_BY_ID, poId), dtPoById_new);
                UpdateFooterOfPoDetail();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dgvImportDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvImportDetail.Columns["QTY_PRODUCT_IMPORT"].DefaultCellStyle.Format = "N0";
        }

        private void tlsReloadImportList_Click(object sender, EventArgs e)
        {
            dgvImports.Refresh();
            lblDateTimeSeacrh.Text = "";
            LoadData();
        }

        private void dgvImports_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvImportDetail_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvImports_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvImports.Rows.Count <= 0)
            {
                return;
            }

            int rsl = dgvImports.CurrentRow.Index;
            Guid imId = Guid.Parse(dgvImports.Rows[rsl].Cells[0].Value.ToString());
            if (!CacheManager.Exists(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId)))
            {
                dtImportProductDetailById = ImportProductDAO.GetImportProductDetailByID(imId);
                CacheManager.Add(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId), dtImportProductDetailById);
                dgvImportDetail.DataSource = dtImportProductDetailById;
            }
            else
            {
                dtImportProductDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId));
                dgvImportDetail.DataSource = dtImportProductDetailById;
            }
        }

        private void txtSearchImportList_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchImportList.Text))
            {
                dgvImports.Refresh();
            }
            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_IMPORT_PRODUCT_FROM_PO_NO,
                QueryStatement.PROPERTY_IMPORT_PRODUCT_STAFF_NAME
            };

            dgvImports.DataSource = Common.Common.Search(txtSearchImportList.Text.Trim(), dtImportProducts, lstProperty);
        }

        private void tlsSearchDate_Click(object sender, EventArgs e)
        {
            if (dgvImports.Rows.Count <= 0)
            {
                return;
            }
            frmSeacrhPOFromDate frmSeacrhPOFromDate = new frmSeacrhPOFromDate();
            frmSeacrhPOFromDate.ShowDialog();

            if (!frmSeacrhPOFromDate.IsSearch)
            {
                dgvImports.Refresh();
                return;
            }

            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_IMPORT_PRODUCT_IMPORT_DATE,
            };

            DateTime fDate = frmSeacrhPOFromDate.FromDate;
            DateTime tDate = frmSeacrhPOFromDate.ToDate;

            lblDateTimeSeacrh.Text = $"From: {fDate.ToString("dd/MM/yyyy")} To: {tDate.ToString("dd/MM/yyyy")}";
            dgvImports.DataSource = Common.Common.SearchDate(fDate, tDate, dtImportProducts, lstProperty);
        }
    }
}
