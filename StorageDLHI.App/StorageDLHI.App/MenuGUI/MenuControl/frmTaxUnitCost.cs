using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.Enums;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.Infrastructor;
using StorageDLHI.Infrastructor.Shared;
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
    public partial class frmTaxUnitCost : KryptonForm
    {
        private int type = (int)TaxUnitCost.Cost;
        private bool status = true;
        private TaxUnitCostDto models = null;

        public frmTaxUnitCost()
        {
            InitializeComponent();
        }

        public frmTaxUnitCost(string title, TaxUnitCost taxUnitCost, bool status, TaxUnitCostDto taxUnitCostDto)
        {
            InitializeComponent();
            this.Text = title;
            this.status = status;
            this.type = (int)taxUnitCost;
            this.models = taxUnitCostDto;

            if (status)
            {
                txtName.Focus();
                switch (type)
                {
                    case 1:
                        lblName.Text = "Tax percent:"; break;
                    case 2:
                        lblName.Text = "Code:"; break;
                    case 3:
                        lblName.Text = "Name:"; break;
                }
            }
            else
            {
                switch (type)
                {
                    case 1:
                        txtName.Focus();
                        txtName.Text = this.models.Taxs.Tax_Percent;
                        lblName.Text = "Tax percent:"; 
                        break;
                    case 2:
                        txtName.Focus();
                        txtName.Text = this.models.Units.Unit_Code;
                        lblName.Text = "Code:"; 
                        break;
                    case 3:
                        txtName.Focus();
                        txtName.Text = this.models.Costs.Cost_Name;
                        lblName.Text = "Name:"; 
                        break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (status)
            {
                switch (type)
                {
                    case 1:
                        Taxs taxs = new Taxs() { Id = Guid.NewGuid(), Tax_Percent = txtName.Text.Trim() };
                        if (MaterialDAO.InsertTax(taxs))
                        {
                            MessageBoxHelper.ShowInfo("Add Tax success!");
                            LoggerConfig.Logger.Info($"Add Tax success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowWarning("Add Tax fail!");
                            this.Close();
                        }
                        break;
                    case 2:
                        Units unit = new Units() { Id = Guid.NewGuid(), Unit_Code = txtName.Text.Trim() };
                        if (MaterialDAO.InsertUnit(unit))
                        {
                            MessageBoxHelper.ShowInfo("Add Unit success!");
                            LoggerConfig.Logger.Info($"Add Unit success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowWarning("Add Unit fail!");
                            this.Close();
                        }
                        break;
                    case 3:
                        Costs cost = new Costs() { Id = Guid.NewGuid(), Cost_Name = txtName.Text.Trim() };
                        if (MaterialDAO.InsertCost(cost))
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
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 1:
                        Taxs taxs = new Taxs() { Id = this.models.Taxs.Id, Tax_Percent = txtName.Text.Trim() };
                        if (MaterialDAO.UpdateTax(taxs))
                        {
                            MessageBoxHelper.ShowInfo("Update Tax success!");
                            LoggerConfig.Logger.Info($"Update Tax success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowWarning("Update Tax fail!");
                            this.Close();
                        }
                        break;
                    case 2:
                        Units unit = new Units() { Id = this.models.Units.Id, Unit_Code = txtName.Text.Trim() };
                        if (MaterialDAO.UpdateUnit(unit))
                        {
                            MessageBoxHelper.ShowInfo("Update Unit success!");
                            LoggerConfig.Logger.Info($"Update Unit success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            MessageBoxHelper.ShowWarning("Update Unit fail!");
                            this.Close();
                        }
                        break;
                    case 3:
                        Costs cost = new Costs() { Id = this.models.Costs.Id, Cost_Name = txtName.Text.Trim() };
                        if (MaterialDAO.UpdateCost(cost))
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
                        break;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
