using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.Infrastructor.Shared;
using StorageDLHI.Infrastructor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MenuGUI.MenuControl
{
    public partial class frmAddCost : KryptonForm
    {
        private int type = (int)TaxUnitCost.Cost;
        private bool status = true;
        private TaxUnitCostDto models = null;

        public frmAddCost(string title, TaxUnitCost taxUnitCost, bool status, TaxUnitCostDto taxUnitCostDto)
        {
            InitializeComponent();
            this.Text = title;
            this.status = status;
            this.type = (int)taxUnitCost;
            this.models = taxUnitCostDto;

            if (status)
            {

            }
            else
            {
                txtName.Text = this.models.Costs.Cost_Name;
                txtCode.Text = this.models.Costs.Currency_code;
                txtValue.Text = decimal.Parse(this.models.Costs.Currency_Value.ToString()).ToString("N2");
                txtCurrency.Text = this.models.Costs.Currency;
            }

        }

        private void frmAddCost_Load(object sender, EventArgs e)
        {
            DisableTabStopForNonInputs(this);
            //txtName.TabIndex = 20;
            //txtCode.TabIndex = 21;
            //txtValue.TabIndex = 22;
            //txtCurrency.TabIndex = 23;
        }

        private void DisableTabStopForNonInputs(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (!(ctrl is TextBox || ctrl is Button))
                {
                    ctrl.TabStop = false;
                }

                // Recursively apply to child controls like inside Panels
                if (ctrl.HasChildren)
                {
                    DisableTabStopForNonInputs(ctrl);
                }
            }
        }


        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (status)
            {
                Costs cost = new Costs() 
                {
                    Id = Guid.NewGuid(),
                    Cost_Name = txtName.Text.Trim(),
                    Currency_code = txtCode.Text.Trim(),
                    Currency_Value = decimal.Parse(txtValue.Text.Trim()),
                    Currency = txtCurrency.Text.Trim(),
                };

                if (await MaterialDAO.InsertCost(cost))
                {
                    MessageBoxHelper.ShowInfo("Add Cost success!");
                    LoggerConfig.Logger.Info($"Add Cost success by {ShareData.UserName}");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Add Cost fail!");
                    this.Close();
                }
            }
            else
            {
                Costs cost = new Costs()
                {
                    Id = this.models.Costs.Id,
                    Cost_Name = txtName.Text.Trim(),
                    Currency_code = txtCode.Text.Trim(),
                    Currency_Value = decimal.Parse(txtValue.Text.Trim()),
                    Currency = txtCurrency.Text.Trim(),
                };

                if (await MaterialDAO.UpdateCost(cost))
                {
                    MessageBoxHelper.ShowInfo("Update Cost success!");
                    LoggerConfig.Logger.Info($"Update Cost success by {ShareData.UserName}");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Update Cost fail!");
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
