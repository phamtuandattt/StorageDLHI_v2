using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.PoGUI;
using StorageDLHI.BLL.ImportDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.BLL.WarehouseDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
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
        private DataTable dtWarehouseDetail = new DataTable();
        //DataTable dtWarehouseDetail = new DataTable();


        private DataTable dtPoDetailCoppy = new DataTable();
        private DataGridViewRow rowClonePODetail = new DataGridViewRow();

        // -----------------------------------------------------
        Panel pnlNoDataImport = new Panel();
        Panel pnlNoDataImportDetail = new Panel();
        Panel pnlNoDataPOList = new Panel();
        Panel pnlNoDataPODetail = new Panel();

        private bool isSyncingScroll = false;
        private int rslOld;
        private int previousRowIndex = -1;
        private Int32 totalAmount = 0;

        public ucImportProd()
        {
            InitializeComponent();
            // -----------------------------------------------------
            var ucF = new ucPanelNoData("No records found");
            pnlNoDataImport = ucF.pnlNoData;
            this.dgvImports.Controls.Add(pnlNoDataImport);

            var ucS = new ucPanelNoData("No records found");
            pnlNoDataImportDetail = ucS.pnlNoData;
            this.dgvImportDetail.Controls.Add(pnlNoDataImportDetail);

            var ucP = new ucPanelNoData("No records found");
            pnlNoDataPOList = ucP.pnlNoData;
            this.dgvPOs.Controls.Add(pnlNoDataPOList);

            var ucPdetail = new ucPanelNoData("No records found");
            pnlNoDataPODetail = ucPdetail.pnlNoData;
            this.dgvPO_Detail.Controls.Add(pnlNoDataPODetail);

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

            //dtWarehouseDetail = WarehouseDAO.GetWarehouseDetailForm();
            dtWarehouseDetail.Columns.Add("ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("WAREHOUSE_ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("PRODUCT_ID", typeof(Guid));
            dtWarehouseDetail.Columns.Add("PRODUCT_IN_STOCK", typeof(Int32));

            if (dgvPO_Detail.Rows.Count > 0)
            {
                rowClonePODetail = (DataGridViewRow)dgvPO_Detail.Rows[0].Clone();
            }   
        }


        private async void LoadData()
        {
            // Load data common
            if (!CacheManager.Exists(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL))
            {
                dtImportProducts = await ImportProductDAO.GetImportProducts();
                CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, await ImportProductDAO.GetImportProducts());
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
                    dtImportProductDetailById = await ImportProductDAO.GetImportProductDetailByID(imId);
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
            if (!CacheManager.Exists(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD))
            {
                dtPos = await PoDAO.GetPosForImportProduct();
                CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, dtPos.Copy());
                dgvPOs.DataSource = dtPos;
            }
            else
            {
                dtPos = CacheManager.Get<DataTable>(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD);
                dgvPOs.DataSource = CacheManager.Get<DataTable>(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD);
            }

            if (dtPos != null && dgvPOs.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOs.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)))
                {
                    dtPoById = await ShowDialogManager.WithLoader(() => PoDAO.GetPODetailById(poId));
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId), dtPoById.Copy());
                    dgvPO_Detail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId));
                    dgvPO_Detail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId));
                }
                dgvPOs.Rows[0].Selected = true;
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvPOs, pnlNoDataPOList);
                Common.Common.ShowNoDataPanel(dgvPO_Detail, pnlNoDataPODetail);
            }
            dtProdForImportForUpdateDB = await ImportProductDAO.GetImportProductDetailForm();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            //CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, PoDAO.GetPosForImportProduct());
            LoadData();
            if (dgvPOs.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOs, pnlNoDataPOList);
                Common.Common.ShowNoDataPanel(dgvPO_Detail, pnlNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnlNoDataPOList);
                Common.Common.HideNoDataPanel(pnlNoDataPODetail);
            }
        }

        private async void btnAddImport_Click(object sender, EventArgs e)
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

            // Import prod into warehouse
            foreach (DataRow item in dtProdForImport.Rows)
            {
                DataRow dataRow = dtWarehouseDetail.NewRow();
                dataRow[0] = Guid.NewGuid();
                dataRow[1] = item[14];
                dataRow[2] = item[0];
                dataRow[3] = item[12];

                dtWarehouseDetail.Rows.Add(dataRow);
            }

            if (await ImportProductDAO.Insert(import_Products))
            {
                if (ImportProductDAO.InsertImportProdDetail(dtProdForImportForUpdateDB) 
                    && WarehouseDAO.UpdateQtyProdOfWarehouse(dtWarehouseDetail)
                    && await ShowDialogManager.WithLoader(() => PoDAO.UpdateIsImportedForPO(true, Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString()))))
                {
                    MessageBoxHelper.ShowInfo("Successfully imported goods to warehouses");
                }
                else
                {
                    ImportProductDAO.DeleteImportProduct(import_Products.Id);
                    MessageBoxHelper.ShowWarning("Unsuccessfully imported goods to warehouses");
                    return;
                }
            }
            else
            {
                MessageBoxHelper.ShowWarning("Unsuccessfully imported goods to warehouses");
                return;
            }

            dtProdForImport.Rows.Clear();
            tlsPONo.Text = "";
            prodsAdded.Clear();
            dgvPOs.Enabled = true;

            UpdateFooter();
            UpdateFooterOfPoDetail();
            dgvPOs.Enabled = true;

            dtPos = await PoDAO.GetPosForImportProduct();
            CacheManager.Add(CacheKeys.POS_DATETABLE_GET_ALL_PO_FOR_IMPORT_PROD, dtPos.Copy());
            dgvPOs.DataSource = dtPos;

            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, await ImportProductDAO.GetImportProducts());
            CacheManager.Add(CacheKeys.WAREHOUSE_DATATABLE_ALL, await WarehouseDAO.GetWarehouses());
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
            var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)).Copy();

            // Remove all dgvPODetail
            dtPoById.Rows.Clear();

            // Set data for cache after delete dtPoById
            CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId), dtPoById_new);
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
            dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId));
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

        private async void dgvPOs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPOs.CurrentCell == null) return;
            int currentRowIndex = dgvPOs.CurrentCell.RowIndex;

            if (dtPos != null && dgvPOs.Rows.Count > 0)
            {
                Guid poId = Guid.Parse(dgvPOs.Rows[currentRowIndex].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)))
                {
                    dtPoById = await PoDAO.GetPODetailById(poId);
                    CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId), dtPoById);
                    dgvPO_Detail.DataSource = dtPoById;
                }
                else
                {
                    dtPoById = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId));
                    dgvPO_Detail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId));
                }
            }
            UpdateFooterOfPoDetail();
            dtPoDetailCoppy.Clear();
            dtPoDetailCoppy = dtPoById.Copy();
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
            frmImportForWarehouse frmImportForWarehouse = new frmImportForWarehouse(TitleManager.IMPORT_ADD_QUANTITY_IMPORT, maxQty);
            frmImportForWarehouse.ShowDialog();
            if (!frmImportForWarehouse.IsAdd || frmImportForWarehouse.Qty == 0)
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
                        item.Cells[12].Value = Int32.Parse(item.Cells[12].Value.ToString().Trim()) + frmImportForWarehouse.Qty;
                        UpdateFooterOfPoDetail();
                        break;
                    }
                }
            }
            else
            {
                // Get data from cache before delete Prod of PO Detail
                Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
                var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)).Copy();
                // Update Qty for Prod of PO Detail
                dgvPO_Detail.Rows[rsl].Cells[13].Value = Int32.Parse(dgvPO_Detail.Rows[rsl].Cells[13].Value.ToString()) - frmImportForWarehouse.Qty;
                CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId), dtPoById_new);

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
                var dtPoById_new = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)).Copy();

                dgvPO_Detail.Rows.RemoveAt(rsl);

                // Set data for cache after delete dtPoById
                CacheManager.Add(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId), dtPoById_new);
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

        private async void dgvImports_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvImports.Rows.Count <= 0)
            {
                return;
            }

            int rsl = dgvImports.CurrentRow.Index;
            Guid imId = Guid.Parse(dgvImports.Rows[rsl].Cells[0].Value.ToString());
            if (!CacheManager.Exists(string.Format(CacheKeys.IMPORT_PRODUCT_DETIAL_BY_ID, imId)))
            {
                dtImportProductDetailById = await ImportProductDAO.GetImportProductDetailByID(imId);
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

            dgvImports.DataSource = Common.Common.Search(txtSearchImportList.Text.Trim(), dtImportProducts.Copy(), lstProperty);

            if (dgvImports.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvImports, pnlNoDataImport);
                Common.Common.ShowNoDataPanel(dgvImportDetail, pnlNoDataImportDetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnlNoDataImport);
                Common.Common.HideNoDataPanel(pnlNoDataImportDetail);
            }
        }

        private void tlsSearchDateForImports_Click(object sender, EventArgs e)
        {
            if (dgvImports.Rows.Count <= 0)
            {
                return;
            }
            tlsClearSeacrhDate.Visible = true;
            frmSeacrhPOFromDate frmSeacrhPOFromDate = new frmSeacrhPOFromDate();
            frmSeacrhPOFromDate.ShowDialog();

            if (!frmSeacrhPOFromDate.IsSearch)
            {
                dgvImports.Refresh();
                return;
            }

            DateTime fDate = frmSeacrhPOFromDate.FromDate;

            lblDateTimeSeacrh.Text = $"From: {fDate.ToString("dd/MM/yyyy")}";

            DataView dvFilter = Common.Common.SearchDateFrom(fDate, dtImportProducts.Copy(), QueryStatement.PROPERTY_IMPORT_PRODUCT_IMPORT_DATE);

            dgvImports.DataSource = dvFilter;

            if (dgvImports.Rows.Count <= 0)
            {
                // Hide all row in dgvImportDetail
                Common.Common.ShowNoDataPanel(dgvImports, pnlNoDataImport);
                Common.Common.ShowNoDataPanel(dgvImportDetail, pnlNoDataImportDetail);
            }
        }

        private void tlsClearSeacrhDate_Click(object sender, EventArgs e)
        {
            lblDateTimeSeacrh.Text = "";
            dgvImports.Refresh();
            dgvImports.DataSource = CacheManager.Get<DataTable>(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL).Copy();
            tlsClearSeacrhDate.Visible = false;
            dgvImports.Visible = true;
            Common.Common.HideNoDataPanel(pnlNoDataImport);
            Common.Common.HideNoDataPanel(pnlNoDataImportDetail);
        }

        private void tlsSearchPOs_KeyUp(object sender, KeyEventArgs e)
        {
            if (dgvPOs.Rows.Count <= 0) return;
            var lstProperty = new List<string>()
            {
                QueryStatement.PROPERTY_PO_NO,
                QueryStatement.PROPERTY_PO_MPR_NO,
                QueryStatement.PROPERTY_PO_WO_NO,
                QueryStatement.PROPERTY_PO_PROJECT_NAME,
            };

            dgvPOs.DataSource = Common.Common.Search(tlsSearchPOs.Text.Trim(), dtPos.Copy(), lstProperty);

            if (dgvPOs.Rows.Count <= 0)
            {
                Common.Common.ShowNoDataPanel(dgvPOs, pnlNoDataPOList);
                Common.Common.ShowNoDataPanel(dgvPO_Detail, pnlNoDataPODetail);
            }
            else
            {
                Common.Common.HideNoDataPanel(pnlNoDataPOList);
                Common.Common.HideNoDataPanel(pnlNoDataPODetail);
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdForImport.Rows.Count <= 0) return;
            int rsl = dgvProdForImport.CurrentRow.Index;

            Guid prodId_OfdgvProdForImport = Guid.Parse(dgvProdForImport.Rows[rsl].Cells[0].Value.ToString());
            Int32 qtyOfModify = Int32.Parse(dgvProdForImport.Rows[rsl].Cells[12].Value.ToString());
            string prodName = dgvProdForImport.Rows[rsl].Cells[1].Value.ToString();

            Products pModel = new Products()
            {
                Id = prodId_OfdgvProdForImport,
                Product_Name = prodName,
                Product_Des_2 = dgvProdForImport.Rows[rsl].Cells[2].Value.ToString().Trim().ToUpper(),
                Product_Code = "",
                Product_Material_Code = dgvProdForImport.Rows[rsl].Cells[4].Value.ToString().Trim().ToUpper(),
                A_Thinhness = dgvProdForImport.Rows[rsl].Cells[5].Value.ToString().Trim(),
                B_Depth = (dgvProdForImport.Rows[rsl].Cells[6].Value.ToString().Trim()),
                C_Witdh = (dgvProdForImport.Rows[rsl].Cells[7].Value.ToString().Trim()),
                D_Web = (dgvProdForImport.Rows[rsl].Cells[8].Value.ToString().Trim()),
                E_Flag = (dgvProdForImport.Rows[rsl].Cells[9].Value.ToString().Trim()),
                F_Length = (dgvProdForImport.Rows[rsl].Cells[10].Value.ToString().Trim()),
                G_Weight = (dgvProdForImport.Rows[rsl].Cells[11].Value.ToString().Trim()),
            };

            frmUpdateImportedProduct frmUpdateImportedProduct = new frmUpdateImportedProduct(prodId_OfdgvProdForImport, qtyOfModify, prodName);
            frmUpdateImportedProduct.ShowDialog();

            if (!frmUpdateImportedProduct.IsUpdated || !frmUpdateImportedProduct.IsCompleted)
            {
                return;
            }
            dgvProdForImport.Rows.RemoveAt(rsl);
            foreach (var item in frmUpdateImportedProduct.ListUpdateImportProd)
            {
                var pArr = item.Split('|');
                var found = false;

                foreach (DataGridViewRow dgvR in dgvProdForImport.Rows)
                {
                    // Update Qty for existed row
                    if (Guid.Parse(dgvR.Cells[0].Value.ToString()).Equals(Guid.Parse(pArr[0])) 
                        && Guid.Parse(dgvR.Cells[14].Value.ToString()).Equals(Guid.Parse(pArr[3])))
                    {
                        dgvR.Cells[12].Value = Int32.Parse(dgvR.Cells[12].Value.ToString()) + Int32.Parse(pArr[1]);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    // Add new row
                    DataRow dataRow = dtProdForImport.NewRow();
                    dataRow[0] = Guid.Parse(pArr[0]);// ProdID
                    dataRow[1] = pModel.Product_Name;
                    dataRow[2] = pModel.Product_Des_2;
                    dataRow[3] = "";
                    dataRow[4] = pModel.Product_Material_Code;
                    dataRow[5] = pModel.A_Thinhness;
                    dataRow[6] = pModel.B_Depth;
                    dataRow[7] = pModel.C_Witdh;
                    dataRow[8] = pModel.D_Web;
                    dataRow[9] = pModel.E_Flag;
                    dataRow[10] = pModel.F_Length;
                    dataRow[11] = pModel.G_Weight;
                    dataRow[12] = Int32.Parse(pArr[1]);// Qty
                    dataRow[13] = pArr[2]; // 
                    dataRow[14] = Guid.Parse(pArr[3]); // Warehouse Id

                    dtProdForImport.Rows.Add(dataRow);
                }
            }
            dgvProdForImport.Rows[0].Selected = true;
            UpdateFooter();
            UpdateFooterOfPoDetail();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdForImport.Rows.Count <= 0) return;
            int rsl = dgvProdForImport.CurrentRow.Index;

            Guid prodId_OfdgvProdForImport = Guid.Parse(dgvProdForImport.Rows[rsl].Cells[0].Value.ToString());
            Int32 qtyOfModify = Int32.Parse(dgvProdForImport.Rows[rsl].Cells[12].Value.ToString());
            Guid warehouseId = Guid.Parse(dgvProdForImport.Rows[rsl].Cells[14].Value.ToString());

            bool found = false;
            foreach (DataGridViewRow item in dgvPO_Detail.Rows)
            {
                // If still available, update qty => Remove row at dgvProdForImport
                if (Guid.Parse(item.Cells[2].Value.ToString()).Equals(prodId_OfdgvProdForImport))
                {
                    item.Cells[13].Value = Int32.Parse(item.Cells[13].Value.ToString()) + qtyOfModify;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // Otherwise, add new row
                Guid poId = Guid.Parse(dgvPOs.Rows[dgvPOs.CurrentRow.Index].Cells[0].Value.ToString());
                DataTable dtClone = CacheManager.Get<DataTable>(string.Format(CacheKeys.PO_DETAIL_BY_ID_FOR_IMPORT_PROD, poId)).Copy();
                foreach (DataRow item in dtClone.Rows)
                {
                    if (Guid.Parse(item[2].ToString()).Equals(prodId_OfdgvProdForImport))
                    {
                        DataRow dataRow = dtPoById.NewRow();
                        dataRow[0] = item[0];
                        dataRow[1] = item[1];
                        dataRow[2] = item[2];
                        dataRow[3] = item[3];
                        dataRow[4] = item[4];
                        dataRow[5] = item[5];
                        dataRow[6] = item[6];
                        dataRow[7] = item[7];
                        dataRow[8] = item[8];
                        dataRow[9] = item[9];
                        dataRow[10] = item[10];
                        dataRow[11] = item[11];
                        dataRow[12] = item[12];
                        dataRow[13] = qtyOfModify;
                        dataRow[14] = item[14];
                        dataRow[15] = item[15];

                        dtPoById.Rows.Add(dataRow);
                        break;
                    }
                }
            }
            dgvProdForImport.Rows.RemoveAt(rsl);

            prodsAdded.Remove(prodId_OfdgvProdForImport + "|" + warehouseId);
            dgvPO_Detail.Rows[0].Selected = true;

            UpdateFooter();
            UpdateFooterOfPoDetail();
        }

        private void dgvProdForImport_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvProdForImport.Rows.Count <= 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.dgvProdForImport.ClearSelection();
                    this.dgvProdForImport.Rows[rowSelected].Selected = true;
                }
            }
        }

        private void dgvProdForImport_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
    }
}
