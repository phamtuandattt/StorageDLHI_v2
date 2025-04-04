using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            var ori_code = cboOrigin.SelectedValue.ToString().Trim();
            var m_type_code = cboType.SelectedValue.ToString().Trim();
            var stand_code = cboStandard.SelectedValue.ToString().Trim();

        }
    }
}
