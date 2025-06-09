using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.Enums;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StorageDLHI.App.PoGUI
{
    public partial class frmAddPriceForProdPO : KryptonForm
    {
        public Int32 Price { get; set; }
        public decimal NetCash { get; set; }
        public string Recevie {  get; set; }
        public string Remark { get; set; }
        public string TaxValue { get; set; }
        public string Formula {  get; set; }
        public decimal ExchangeRate {  get; set; }
        public string Currency { get; set; }
        public string CurrencyOption  { get; set; }
        public Guid CostID { get; set; }
        public Guid TaxID { get; set; }
        
        private DataTable dtFormula = new DataTable();
        private DataTable dtFormulaPara = new DataTable();
        private DataTable dtCost = new DataTable();
        private string paraString = string.Empty;
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        private Products product = new Products();
        private DataTable dtProdInfo = new DataTable();
        private string QtyProd = "";
        private bool IsAdd = true;

        private Dictionary<Guid, string> cboCostDataSrouce = new Dictionary<Guid, string>();
        private Dictionary<string, string> exchangeRate = new Dictionary<string, string>();

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
            await LoadCost();

            if (this.IsAdd)
            {
                this.variables.TryGetValue(QueryStatement.PROPERTY_CURRENCY, out string currencyHint);
                if (!string.IsNullOrEmpty(currencyHint))
                {
                    cboCost.SelectedIndex = cboCost.FindStringExact(currencyHint);
                }

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
                this.variables.TryGetValue(QueryStatement.PROPERTY_COST_ID, out string costOption);
                this.variables.TryGetValue(QueryStatement.PROPERTY_PO_DETAIL_RECEVIE, out string recevie);
                this.variables.TryGetValue(QueryStatement.PROPERTY_PO_DETAIL_REMARKS, out string remark);

                if (!string.IsNullOrEmpty(taxValue))
                {
                    cboTax.SelectedIndex = cboTax.FindStringExact(taxValue);
                }
                cboFormula.SelectedIndex = cboFormula.FindStringExact(formula);
                cboCost.SelectedIndex = cboCost.FindStringExact(costOption);

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
            if (!CacheManager.Exists(CacheKeys.TAX_DATATABLE_ALLTAX_CUSTOM))
            {
                dtTax = await MaterialDAO.GetTaxCustoms();
                CacheManager.Add(CacheKeys.TAX_DATATABLE_ALLTAX_CUSTOM, dtTax);

            }
            else
            {
                dtTax = CacheManager.Get<DataTable>(CacheKeys.TAX_DATATABLE_ALLTAX_CUSTOM);
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

        public async Task LoadCost()
        {
            if (!CacheManager.Exists(CacheKeys.COST_DATATABLE_ALLCOST))
            {
                dtCost = await MaterialDAO.GetCosts();
                CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, dtCost);

            }
            else
            {
                dtCost = CacheManager.Get<DataTable>(CacheKeys.COST_DATATABLE_ALLCOST);
            }

            // Create Dictionary For display combobox
            foreach (DataRow row in dtCost.Rows)
            {
                cboCostDataSrouce.Add(Guid.Parse(row[QueryStatement.PROPERTY_COST_ID].ToString()), row[QueryStatement.PROPERTY_COST_NAME].ToString());
                var display = row[QueryStatement.PROPERTY_CURRENCY] + " - " + row[QueryStatement.PROPERTY_CURRENCY_CODE] + " - " + row[QueryStatement.PROPERTY_CURRENCY_VALUE];
                var member = row[QueryStatement.PROPERTY_COST_ID].ToString();
                exchangeRate.Add(member, display);
            }

            cboCost.DataSource = new BindingSource(cboCostDataSrouce, null);
            cboCost.DisplayMember = "Value";
            cboCost.ValueMember = "Key";
        }

        private void btnAddProdIntoMpr_Click(object sender, EventArgs e)
        {
            this.Price = (Int32)txtPrice.Value;
            this.Recevie = txtRecevie.Text.Trim();
            this.Remark = txtRemark.Text.Trim();
            this.Formula = cboFormula.Text.Trim();

            if (txtCurrencyCode.Text.Trim().Equals("VND"))
            {
                this.TaxValue = cboTax.Text.Trim();
            }
            else
            {
                this.TaxValue = "";
            }

            var costValue = ((KeyValuePair<Guid, string>)cboCost.SelectedItem).Value.ToString();
            var costID = ((KeyValuePair<Guid, string>)cboCost.SelectedItem).Key.ToString();
            this.CurrencyOption = costValue;
            this.CostID = Guid.Parse(costID);
            this.TaxID = Guid.Parse(cboTax.SelectedValue.ToString());
            this.ExchangeRate = decimal.Parse(txtExchangeRate.Text.Trim());
            this.Currency = txtCurrencyCode.Text.Trim();

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


            if (txtCurrencyCode.Text.Trim().Equals("VND"))
            {
                NetCash = decimal.Parse(amount.ToString()) * decimal.Parse(taxArr[1]);
                dgvProdInfo.Rows[0].Cells[13].Value = cboTax.Text.Trim();
            }
            else
            {
                NetCash = ConvertVNDToExchangeRate(decimal.Parse(amount.ToString()), decimal.Parse(txtExchangeRate.Text.Trim()));
                dgvProdInfo.Rows[0].Cells[13].Value = "";
            }

            dgvProdInfo.Rows[0].Cells[11].Value = QtyProd;
            dgvProdInfo.Rows[0].Cells[12].Value = txtPrice.Value;
            dgvProdInfo.Rows[0].Cells[14].Value = NetCash;

            btnSave.Enabled = true;
        }

        private void dgvProdInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void dgvProdInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProdInfo.Columns["QTY"].DefaultCellStyle.Format = "N0";
            dgvProdInfo.Columns["PRICE"].DefaultCellStyle.Format = "N0";
            dgvProdInfo.Columns["NET_CASH"].DefaultCellStyle.Format = "N2";
        }

        private void cboCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCost.Items.Count < 0) return;
            var key = ((KeyValuePair<Guid, string>)cboCost.SelectedItem).Key.ToString();
            this.exchangeRate.TryGetValue(key, out string currency);

            var currencyArr = currency.Split('-');
            txtCurrencyCode.Text = currencyArr[1];
            txtExchangeRate.Text = decimal.Parse(currencyArr[2].ToString()).ToString("N2");
        }

        private decimal ConvertVNDToExchangeRate(decimal amountVND, decimal amountExRate)
        {
            decimal convertedAmount = amountVND / amountExRate;
            return convertedAmount; // Return the converted amount
        }

        private void cboTax_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private async void txtExchangeRate_MouseClick(object sender, MouseEventArgs e)
        {
            var costID = ((KeyValuePair<Guid, string>)cboCost.SelectedItem).Key.ToString();
            var costValue = ((KeyValuePair<Guid, string>)cboCost.SelectedItem).Value.ToString();
            this.exchangeRate.TryGetValue(costID, out string currency);
            
            Costs costs = new Costs()
            {
                Id = Guid.Parse(costID),
                Cost_Name = costValue,
                Currency_code = currency,
                Currency_Value = decimal.Parse(txtExchangeRate.Text.Trim())
            };

            TaxUnitCostDto dtos = new TaxUnitCostDto()
            {
                Costs = costs,
            };

            frmTaxUnitCost frm = new frmTaxUnitCost(TitleManager.COST_UPDATE_TITLE, TaxUnitCost.Cost, false, dtos);
            frm.ShowDialog();

            if (frm.CostValue > 0)
            {
                txtExchangeRate.Text = frm.CostValue.ToString("N2");
                dtCost = await MaterialDAO.GetCosts();
                CacheManager.Add(CacheKeys.COST_DATATABLE_ALLCOST, dtCost);
                this.ExchangeRate = decimal.Parse(txtExchangeRate.Text.Trim());
                this.Currency = txtCurrencyCode.Text.Trim();
                this.exchangeRate[costID] = "" + " - " + txtCurrencyCode.Text.Trim() + " - " + frm.CostValue.ToString();
            }
        }
    }
}
