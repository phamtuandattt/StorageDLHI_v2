﻿using ComponentFactory.Krypton.Toolkit;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MenuGUI.MenuControl
{
    public partial class frmAddMaterials : KryptonForm
    {
        private int type = (int)Materials.Types;
        private bool status = true;
        private MaterialCommonDto models = null;


        public frmAddMaterials()
        {
            InitializeComponent();
        }

        public frmAddMaterials(string title, Materials materials, bool status, MaterialCommonDto models) // true: add, false: edit
        {
            InitializeComponent();
            this.Text = title;
            type = (int)materials;
            this.status = status;           
            this.models = models;

            if (status)
            {
                txtCode.Focus();
                txtCode.TabIndex = 1;
                txtDes.TabIndex = 2;
                btnCancel.TabIndex = 3;
                btnSave.TabIndex = 4;
            } 
            else
            {
                switch (type)
                {
                    case 1:
                        txtCode.Focus();
                        txtCode.Text = this.models.Origins.Origin_Code;
                        txtDes.Text = this.models.Origins.Origin_Des;
                        break;
                    case 2:
                        txtCode.Focus();
                        txtCode.Text = this.models.MaterialType.Type_Code;
                        txtDes.Text = this.models.MaterialType.Type_Des;
                        break;
                    case 3:
                        txtCode.Focus();
                        txtCode.Text = this.models.MaterialStandard.Standard_Code;
                        txtDes.Text = this.models.MaterialStandard.Standard_Des;
                        break;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text.Trim())
                || string.IsNullOrEmpty(txtDes.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill in the information completely !");
                return;
            }

            if (status)
            {
                switch (type)
                {
                    case 1:
                        Origins model = new Origins()
                        {
                            Id = Guid.NewGuid(),
                            Origin_Code = txtCode.Text.Trim().ToUpper(),
                            Origin_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.InsertOrigin(model))
                        {
                            MessageBoxHelper.ShowInfo("Add Origins success!");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Origins fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }

                        break;

                    case 2:
                        Material_Types typeModel = new Material_Types()
                        {
                            Id = Guid.NewGuid(),
                            Type_Code = txtCode.Text.Trim().ToUpper(),
                            Type_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.InsertMaterialType(typeModel))
                        {
                            KryptonMessageBox.Show("Add Types success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Types fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        break;

                    case 3:
                        Material_Standards standModel = new Material_Standards()
                        {
                            Id = Guid.NewGuid(),
                            Standard_Code = txtCode.Text.Trim().ToUpper(),
                            Standard_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.InsertMaterialStandards(standModel))
                        {
                            KryptonMessageBox.Show("Add Standard success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Add Standard fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        Origins model = new Origins()
                        {
                            Id = this.models.Origins.Id,
                            Origin_Code = txtCode.Text.Trim().ToUpper(),
                            Origin_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.UpdateOrigin(model))
                        {
                            KryptonMessageBox.Show("Update Origins success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Origins by {ShareData.UserName} success!");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Origins fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }

                        break;

                    case 2:
                        Material_Types typeModel = new Material_Types()
                        {
                            Id = this.models.MaterialType.Id,
                            Type_Code = txtCode.Text.Trim().ToUpper(),
                            Type_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.UpdateMaterialType(typeModel))
                        {
                            KryptonMessageBox.Show("Update Types success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Material Types by {ShareData.UserName} success!");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Types fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        break;

                    case 3:
                        Material_Standards standModel = new Material_Standards()
                        {
                            Id = this.models.MaterialStandard.Id,
                            Standard_Code = txtCode.Text.Trim().ToUpper(),
                            Standard_Des = txtDes.Text.Trim(),
                        };
                        if (await MaterialDAO.UpdateMaterialStandard(standModel))
                        {
                            KryptonMessageBox.Show("Update Standard success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoggerConfig.Logger.Info($"Update Material Standard by {ShareData.UserName} success!");
                            this.Close();
                        }
                        else
                        {
                            KryptonMessageBox.Show("Update Standard fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        break;
                }
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtCode.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtCode.Text != cleaned)
            {
                int pos = txtCode.SelectionStart - 1;
                txtCode.Text = cleaned;
                txtCode.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtDes_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtDes.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtDes.Text != cleaned)
            {
                int pos = txtDes.SelectionStart - 1;
                txtDes.Text = cleaned;
                txtDes.SelectionStart = Math.Max(pos, 0);
            }
        }
    }
}
