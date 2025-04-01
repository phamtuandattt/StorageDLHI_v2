using ComponentFactory.Krypton.Toolkit;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
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
                        if (MaterialDAO.InsertOrigin(model))
                        {
                            KryptonMessageBox.Show("Add Origins success!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (MaterialDAO.InsertMaterialType(typeModel))
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
                        if (MaterialDAO.InsertMaterialStandards(standModel))
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
                        if (MaterialDAO.UpdateOrigin(model))
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
                        if (MaterialDAO.UpdateMaterialType(typeModel))
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
                        if (MaterialDAO.UpdateMaterialStandard(standModel))
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
    }
}
