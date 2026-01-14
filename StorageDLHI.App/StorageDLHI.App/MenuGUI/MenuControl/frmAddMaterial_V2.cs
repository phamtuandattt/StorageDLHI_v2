using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.Models;
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
        private bool _isUpdate = false;
        private Material_Type_Detail mMaterial = new Material_Type_Detail();

        public frmAddMaterial_V2()
        {
            InitializeComponent();
        }

        public frmAddMaterial_V2(bool isUpdate, Material_Type_Detail material_Type_Detail)
        {
            InitializeComponent();
            this._isUpdate = isUpdate;
            this.mMaterial = material_Type_Detail;
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
                var typeId = Guid.Parse(cboType.SelectedValue.ToString().Trim());

                //var dtMaterialType = GetDataForComboBoxMaterialType(typeId);
                //cboMaterialType.DataSource = dtMaterialType;

                var dtFollowTypeId = GetDataForDataGridViewMaterialType(typeId);
                dgvMaterialOfType.DataSource = dtFollowTypeId;
                Common.Common.ConfigDataGridView(dtFollowTypeId, dgvMaterialOfType, Common.Common.GetHiddenColumns(QueryStatement.HiddenColumnDataGridViewOfAddMaterial));
            
                if (_isUpdate)
                {
                    cboType.SelectedValue = mMaterial.Material_Types_Id;
                }    
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

        //private DataTable GetDataForComboBoxMaterialType(Guid typeId)
        //{
        //    if (CacheManager.Exists(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId)))
        //    {
        //        return CacheManager.Get<DataTable>(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId));
        //    }

        //    //var filtered = dtMaterialOfType.AsEnumerable();
        //    //var rs = filtered.Where(r => r.Field<Guid>("ID").Equals(typeId));
        //    DataRow[] dataRows = dtMaterialOfType.Select($"MATERIAL_TYPES_ID = '{typeId.ToString()}'");


        //    if (!dataRows.Any())
        //    {
        //        return null;
        //    }

        //    var data = new DataTable();
        //    data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE);
        //    data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY);
        //    foreach (DataRow row in dataRows)
        //    {
        //        DataRow r = data.NewRow();
        //        r[0] = row[0].ToString().Trim();
        //        r[1] = row[2].ToString().Trim() + "|" + row[1].ToString().Trim();
        //        data.Rows.Add(r);
        //    }
        //    CacheManager.Add(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId), data);
        //    return data;
        //}

        private DataTable GetDataForDataGridViewMaterialType(Guid typeId)
        {
            if (CacheManager.Exists(string.Format(CacheKeys.MATERIAL_TYPE_DETAIL_FOR_DATAGRIDVIEW_BY_TYPE_ID, typeId)))
            {
                return CacheManager.Get<DataTable>(string.Format(CacheKeys.MATERIAL_TYPE_DETAIL_FOR_DATAGRIDVIEW_BY_TYPE_ID, typeId));
            }

            //var filtered = dtMaterialOfType.AsEnumerable();
            //var rs = filtered.Where(r => r.Field<Guid>("ID").Equals(typeId));
            DataRow[] dataRows = dtMaterialOfType.Select($"{QueryStatement.PROPERTY_MATERIAL_TYPES_ID} = '{typeId.ToString()}'");

            if (!dataRows.Any())
            {
                return null;
            }

            var data = new DataTable();
            data.Columns.Add(QueryStatement.PROPERTY_MATERIAL_ID);
            data.Columns.Add(QueryStatement.PROPERTY_MATERIAL_TYPE_CODE);
            data.Columns.Add(QueryStatement.PROPERTY_MATERIAL_TYPE_NAME);
            data.Columns.Add(QueryStatement.PROPERTY_MATERIAL_TYPES_ID);

            foreach (DataRow row in dataRows)
            {
                DataRow r = data.NewRow();
                r[0] = row[0];
                r[1] = row[1];
                r[2] = row[2];
                r[3] = row[3];
                data.Rows.Add(r);
            }
            CacheManager.Add(string.Format(CacheKeys.MATERIAL_TYPE_DETAIL_FOR_DATAGRIDVIEW_BY_TYPE_ID, typeId), data);
            return data;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please enter Type Code before add new !");
                txtCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBoxHelper.ShowWarning("Please enter Type Name before add new !");
                txtName.Focus();
                return;
            }

            if (_isUpdate)
            {

            }
            else
            {
                Material_Type_Detail model = new Material_Type_Detail()
                {
                    Id = Guid.NewGuid(),
                    Material_Type_Code = txtCode.Text.Trim().ToUpper(),
                    Material_Type_Name = txtName.Text.Trim(),
                    Material_Types_Id = Guid.Parse(cboType.SelectedValue.ToString().Trim()),
                };

                if (await MaterialDAO.InsertMaterialTypeDetail(model))
                {
                    MessageBoxHelper.ShowInfo("Add Material success !");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowError("Add Material fail !");
                }
            }
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_typeLoaded) return;
            var typeId = Guid.Parse(cboType.SelectedValue.ToString().Trim());

            var dtFollowTypeId = GetDataForDataGridViewMaterialType(typeId);
            dgvMaterialOfType.DataSource = dtFollowTypeId;
        }

        private void dgvMaterialOfType_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void cboType_Validating(object sender, CancelEventArgs e)
        {
            if (cboMaterialType.Items.Count <= 0) return;
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }
    }
}

