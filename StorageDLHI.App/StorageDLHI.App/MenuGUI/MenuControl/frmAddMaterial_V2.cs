using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
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
    public partial class frmAddMaterial_V2 : KryptonForm
    {
        private DataTable dtMaterialOfType = new DataTable();
        private bool _typeLoaded = false;
        private bool _materialTypeLoaded = false;

        public frmAddMaterial_V2()
        {
            InitializeComponent();
        }
        private async void frmAddMaterial_V2_Load(object sender, EventArgs e)
        {
            cboType.DisplayMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY;
            cboType.ValueMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE;

            cboMaterialType.DisplayMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY;
            cboMaterialType.ValueMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE;

            await LoadType();
            await LoadMaterialOfType();
            if (_typeLoaded && _materialTypeLoaded)
            {
                var dtMaterialType = GetDataForComboBoxMaterialType(Guid.Parse(cboType.SelectedValue.ToString().Trim()));
                cboMaterialType.DataSource = dtMaterialType;
            }
        }

        private async Task LoadType()
        {
            // Type
            var dtTypes = await MaterialDAO.GetMTypeForCombobox();

            if (dtTypes.Rows.Count > 0)
            {
                cboType.DataSource = dtTypes;
                _typeLoaded = true;
            }
            else
            {
                MessageBoxHelper.ShowError("Please check the list of Types !");
            }
        }

        private async Task LoadMaterialOfType()
        {
            if (!CacheManager.Exists(CacheKeys.MATERIAL_OF_TYPES))
            {
                dtMaterialOfType = await ShowDialogManager.WithLoader(() => MaterialDAO.GetMaterialOfTypeForCombobox());
                if (dtMaterialOfType.Rows.Count > 0)
                {
                    CacheManager.Add(CacheKeys.MATERIAL_OF_TYPES, dtMaterialOfType);
                    _materialTypeLoaded = true;
                }
                else
                {
                    MessageBoxHelper.ShowError("Please check the list materials of The types !");
                }
            }
            else
            {
                dtMaterialOfType = CacheManager.Get<DataTable>(CacheKeys.MATERIAL_OF_TYPES);
                _materialTypeLoaded = true;
            }
        }

        private DataTable GetDataForComboBoxMaterialType(Guid typeId)
        {
            if (CacheManager.Exists(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId)))
            {
                return CacheManager.Get<DataTable>(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId));
            }

            //var filtered = dtMaterialOfType.AsEnumerable();
            //var rs = filtered.Where(r => r.Field<Guid>("ID").Equals(typeId));
            DataRow[] dataRows = dtMaterialOfType.Select($"MATERIAL_TYPES_ID = '{typeId.ToString()}'");


            if (!dataRows.Any())
            {
                return null;
            }

            var data = new DataTable();
            data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE);
            data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY);
            foreach (DataRow row in dataRows)
            {
                DataRow r = data.NewRow();
                r[0] = row[0].ToString().Trim();
                r[1] = row[2].ToString().Trim() + "|" + row[1].ToString().Trim();
                data.Rows.Add(r);
            }
            CacheManager.Add(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId), data);
            return data;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_typeLoaded) return;
            var typeId = Guid.Parse(cboType.SelectedValue.ToString().Trim());
            var materialType = GetDataForComboBoxMaterialType(typeId);
            cboMaterialType.DataSource = materialType;
        }

        private async void cboMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var itemNumberOfMaterialType = await ShowDialogManager.WithLoader(() => ProductDAO.GetItemNumberOfMaterialType(Guid.Parse(cboMaterialType.SelectedValue.ToString())));
            
            
        
        }
    }
}
