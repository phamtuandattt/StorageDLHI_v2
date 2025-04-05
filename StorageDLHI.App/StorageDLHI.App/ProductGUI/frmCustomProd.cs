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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ProductGUI
{
    public partial class frmCustomProd : KryptonForm
    {
        private string path = string.Empty;
        public frmCustomProd()
        {
            InitializeComponent();
            LoadData();


            var modelT = ProductDAO.GetProduct(Guid.Parse("6165C282-C47A-4423-A580-126A023AF473"));
            picItem.Image = modelT.Image.Length == 100 ? picItem.InitialImage : Image.FromStream(new MemoryStream(modelT.Image));

        }

        private void LoadData()
        {
            txtProdCode.Focus();

            var dtOrigins = MaterialDAO.GetOrigins();
            LoadDataCombox(cboOrigin, dtOrigins, QueryStatement.PROPERTY_ORIGIN_NAME, QueryStatement.PROPERTY_ORIGIN_CODE);

            var dtMTypes = MaterialDAO.GetMaterialTypes();
            LoadDataCombox(cboType, dtMTypes, QueryStatement.PROPERTY_M_TYPE_DES, QueryStatement.PROPERTY_M_TYPE_CODE);

            var dtStand = MaterialDAO.GetMaterialStandards();
            LoadDataCombox(cboStandard, dtStand, QueryStatement.PROPERTY_M_STANDARD_DES, QueryStatement.PROPERTY_M_STANDARD_CODE);

            var dtUnits = MaterialDAO.GetUnits();
            LoadDataCombox(cboUnit, dtUnits, QueryStatement.PROPERTY_UNIT_CODE, QueryStatement.PROPERTY_UNIT_ID);
        }

        private void LoadDataCombox(KryptonComboBox comboBox, DataTable dataTable, string displayMember, string valueMember)
        {
            comboBox.DataSource = dataTable;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            if (dataTable != null)
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

            var ori_code = cboOrigin.SelectedValue.ToString().Trim();
            var m_type_code = cboType.SelectedValue.ToString().Trim();
            var stand_code = cboStandard.SelectedValue.ToString().Trim();
            var size = H + B + T1 + T2;
            var length = txtLength.Text.Trim();

            var prod_code = string.Concat(ori_code, m_type_code, stand_code, size, "_", length);
            txtProdCode.Text = prod_code;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Products prod = new Products()
            {
                Id = Guid.NewGuid(),
                Product_Name = txtProdName.Text.Trim(),
                Product_Des_2 = txtDes2.Text.Trim().ToUpper(),
                Product_Code = txtProdCode.Text.Trim().ToUpper(),
                Product_Material_Code = cboStandard.Text.Trim(),
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

            };
            if (ProductDAO.Insert(prod))
            {
                MessageBoxHelper.ShowInfo("Add product success !");
            }
            else
            {
                MessageBoxHelper.ShowWarning("Add product fail !");
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
    }
}
