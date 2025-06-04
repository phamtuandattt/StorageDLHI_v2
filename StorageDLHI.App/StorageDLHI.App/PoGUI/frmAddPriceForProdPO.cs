using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.MenuGUI.MenuControl;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.PoGUI
{
    public partial class frmAddPriceForProdPO : KryptonForm
    {
        public Int32 Price { get; set; }
        public double NetCash { get; set; }
        public string Recevie {  get; set; }
        public string Remark { get; set; }
        public string TaxValue { get; set; }
        public string Formula {  get; set; }
        
        private DataTable dtFormula = new DataTable();
        private DataTable dtFormulaPara = new DataTable();
        private string paraString = string.Empty;
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        private Products product = new Products();
        private DataTable dtProdInfo = new DataTable();
        private string QtyProd = "";
        private bool IsAdd = true;


        public frmAddPriceForProdPO()
        {
            InitializeComponent();

            // Get Dictionary when click product
        }

        public frmAddPriceForProdPO(Dictionary<string, string> variables, Products product, bool IsAdd)
        {
            InitializeComponent();

            // Get Dictionary when click product
            this.variables = variables;
            this.product = product;
            this.IsAdd = IsAdd;

            this.variables.TryGetValue(QueryStatement.QTY_PARA, out QtyProd);
            txtQty.Text = int.Parse(QtyProd).ToString("N0");

            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_ID);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_NAME);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_DES_2);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_MATERIAL_CODE);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_A);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_B);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_C);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_D);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_E);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_F);
            dtProdInfo.Columns.Add(QueryStatement.PROPERTY_PROD_G);
            dtProdInfo.Columns.Add("QTY", typeof(Int32)); // 11
            dtProdInfo.Columns.Add("PRICE", typeof(Int32));
            dtProdInfo.Columns.Add("TAX_VALUE");
            dtProdInfo.Columns.Add("NET_CASH", typeof(double));

            dgvProdInfo.DataSource = dtProdInfo;
        }


        private async void frmAddPriceForProdPO_Load(object sender, EventArgs e)
        {
            await LoadTaxs();
            await LoadFormula();

            if (this.IsAdd)
            {
                DataRow dataRow = dtProdInfo.NewRow();
                dataRow[0] = this.product.Id;
                dataRow[1] = this.product.Product_Name;
                dataRow[2] = this.product.Product_Des_2;
                dataRow[3] = this.product.Product_Material_Code;
                dataRow[4] = this.product.A_Thinhness;
                dataRow[5] = this.product.B_Depth;
                dataRow[6] = this.product.C_Witdh;
                dataRow[7] = this.product.D_Web;
                dataRow[8] = this.product.E_Flag;
                dataRow[9] = this.product.F_Length;
                dataRow[10] = this.product.G_Weight;
                dataRow[11] = QtyProd;
                dataRow[12] = 0;
                dataRow[13] = 0;
                dataRow[14] = 0;

                dtProdInfo.Rows.Add(dataRow);
            }
            else
            {
                this.variables.TryGetValue(QueryStatement.PRICE_PARA, out string price);
                this.variables.TryGetValue(QueryStatement.TAXVALUE_PARA, out string taxValue);
                this.variables.TryGetValue(QueryStatement.PROPERTY_FORMULA_TEXT, out string formula);
                this.variables.TryGetValue(QueryStatement.NETCASH_PARA, out string netCash);
                this.variables.TryGetValue(QueryStatement.PROPERTY_PO_DETAIL_RECEVIE, out string recevie);
                this.variables.TryGetValue(QueryStatement.PROPERTY_PO_DETAIL_REMARKS, out string remark);

                cboTax.SelectedIndex = cboTax.FindStringExact(taxValue);
                cboFormula.SelectedIndex = cboFormula.FindStringExact(formula);

                txtPrice.Value = Int32.Parse(price);
                txtRecevie.Text = recevie;
                txtRemark.Text = remark;

                DataRow dataRow = dtProdInfo.NewRow();
                dataRow[0] = this.product.Id;
                dataRow[1] = this.product.Product_Name;
                dataRow[2] = this.product.Product_Des_2;
                dataRow[3] = this.product.Product_Material_Code;
                dataRow[4] = this.product.A_Thinhness;
                dataRow[5] = this.product.B_Depth;
                dataRow[6] = this.product.C_Witdh;
                dataRow[7] = this.product.D_Web;
                dataRow[8] = this.product.E_Flag;
                dataRow[9] = this.product.F_Length;
                dataRow[10] = this.product.G_Weight;
                dataRow[11] = QtyProd;
                dataRow[12] = price;
                dataRow[13] = taxValue;
                dataRow[14] = netCash;

                dtProdInfo.Rows.Add(dataRow);
            }
        }

        public async Task LoadTaxs()
        {
            var dtTax = new DataTable();
            if (!CacheManager.Exists(CacheKeys.TAX_DATATABLE_ALLTAX))
            {
                dtTax = await MaterialDAO.GetTaxCustoms();
                CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, dtTax);

            }
            else
            {
                dtTax = CacheManager.Get<DataTable>(CacheKeys.TAX_DATATABLE_ALLTAX);
            }
            
            cboTax.DataSource = dtTax;
            cboTax.ValueMember = QueryStatement.PROPERTY_TAX_ID;
            cboTax.DisplayMember = QueryStatement.PROPERTY_TAX_CUSTOM_VALUE;
        }

        public async Task LoadFormula()
        {
            if (!CacheManager.Exists(CacheKeys.FORMULA_DATATABLE_ALLFORMULA))
            {
                dtFormula = await MaterialDAO.GetFormulaParas();
                CacheManager.Add(CacheKeys.FORMULA_DATATABLE_ALLFORMULA, dtFormula);

            }
            else
            {
                dtFormula = CacheManager.Get<DataTable>(CacheKeys.FORMULA_DATATABLE_ALLFORMULA);
            }

            cboFormula.ValueMember = QueryStatement.PROPERTY_FORMULA_PARAS;
            cboFormula.DisplayMember = QueryStatement.PROPERTY_FORMULA_TEXT;
            cboFormula.DataSource = dtFormula;
        }

        private void btnAddProdIntoMpr_Click(object sender, EventArgs e)
        {
            this.Price = (Int32)txtPrice.Value;
            this.Recevie = txtRecevie.Text.Trim();
            this.Remark = txtRemark.Text.Trim();
            this.TaxValue = cboTax.Text.Trim();
            this.Formula = cboFormula.Text.Trim();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Price = 0;
            this.Close();
        }

        private async void btnAddTax_Click(object sender, EventArgs e)
        {
            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.TAX_ADD_TITLE, Enums.TaxUnitCost.Tax, true, null);
            frm.ShowDialog();
            CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX, await MaterialDAO.GetTaxs());
            frm.Close();
        }

        private void btnAddCash_Click(object sender, EventArgs e)
        {

        }

        private void cboFormula_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            // Replace PRICE value
            variables[QueryStatement.PRICE_PARA] = txtPrice.Value.ToString().Trim();
            
            var formula = cboFormula.SelectedValue.ToString().Split('|');

            var varis = new Dictionary<string, double>();
            foreach (var item in formula)
            {
                variables.TryGetValue(item, out string value);
                varis.Add(item, double.Parse(value));
            }
            
            // Calcute amount
            double amount = Common.Common.EvaluateExpression(cboFormula.Text.Trim(), varis);

            var taxArr = cboTax.Text.Trim().Split('~');

            NetCash = amount * float.Parse(taxArr[1]);

            dgvProdInfo.Rows[0].Cells[11].Value = QtyProd;
            dgvProdInfo.Rows[0].Cells[12].Value = txtPrice.Value;
            dgvProdInfo.Rows[0].Cells[13].Value = cboTax.Text.Trim();
            dgvProdInfo.Rows[0].Cells[14].Value = NetCash;

        }

        private void dgvProdInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvProdInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProdInfo.Columns["QTY"].DefaultCellStyle.Format = "N0";
            dgvProdInfo.Columns["PRICE"].DefaultCellStyle.Format = "N0";
            dgvProdInfo.Columns["NET_CASH"].DefaultCellStyle.Format = "N3";
        }
    }
}
