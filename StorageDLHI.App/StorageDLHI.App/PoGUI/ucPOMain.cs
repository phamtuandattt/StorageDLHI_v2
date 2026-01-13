using ComponentFactory.Krypton.Toolkit;
using log4net.Appender;
using StorageDLHI.App.Common;
using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.ImportDAO;
using StorageDLHI.BLL.MprDAO;
using StorageDLHI.BLL.PoDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.BLL.ProjectDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private double totalAmount = 0;
        private string CurrencyDefault = "";

        private bool _projectIsLoad = false;
        private DataTable dtProjects = new DataTable();


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
            dtProdsOfAddPO.Columns.Add("PO_AMOUNT", typeof(double));
            dtProdsOfAddPO.Columns.Add("PO_RECEVIE");
            dtProdsOfAddPO.Columns.Add("PO_REMARKS");

            dtProdsOfAddPO.Columns.Add("TAX_VALUE");
            dtProdsOfAddPO.Columns.Add("FORMULA");
            dtProdsOfAddPO.Columns.Add("CURRENCY");

            dtProdsOfAddPO.Columns.Add("COST_ID");
            dtProdsOfAddPO.Columns.Add("TAX_ID");

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

        private async void ucPOMain_Load(object sender, EventArgs e)
        {
            Common.Common.SetupComboxOfToolStrip(this.cboProjectForAddPO, QueryStatement.PROPERTY_PROJECT_NAME, QueryStatement.PROPERTY_PROJECT_ID);

            await LoadProjets();
            if (_projectIsLoad)
            {
                var projectId = Guid.Parse(cboProjectForAddPO.ComboBox.SelectedValue.ToString().Trim());
                await LoadData();
                await LoadMPRByProjectForCreatePO(projectId);
            }
        }

        private async Task LoadData()
        {
            // --------- LOAD POs
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

            // ---------------------------------------------------
            // Load MPRs
        }

        private async Task LoadMPRByProjectForCreatePO(Guid projectId)
        {
            Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);

            if (!CacheManager.Exists(string.Format(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS_OF_PROJECT, projectId)))
            {
                dtMprs = await ShowDialogManager.WithLoader(() => MprDAO.GetMprsForMakePO(projectId));
                CacheManager.Add(string.Format(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS_OF_PROJECT, projectId), dtMprs);
                dgvMPRs.DataSource = dtMprs;
            }
            else
            {
                dtMprs = await ShowDialogManager.WithLoader(() => MprDAO.GetMprsForMakePO(projectId));
                dgvMPRs.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS_OF_PROJECT, projectId));
            }
            if (dtMprs != null && dtMprs.Rows.Count > 0 && dgvMPRs.Rows.Count > 0)
            {
                Common.Common.ConfigDataGridView(dtMprs, dgvMPRs, Common.Common.GetHiddenColumns(QueryStatement.HiddenColumnDataGridViewOfMprs));
                Common.Common.HideNoDataPanel(pnNoDataMprs);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
            }

            //-----------------------

            if (dtMprs.Rows.Count > 0 && dtMprs != null && dgvMPRs.Rows.Count > 0)
            {
                var mprId = Guid.Parse(dgvMPRs.Rows[0].Cells[0].Value.ToString());
                if (!CacheManager.Exists(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId)))
                {
                    dtMprDetailById = await MprDAO.GetMprDetailByMpr(mprId);
                    CacheManager.Add(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId), dtMprDetailById);
                    //dgvMPRDetail.DataSource = dtMprDetailById;
                }
                else
                {
                    dtMprDetailById = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
                    //dgvMPRDetail.DataSource = CacheManager.Get<DataTable>(string.Format(CacheKeys.MPR_DETAIL_BY_ID_FOR_POS, mprId));
                }
                if (dtMprDetailById != null && dtMprDetailById.Rows.Count > 0)
                {
                    Common.Common.ConfigDataGridView(dtMprDetailById, dgvMPRDetail, Common.Common.GetHiddenColumns(QueryStatement.HiddenColumnDataGirdViewOfMprDetails));
                    //Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
                }
                else
                {
                    Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
                }

                dtMprDetailByIdTemporary = dtMprDetailById.Copy();
                dtMprDetailByIdTemporary.Clear();

                //Common.Common.HideNoDataPanel(pnNoDataMprs);
                //Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
        }

        private async Task LoadProjets()
        {
            // ----- LOAD DATA FOR PROD OF CREATE MPR
            if (!CacheManager.Exists(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX)
                && !CacheManager.Exists(CacheKeys.PROJECT_DATATABLE))
            {
                var dtCommon = await ProjectDAO.GetProjects();
                if (dtCommon != null && dtCommon.dtProjects != null && dtCommon.dtProjectForCombox != null
                    && dtCommon.dtProjects.AsEnumerable().Any() && dtCommon.dtProjectForCombox.AsEnumerable().Any())
                {
                    var dtCombobox = dtCommon.dtProjectForCombox;
                    this.dtProjects = dtCommon.dtProjects;

                    cboProjectForAddPO.ComboBox.DataSource = dtCombobox;

                    CacheManager.Add(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX, dtCombobox);
                    CacheManager.Add(CacheKeys.PROJECT_DATATABLE, dtProjects);
                    _projectIsLoad = true;
                }
                else
                {
                    MessageBoxHelper.ShowError("Please add project and MPRs before create POs !");
                    return;
                }
            }
            else
            {
                cboProjectForAddPO.ComboBox.DataSource = CacheManager.Get<DataTable>(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX);

                this.dtProjects = CacheManager.Get<DataTable>(CacheKeys.PROJECT_DATATABLE);
                _projectIsLoad = true;
            }
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            //CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, await MprDAO.GetMprsForMakePO());
            //await LoadData();
            //if (dgvMPRs.Rows.Count <= 0)
            //{
            //    Common.Common.ShowNoDataPanel(dgvMPRs, pnNoDataMprs);
            //    Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            //}
            //else
            //{
            //    Common.Common.HideNoDataPanel(pnNoDataMprs);
            //    Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            //}
            _projectIsLoad = false;
            await LoadData();
            await LoadProjets();
            if (_projectIsLoad)
            {
                var projectId = Guid.Parse(cboProjectForAddPO.ComboBox.SelectedValue.ToString().Trim());

                var reloadModel = await ShowDialogManager.WithLoader(() => MprDAO.GetMprsForMakePO(projectId));
                CacheManager.Add(string.Format(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS_OF_PROJECT, projectId), reloadModel);

                await LoadMPRByProjectForCreatePO(projectId);
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
            if (dtMprDetailById != null && dtMprDetailById.Rows.Count > 0
                     && dgvMPRDetail.Rows.Count > 0)
            {
                Common.Common.ConfigDataGridView(dtMprDetailById, dgvMPRDetail, Common.Common.GetHiddenColumns(QueryStatement.HiddenColumnDataGirdViewOfMprDetails));
                Common.Common.HideNoDataPanel(pnNoDataMprsDetail);
            }
            else
            {
                Common.Common.ShowNoDataPanel(dgvMPRDetail, pnNoDataMprsDetail);
            }
            UpdateFooter();
        }

        private void dgvMPRDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMPRDetail.Rows.Count <= 0) return;
            int rsl = dgvMPRDetail.CurrentRow.Index;

            // Dictionary info prod
            var variable = new Dictionary<string, string>
            {
                {QueryStatement.PROPERTY_PROD_A, dgvMPRDetail.Rows[rsl].Cells[6].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_B, dgvMPRDetail.Rows[rsl].Cells[7].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_C, dgvMPRDetail.Rows[rsl].Cells[8].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_D, dgvMPRDetail.Rows[rsl].Cells[9].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_E, dgvMPRDetail.Rows[rsl].Cells[10].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_F, dgvMPRDetail.Rows[rsl].Cells[11].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_G, dgvMPRDetail.Rows[rsl].Cells[12].Value.ToString().Trim()},
                {QueryStatement.QTY_PARA, dgvMPRDetail.Rows[rsl].Cells[13].Value.ToString().Trim()},

                {QueryStatement.PROPERTY_CURRENCY, this.CurrencyDefault}
            };

            var prod = new Products()
            {
                Id = Guid.Parse(dgvMPRDetail.Rows[rsl].Cells[2].Value.ToString().Trim()),
                Product_Name = dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString().Trim(),
                Product_Des_2 = dgvMPRDetail.Rows[rsl].Cells[3].Value.ToString().Trim(),
                Product_Material_Code = dgvMPRDetail.Rows[rsl].Cells[4].Value.ToString().Trim(),
                A_Thinhness = dgvMPRDetail.Rows[rsl].Cells[6].Value.ToString().Trim(),
                B_Depth = dgvMPRDetail.Rows[rsl].Cells[7].Value.ToString().Trim(),
                C_Witdh = dgvMPRDetail.Rows[rsl].Cells[8].Value.ToString().Trim(),
                D_Web = dgvMPRDetail.Rows[rsl].Cells[9].Value.ToString().Trim(),
                E_Flag = dgvMPRDetail.Rows[rsl].Cells[10].Value.ToString().Trim(),
                F_Length = dgvMPRDetail.Rows[rsl].Cells[11].Value.ToString().Trim(),
                G_Weight = dgvMPRDetail.Rows[rsl].Cells[12].Value.ToString().Trim(),
            };

            frmAddPriceForProdPO frmAddPriceForProdPO = new frmAddPriceForProdPO(variable, prod, true);
            frmAddPriceForProdPO.ShowDialog();
            
            if (frmAddPriceForProdPO.Price == 0 || frmAddPriceForProdPO.NetCash == 0) { return; }

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
            dataRow[14] = frmAddPriceForProdPO.NetCash.ToString().Trim(); // Amount
            dataRow[15] = frmAddPriceForProdPO.Recevie;
            dataRow[16] = frmAddPriceForProdPO.Remark;
            dataRow[17] = frmAddPriceForProdPO.TaxValue; // TAX_VALUE
            dataRow[18] = frmAddPriceForProdPO.Formula; // FORMULA
            dataRow[19] = frmAddPriceForProdPO.CurrencyOption; // CURRENCY

            dataRow[20] = frmAddPriceForProdPO.CostID;
            dataRow[21] = frmAddPriceForProdPO.TaxID;

            this.CurrencyDefault = frmAddPriceForProdPO.CurrencyOption;

            totalAmount += double.Parse(frmAddPriceForProdPO.NetCash.ToString()); // Amount
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

            //CacheManager.Add(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS, await MprDAO.GetMprsForMakePO());
            var projectId = Guid.Parse(cboProjectForAddPO.ComboBox.SelectedValue.ToString().Trim());
            CacheManager.Add(string.Format(CacheKeys.MPRS_DATATABLE_ALL_MPRS_FOR_POS_OF_PROJECT, projectId), dtMprs);

            CacheManager.Add(CacheKeys.IMPORT_PRODUCT_DATATABLE_ALL, await ImportProductDAO.GetImportProducts());

            await LoadData();
        }

        private void UpdateFooter()
        {
            double totalQty = 0;
            double totalPrice = 0;
            double totalAmount = 0;

            foreach (DataGridViewRow row in dgvProdOfPO.Rows)
            {
                if (double.TryParse(row.Cells[12].Value?.ToString(), out double qty))
                {
                    totalQty += qty;
                }

                if (double.TryParse(row.Cells[13].Value?.ToString(), out double price))
                {
                    totalPrice += price;
                }

                if (double.TryParse(row.Cells[14].Value?.ToString(), out double amount))
                {
                    totalAmount += amount;
                }
            }

            dgvFooter.Rows[0].Cells[1].Value = "TOTAL";
            dgvFooter.Rows[0].Cells[12].Value = totalQty;
            dgvFooter.Rows[0].Cells[13].Value = totalPrice.ToString("N2");
            dgvFooter.Rows[0].Cells[14].Value = totalAmount.ToString("N2");

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
            dgvProdOfPO.Columns["QTY"].DefaultCellStyle.Format = "N2";
            dgvProdOfPO.Columns["PO_PRICE"].DefaultCellStyle.Format = "N2";
            dgvProdOfPO.Columns["PO_AMOUNT"].DefaultCellStyle.Format = "N2";
        }

        private void dgvMPRDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvMPRDetail.Columns["A_THINHNESS_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["B_DEPTH_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["C_WIDTH_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["D_WEB_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["E_FLAG_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["F_LENGTH_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["G_WEIGHT_M"].DefaultCellStyle.Format = "N0";
            //dgvMPRDetail.Columns["MPR_QTY_M"].DefaultCellStyle.Format = "N0";
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
            dgvPOList.Columns["PO_TOTAL_AMOUNT"].DefaultCellStyle.Format = "N2";
        }

        private void dgvPODetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPODetail.Columns["PO_DETAIL_QTY"].DefaultCellStyle.Format = "N2";
            dgvPODetail.Columns["PO_DETAIL_PRICE"].DefaultCellStyle.Format = "N2";
            dgvPODetail.Columns["PO_DETAIL_AMOUNT"].DefaultCellStyle.Format = "N2";
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

        private async void tlsReloadPOs_Click(object sender, EventArgs e)
        {
            CacheManager.Remove(CacheKeys.POS_DATATABLE_ALL_PO);
            lblDateTimeSeacrh.Text = "";

            // Reload POs
            if (_projectIsLoad)
            {
                await LoadData();
            }

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

                SupplierId = Guid.Parse(dgvPOList.Rows[rsl].Cells[15].Value.ToString().Trim())
            };

            DataTable dtForExport = new DataTable();
            dtForExport.Columns.Add("PROD_CODE"); // new code - 0
            dtForExport.Columns.Add("PROD_NAME"); // 1
            dtForExport.Columns.Add("MATERIAL"); // 2
            dtForExport.Columns.Add("A"); // 3
            dtForExport.Columns.Add("B");
            dtForExport.Columns.Add("C");
            dtForExport.Columns.Add("QTY"); // 6
            dtForExport.Columns.Add("UNIT"); // 7
            dtForExport.Columns.Add("G"); // 8
            dtForExport.Columns.Add("MPR_NO_REV"); // 9
            dtForExport.Columns.Add("REQ_DATE"); // 10
            dtForExport.Columns.Add("PLACE_OF_ENTRY"); // 11
            dtForExport.Columns.Add("PRICE"); // 12
            dtForExport.Columns.Add("AMOUNT"); // 13
            dtForExport.Columns.Add("RECEIVE");
            dtForExport.Columns.Add("REMARKS");

            foreach (DataGridViewRow item in dgvPODetail.Rows)
            {
                DataRow dataRow = dtForExport.NewRow();
                dataRow[0] = item.Cells[19].Value.ToString().Trim();
                dataRow[1] = item.Cells[3].Value.ToString().Trim();
                dataRow[2] = item.Cells[5].Value.ToString().Trim();
                dataRow[3] = item.Cells[6].Value.ToString().Trim();
                dataRow[4] = item.Cells[7].Value.ToString().Trim();
                dataRow[5] = item.Cells[8].Value.ToString().Trim();
                dataRow[6] = item.Cells[13].Value.ToString().Trim(); //
                dataRow[7] = item.Cells[18].Value.ToString().Trim();
                dataRow[8] = item.Cells[12].Value.ToString().Trim();
                dataRow[9] = "";
                dataRow[10] = "";
                dataRow[11] = "";
                dataRow[12] = item.Cells[14].Value.ToString().Trim();
                dataRow[13] = item.Cells[15].Value.ToString().Trim();
                dataRow[14] = "";
                dataRow[15] = "";

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
                //QueryStatement.PROPERTY_MPR_MPR_WO_NO,
                //QueryStatement.PROPERTY_MPR_MPR_PROJECT_NAME
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

        private void updateAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvProdOfPO.Rows.Count <= 0) return;
            int rsl = dgvProdOfPO.CurrentRow.Index;

            int qty = CheckOrReturnNumber(dgvProdOfPO.Rows[rsl].Cells[12].Value.ToString()) > 0 
                        ? CheckOrReturnNumber(dgvProdOfPO.Rows[rsl].Cells[12].Value.ToString()) : 1;

            // Dictionary info prod
            var variable = new Dictionary<string, string>
            {
                {QueryStatement.PROPERTY_PROD_A, dgvProdOfPO.Rows[rsl].Cells[5].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_B, dgvProdOfPO.Rows[rsl].Cells[6].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_C, dgvProdOfPO.Rows[rsl].Cells[7].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_D, dgvProdOfPO.Rows[rsl].Cells[8].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_E, dgvProdOfPO.Rows[rsl].Cells[9].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_F, dgvProdOfPO.Rows[rsl].Cells[10].Value.ToString().Trim()},
                {QueryStatement.PROPERTY_PROD_G, dgvProdOfPO.Rows[rsl].Cells[11].Value.ToString().Trim()},

                {QueryStatement.QTY_PARA, qty.ToString() },
                {QueryStatement.PRICE_PARA,  dgvProdOfPO.Rows[rsl].Cells[13].Value.ToString().Trim()}, // Price
                {QueryStatement.NETCASH_PARA, dgvProdOfPO.Rows[rsl].Cells[14].Value.ToString().Trim()}, // Netcash
                {QueryStatement.TAXVALUE_PARA, dgvProdOfPO.Rows[rsl].Cells[17].Value.ToString().Trim()}, // Tax value
                {QueryStatement.PROPERTY_FORMULA_TEXT, dgvProdOfPO.Rows[rsl].Cells[18].Value.ToString().Trim() }, // Formula
                {QueryStatement.PROPERTY_COST_ID, dgvProdOfPO.Rows[rsl].Cells[19].Value.ToString().Trim() }, // CurrencyOption


                {QueryStatement.PROPERTY_PO_DETAIL_RECEVIE, dgvProdOfPO.Rows[rsl].Cells[15].Value.ToString().Trim()}, // Recevie
                {QueryStatement.PROPERTY_PO_DETAIL_REMARKS, dgvProdOfPO.Rows[rsl].Cells[16].Value.ToString().Trim()}, // Remark
            };

            var prod = new Products()
            {
                Id = Guid.Parse(dgvProdOfPO.Rows[rsl].Cells[0].Value.ToString().Trim()),
                Product_Name = dgvProdOfPO.Rows[rsl].Cells[1].Value.ToString().Trim(),
                Product_Des_2 = dgvProdOfPO.Rows[rsl].Cells[2].Value.ToString().Trim(),
                Product_Material_Code = dgvProdOfPO.Rows[rsl].Cells[4].Value.ToString().Trim(),
                A_Thinhness = dgvProdOfPO.Rows[rsl].Cells[6].Value.ToString().Trim(),
                B_Depth = dgvProdOfPO.Rows[rsl].Cells[6].Value.ToString().Trim(),
                C_Witdh = dgvProdOfPO.Rows[rsl].Cells[7].Value.ToString().Trim(),
                D_Web = dgvProdOfPO.Rows[rsl].Cells[8].Value.ToString().Trim(),
                E_Flag = dgvProdOfPO.Rows[rsl].Cells[9].Value.ToString().Trim(),
                F_Length = dgvProdOfPO.Rows[rsl].Cells[10].Value.ToString().Trim(),
                G_Weight = dgvProdOfPO.Rows[rsl].Cells[11].Value.ToString().Trim(),
            };

            frmAddPriceForProdPO frmAddPriceForProdPO = new frmAddPriceForProdPO(variable, prod, false);
            frmAddPriceForProdPO.ShowDialog();

            if (frmAddPriceForProdPO.Price == 0 || frmAddPriceForProdPO.NetCash == 0) { return; }

            dgvProdOfPO.Rows[rsl].Cells[13].Value = frmAddPriceForProdPO.Price;
            dgvProdOfPO.Rows[rsl].Cells[14].Value = frmAddPriceForProdPO.NetCash;
            dgvProdOfPO.Rows[rsl].Cells[15].Value = frmAddPriceForProdPO.Recevie;
            dgvProdOfPO.Rows[rsl].Cells[16].Value = frmAddPriceForProdPO.Remark;
            dgvProdOfPO.Rows[rsl].Cells[17].Value = frmAddPriceForProdPO.TaxValue;
            dgvProdOfPO.Rows[rsl].Cells[18].Value = frmAddPriceForProdPO.Formula;
            dgvProdOfPO.Rows[rsl].Cells[19].Value = frmAddPriceForProdPO.CurrencyOption;
            dgvProdOfPO.Rows[rsl].Cells[20].Value = frmAddPriceForProdPO.CostID;
            dgvProdOfPO.Rows[rsl].Cells[21].Value = frmAddPriceForProdPO.TaxID;

            this.CurrencyDefault = frmAddPriceForProdPO.CurrencyOption;

            UpdateFooter();
        }

        private async void cboProjectForAddPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_projectIsLoad) return;

            var projectId = Guid.Parse(cboProjectForAddPO.ComboBox.SelectedValue.ToString().Trim());
            await LoadMPRByProjectForCreatePO(projectId);
        }
    }
}
