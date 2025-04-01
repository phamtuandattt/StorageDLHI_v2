﻿using ComponentFactory.Krypton.Toolkit;
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
                            KryptonMessageBox.Show("Add Tax success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Add Tax success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Tax fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        break;
                    case 2:
                        Units unit = new Units() { Id = Guid.NewGuid(), Unit_Code = txtName.Text.Trim() };
                        if (MaterialDAO.InsertUnit(unit))
                        {
                            KryptonMessageBox.Show("Add Unit success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Add Unit success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Unit fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        break;
                    case 3:
                        Costs cost = new Costs() { Id = Guid.NewGuid(), Cost_Name = txtName.Text.Trim() };
                        if (MaterialDAO.InsertCost(cost))
                        {
                            KryptonMessageBox.Show("Add Cost success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Add Cost success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Cost fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            KryptonMessageBox.Show("Update Tax success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Tax success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Tax fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        break;
                    case 2:
                        Units unit = new Units() { Id = this.models.Units.Id, Unit_Code = txtName.Text.Trim() };
                        if (MaterialDAO.UpdateUnit(unit))
                        {
                            KryptonMessageBox.Show("Update Unit success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Unit success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Unit fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        break;
                    case 3:
                        Costs cost = new Costs() { Id = this.models.Costs.Id, Cost_Name = txtName.Text.Trim() };
                        if (MaterialDAO.UpdateCost(cost))
                        {
                            KryptonMessageBox.Show("Update Cost success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Cost success by {ShareData.UserName}");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Cost fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
