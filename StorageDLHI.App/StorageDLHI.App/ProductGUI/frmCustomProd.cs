using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StorageDLHI.App.ProductGUI
{
    public partial class frmCustomProd : KryptonForm
    {
        private string path = string.Empty;
        private bool status = false;
        private Products pModel = null;

        public frmCustomProd()
        {
            InitializeComponent();
            LoadData();


            //this.pModel = await ProductDAO.GetProduct(Guid.Parse("1730930D-FD63-46C5-9B63-698B2AF63C22"));

            //txtProdCode.Text = this.pModel.Product_Code.Trim();
            //txtDes2.Text = this.pModel.Product_Des_2.Trim();
            //txtProdName.Text = this.pModel.Product_Name.Trim();
            //cboOrigin.SelectedValue = this.pModel.Origin_Id;
            //cboType.SelectedValue = this.pModel.M_Type_Id;
            //cboStandard.SelectedValue = this.pModel.Stand_Id;
            //txtThinh.Text = this.pModel.A_Thinhness.Trim();
            //txtDep.Text = this.pModel.B_Depth.Trim();
            //txtWidth.Text = this.pModel.C_Witdh.Trim();
            //txtWeb.Text = this.pModel.D_Web.Trim();
            //txtFlag.Text = this.pModel.E_Flag.Trim();
            //txtLength.Text = this.pModel.F_Length.Trim();
            //txtWeigth.Text = this.pModel.G_Weight.Trim();
            //txtUsageNote.Text = this.pModel.Used_Note.Trim();
            //cboUnit.SelectedValue = this.pModel.UnitId;
            //picItem.Image = this.pModel.Image.Length == 100 ? picItem.InitialImage : Image.FromStream(new MemoryStream(this.pModel.Image));
            //path = this.pModel.PictureLink;
        }

        public frmCustomProd(string title, bool status, Products pModel) // true is ADD || UPDATE
        {
            InitializeComponent();
            LoadData();
            this.Text = title;
            this.pModel = pModel;
            this.status = status;

            if (!status)
            {
                txtProdCode.Text = this.pModel.Product_Code.Trim();
                txtDes2.Text = this.pModel.Product_Des_2.Trim();
                txtProdName.Text = this.pModel.Product_Name.Trim();
                cboOrigin.SelectedValue = this.pModel.Origin_Id;
                cboType.SelectedValue = this.pModel.Type_Id;
                cboStandard.SelectedValue = this.pModel.Stand_Id;
                txtThinh.Text = this.pModel.A_Thinhness.Trim();
                txtDep.Text = this.pModel.B_Depth.Trim();
                txtWidth.Text = this.pModel.C_Witdh.Trim();
                txtWeb.Text = this.pModel.D_Web.Trim();
                txtFlag.Text = this.pModel.E_Flag.Trim();
                txtLength.Text = this.pModel.F_Length.Trim();
                txtWeigth.Text = this.pModel.G_Weight.Trim();
                txtUsageNote.Text = this.pModel.Used_Note.Trim();
                cboUnit.SelectedValue = this.pModel.UnitId;
                picItem.Image = this.pModel.Image.Length == 100 ? picItem.InitialImage : Image.FromStream(new MemoryStream(this.pModel.Image));
                path = this.pModel.PictureLink;
            }
        }

        private async void LoadData()
        {
            txtProdCode.Focus();

            var dtOrigins = await MaterialDAO.GetOriginForCombobox();
            LoadDataCombox(cboOrigin, dtOrigins);

            var dtMTypes = await MaterialDAO.GetMTypeForCombobox();
            LoadDataCombox(cboType, dtMTypes);

            var dtStand = await MaterialDAO.GetStandForCombobox();
            LoadDataCombox(cboStandard, dtStand);

            var dtUnits = await MaterialDAO.GetUnits();
            cboUnit.DataSource = dtUnits;
            cboUnit.DisplayMember = QueryStatement.PROPERTY_UNIT_CODE;
            cboUnit.ValueMember = QueryStatement.PROPERTY_UNIT_ID;
            if (dtUnits.Rows.Count > 0)
            {
                cboUnit.SelectedIndex = 0;
            }
        }

        private void LoadDataCombox(KryptonComboBox comboBox, DataTable dataTable)
        {
            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY;
            comboBox.ValueMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE;
            if (dataTable.Rows.Count > 0)
            {
              comboBox.SelectedIndex = 0;
            }
        }

        private void picItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                picItem.Image = new Bitmap(open.FileName);
                path = open.FileName;
            }
        }

        private void btnGenerateCode_Click(object sender, EventArgs e)
        {
            // origin - type - standard - size - length

            // size: thinh || Depth - width - web -  flag

            if (string.IsNullOrEmpty(txtThinh.Text.Trim()) && string.IsNullOrEmpty(txtDep.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Thinh or Depth !");
                txtThinh.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtWidth.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Width !");
                txtWidth.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtWeb.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Flag !");
                txtWeb.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFlag.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill Flag !");
                txtFlag.Focus();
                return;
            }

            var H = !string.IsNullOrEmpty(txtThinh.Text.Trim()) ? txtThinh.Text.Trim() : txtDep.Text.Trim();
            var B = txtWidth.Text.Trim();
            var T1 = txtWeb.Text.Trim();
            var T2 = txtFlag.Text.Trim();

            var oriInfos = cboOrigin.Text.ToString().Split('|');
            var mTypeInfo = cboType.Text.ToString().Split('|');
            var standInfo = cboStandard.Text.ToString().Split('|');

            var ori_code = oriInfos[1].Trim();
            var m_type_code = mTypeInfo[1].Trim();
            var stand_code = standInfo[1].Trim();
            var size = H + B + T1 + T2;
            var length = txtLength.Text.Trim();

            var prod_code = string.Concat(ori_code, m_type_code, stand_code, size, "_", length);
            txtProdCode.Text = prod_code;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProdCode.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please fill in the information completely !");
                return;
            }

            if (status)
            {
                var oriInfos = cboOrigin.Text.ToString().Split('|');
                var mTypeInfo = cboType.Text.ToString().Split('|');
                var standInfo = cboStandard.Text.ToString().Split('|');

                Products prod = new Products()
                {
                    Id = Guid.NewGuid(),
                    Product_Name = txtProdName.Text.Trim(),
                    Product_Des_2 = txtDes2.Text.Trim().ToUpper(),
                    Product_Code = txtProdCode.Text.Trim().ToUpper(),
                    Product_Material_Code = standInfo[0].Trim(), //cboStandard.Text.Trim(),
                    PictureLink = path,
                    Picture = path,
                    A_Thinhness = txtThinh.Text.Trim(),
                    B_Depth = txtDep.Text.Trim(),
                    C_Witdh = txtWidth.Text.Trim(),
                    D_Web = txtWeb.Text.Trim(),
                    E_Flag = txtFlag.Text.Trim(),
                    F_Length = txtLength.Text.Trim(),
                    G_Weight = txtWeigth.Text.Trim(),
                    Used_Note = txtUsageNote.Text.Trim(),
                    UnitId = Guid.Parse(cboUnit.SelectedValue.ToString()),
                    Origin_Id = Guid.Parse(cboOrigin.SelectedValue.ToString()),
                    Type_Id = Guid.Parse(cboType.SelectedValue.ToString()),
                    Stand_Id = Guid.Parse(cboStandard.SelectedValue.ToString()),
                };

                if (!string.IsNullOrEmpty(path))
                {
                    if (await ShowDialogManager.WithLoader(() => ProductDAO.Insert(prod)))
                    {
                        MessageBoxHelper.ShowInfo("Add product success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Add product fail !");
                    }
                }
                else
                {
                    if (await ShowDialogManager.WithLoader(() => ProductDAO.InsertNoImage(prod)))
                    {
                        MessageBoxHelper.ShowInfo("Add product success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Add product fail !");
                    }
                }
            }
            else
            {
                var oriInfos = cboOrigin.Text.ToString().Split('|');
                var mTypeInfo = cboType.Text.ToString().Split('|');
                var standInfo = cboStandard.Text.ToString().Split('|');

                Products prod = new Products()
                {
                    Id = this.pModel.Id,
                    Product_Name = txtProdName.Text.Trim(),
                    Product_Des_2 = txtDes2.Text.Trim().ToUpper(),
                    Product_Code = txtProdCode.Text.Trim().ToUpper(),
                    Product_Material_Code = standInfo[1].Trim(), // cboStandard.Text.Trim(),
                    PictureLink = path,
                    Picture = path,
                    A_Thinhness = txtThinh.Text.Trim(),
                    B_Depth = txtDep.Text.Trim(),
                    C_Witdh = txtWidth.Text.Trim(),
                    D_Web = txtWeb.Text.Trim(),
                    E_Flag = txtFlag.Text.Trim(),
                    F_Length = txtLength.Text.Trim(),
                    G_Weight = txtWeigth.Text.Trim(),
                    Used_Note = txtUsageNote.Text.Trim(),
                    UnitId = Guid.Parse(cboUnit.SelectedValue.ToString()),
                    Origin_Id = Guid.Parse(cboOrigin.SelectedValue.ToString()),
                    Type_Id = Guid.Parse(cboType.SelectedValue.ToString()),
                    Stand_Id = Guid.Parse(cboStandard.SelectedValue.ToString()),
                };

                if (!string.IsNullOrEmpty(path))
                {
                    if (await ShowDialogManager.WithLoader(() => ProductDAO.Update(prod)))
                    {
                        MessageBoxHelper.ShowInfo("Update product success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Update product fail !");
                    }
                }
                else
                {
                    if (await ShowDialogManager.WithLoader(() => ProductDAO.InsertNoImage(prod)))
                    {
                        MessageBoxHelper.ShowInfo("Update product success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Update product fail !");
                    }
                }
            }

        }

        private void btnClearContent_Click(object sender, EventArgs e)
        {
            txtProdCode.Clear();
            txtProdName.Clear();
            txtDes2.Clear();
            txtDep.Clear();
            txtWidth.Clear();
            txtThinh.Clear();
            txtWeb.Clear();
            txtWeigth.Clear();
            txtFlag.Clear();
            txtLength.Clear();
            txtUsageNote.Clear();
        }

        private void txtDes2_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtDes2.Text, Infrastructor.Commons.Common.REGEX_VALID_CODE, "");
            if (txtDes2.Text != cleaned)
            {
                int pos = txtDes2.SelectionStart - 1;
                txtDes2.Text = cleaned;
                txtDes2.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtProdName_TextChanged(object sender, EventArgs e)
        {
            //string cleaned = Regex.Replace(txtProdName.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            //if (txtProdName.Text != cleaned)
            //{
            //    int pos = txtProdName.SelectionStart - 1;
            //    txtProdName.Text = cleaned;
            //    txtProdName.SelectionStart = Math.Max(pos, 0);
            //}
        }

        private void txtThinh_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtThinh.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtThinh.Text != cleaned)
            {
                int pos = txtThinh.SelectionStart - 1;
                txtThinh.Text = cleaned;
                txtThinh.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtDep_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtDep.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtDep.Text != cleaned)
            {
                int pos = txtDep.SelectionStart - 1;
                txtDep.Text = cleaned;
                txtDep.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWidth.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtWidth.Text != cleaned)
            {
                int pos = txtWidth.SelectionStart - 1;
                txtWidth.Text = cleaned;
                txtWidth.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtWeb_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWeb.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtWeb.Text != cleaned)
            {
                int pos = txtWeb.SelectionStart - 1;
                txtWeb.Text = cleaned;
                txtWeb.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtFlag_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtFlag.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtFlag.Text != cleaned)
            {
                int pos = txtFlag.SelectionStart - 1;
                txtFlag.Text = cleaned;
                txtFlag.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtLength.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtLength.Text != cleaned)
            {
                int pos = txtLength.SelectionStart - 1;
                txtLength.Text = cleaned;
                txtLength.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtWeigth_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtWeigth.Text, Infrastructor.Commons.Common.REGEX_VALID_DIGIT, "");
            if (txtWeigth.Text != cleaned)
            {
                int pos = txtWeigth.SelectionStart - 1;
                txtWeigth.Text = cleaned;
                txtWeigth.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtUsageNote_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtUsageNote.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtUsageNote.Text != cleaned)
            {
                int pos = txtUsageNote.SelectionStart - 1;
                txtUsageNote.Text = cleaned;
                txtUsageNote.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void cboOrigin_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboType_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboStandard_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboUnit_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }
    }
}
