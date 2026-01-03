using ComponentFactory.Krypton.Toolkit;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.MaterialDAO;
using StorageDLHI.BLL.ProductDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Commons;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomProd_v2 : KryptonForm
    {
        private string path = string.Empty;
        private bool status = false;
        private Products pModel = null;
        private Guid Material_Type_Id = Guid.Empty;
        private string itemNumberOfMaterialType = string.Empty;

        private DataTable dtMaterialOfType = new DataTable();
        public frmCustomProd_v2()
        {
            InitializeComponent();
            LoadData();
        }

        private void frmCustomProd_v2_Load(object sender, EventArgs e)
        {

        }

        public frmCustomProd_v2(string title, bool status, Products pModel) // true is ADD || UPDATE
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

            var dtStand = await MaterialDAO.GetStandForCombobox();
            LoadDataCombox(cboStandard, dtStand);

            cboUnit.DisplayMember = QueryStatement.PROPERTY_UNIT_CODE;
            cboUnit.ValueMember = QueryStatement.PROPERTY_UNIT_ID;
            var dtUnits = await MaterialDAO.GetUnits();
            cboUnit.DataSource = dtUnits;
            if (dtUnits.Rows.Count > 0)
            {
                cboUnit.SelectedIndex = 0;
            }

            // Type
            cboType.DisplayMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY;
            cboType.ValueMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE;
            var dtTypes = await MaterialDAO.GetMTypeForCombobox();
            
            if (dtTypes.Rows.Count > 0)
            {
                LoadDataCombox(cboType, dtTypes);
            }
            else
            {
                MessageBoxHelper.ShowError("Please check the list of Types !");
            }
            

            // --------------------- Material of type
            if (!CacheManager.Exists(CacheKeys.MATERIAL_OF_TYPES))
            {
                dtMaterialOfType = await ShowDialogManager.WithLoader(() => MaterialDAO.GetMaterialOfTypeForCombobox());
                if (dtMaterialOfType.Rows.Count > 0)
                {
                    CacheManager.Add(CacheKeys.MATERIAL_OF_TYPES, dtMaterialOfType);
                }
                else
                {
                    MessageBoxHelper.ShowError("Please check the list materials of The types !");
                }
            }
            else
            {
                dtMaterialOfType = CacheManager.Get<DataTable>(CacheKeys.MATERIAL_OF_TYPES);
            }

            // Get list material of type by TypeId and Load data for Combobox
            var dtCombobox = GetDataForComboBoxMaterialType(Guid.Parse(cboType.SelectedValue.ToString()));
            if (dtCombobox != null && dtCombobox.Rows.Count > 0)
            {
                cboMaterialOfType.DisplayMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY;
                cboMaterialOfType.ValueMember = QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE;
                cboMaterialOfType.DataSource = dtCombobox;
            }
            else
            {
                MessageBoxHelper.ShowError("Please check the list materials of The types !");
            }
            //---------------------------------------------
        }

        private DataTable GetDataForComboBoxMaterialType(Guid typeId)
        {
            if (CacheManager.Exists(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId)))
            {
                return CacheManager.Get<DataTable>(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId));
            }

            var filtered = dtMaterialOfType.AsEnumerable()
                .Where(r => r.Field<Guid>("MATERIAL_TYPES_ID").Equals(typeId));
            if (!filtered.Any())
            {
                return null;
            }

            var data = new DataTable();
            data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE);
            data.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY);
            foreach (DataRow row in filtered.CopyToDataTable().Rows)
            {
                DataRow r = data.NewRow();
                r[0] = row[0].ToString().Trim();
                r[1] = row[2].ToString().Trim() + "|" + row[1].ToString().Trim();
                data.Rows.Add(r);
            }
            CacheManager.Add(string.Format(CacheKeys.MATERIAL_OF_TYPE_BY_TYPE_ID, typeId), data);
            return data;
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

        private void cboOrigin_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboStandard_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboType_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboMaterialOfType_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboUnit_Validating(object sender, CancelEventArgs e)
        {
            Common.Common.AutoCompleteComboboxValidating(sender as KryptonComboBox, e);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Items.Count <= 0)
            {
                return;
            }
            var typeId = cboType.SelectedValue.ToString();
            var dtMaterialOfType = GetDataForComboBoxMaterialType(Guid.Parse(cboType.SelectedValue.ToString()));
            if (dtMaterialOfType != null && dtMaterialOfType.Rows.Count >= 0)
            {
                cboMaterialOfType.DataSource = dtMaterialOfType;
                cboMaterialOfType.Enabled = true;
            }
            else
            {
                cboMaterialOfType.Enabled = false;
            }
        }

        private async void btnGenerateCode_Click(object sender, EventArgs e)
        {
            itemNumberOfMaterialType = await ShowDialogManager.WithLoader(() => ProductDAO.GetItemNumberOfMaterialType(Guid.Parse(cboMaterialOfType.SelectedValue.ToString())));
            
            var H = !string.IsNullOrEmpty(txtThinh.Text.Trim()) ? txtThinh.Text.Trim() : txtDep.Text.Trim();
            var B = txtWidth.Text.Trim();
            var T1 = txtWeb.Text.Trim();
            var T2 = txtFlag.Text.Trim();
            
            var oriInfos = cboOrigin.Text.ToString().Split('|');
            var materialOfType = cboMaterialOfType.Text.ToString().Trim().Split('|');
            var standInfo = cboStandard.Text.ToString().Split('|');

            var ori_code = oriInfos[1].Trim();
            var materialType = materialOfType[1].Trim();
            var materialCode = materialType + "" + itemNumberOfMaterialType;
            var stand_code = standInfo[1].Trim();
            var size = H + B + T1 + T2;
            var length = txtLength.Text.Trim();


            var prod_code = string.Concat(ori_code, materialCode, stand_code, size, "_", length);
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
                // Add Item Type
                Material_Type_Detail_Item item = new Material_Type_Detail_Item()
                {
                    Id = Guid.NewGuid(),
                    Item_Number = itemNumberOfMaterialType,
                    Item_Name = txtProdName.Text.Trim(),
                    Item_Type = Guid.Parse(cboMaterialOfType.SelectedValue.ToString())
                };
                if (await ShowDialogManager.WithLoader(() => ProductDAO.InsertMaterialTypeDetailItem(item)))
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
                        UnitId = Guid.Parse(cboUnit.SelectedValue.ToString()), // 15
                        Product_TypeId = Guid.Empty,
                        Origin_Id = Guid.Parse(cboOrigin.SelectedValue.ToString()),
                        Stand_Id = Guid.Parse(cboStandard.SelectedValue.ToString()),
                        Type_Id = Guid.Parse(cboType.SelectedValue.ToString()),
                        Materials_Of_Type = Guid.Parse(cboMaterialOfType.SelectedValue.ToString()),
                        Item_Type = item.Id
                    };

                    if (!string.IsNullOrEmpty(path))
                    {
                        if (await ShowDialogManager.WithLoader(() => ProductDAO.Insert_v2(prod)))
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
                        if (await ShowDialogManager.WithLoader(() => ProductDAO.InsertNoImage_v2(prod)))
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
                    var rs = ProductDAO.DeleteMaterialTypeDetailItem(item);
                    MessageBoxHelper.ShowWarning("Add product fail !");
                }
            }
            else
            {
                //var oriInfos = cboOrigin.Text.ToString().Split('|');
                //var mTypeInfo = cboType.Text.ToString().Split('|');
                //var standInfo = cboStandard.Text.ToString().Split('|');

                //Products prod = new Products()
                //{
                //    Id = this.pModel.Id,
                //    Product_Name = txtProdName.Text.Trim(),
                //    Product_Des_2 = txtDes2.Text.Trim().ToUpper(),
                //    Product_Code = txtProdCode.Text.Trim().ToUpper(),
                //    Product_Material_Code = standInfo[1].Trim(), // cboStandard.Text.Trim(),
                //    PictureLink = path,
                //    Picture = path,
                //    A_Thinhness = txtThinh.Text.Trim(),
                //    B_Depth = txtDep.Text.Trim(),
                //    C_Witdh = txtWidth.Text.Trim(),
                //    D_Web = txtWeb.Text.Trim(),
                //    E_Flag = txtFlag.Text.Trim(),
                //    F_Length = txtLength.Text.Trim(),
                //    G_Weight = txtWeigth.Text.Trim(),
                //    Used_Note = txtUsageNote.Text.Trim(),
                //    UnitId = Guid.Parse(cboUnit.SelectedValue.ToString()),
                //    Origin_Id = Guid.Parse(cboOrigin.SelectedValue.ToString()),
                //    Type_Id = Guid.Parse(cboType.SelectedValue.ToString()),
                //    Stand_Id = Guid.Parse(cboStandard.SelectedValue.ToString()),
                //};

                //if (!string.IsNullOrEmpty(path))
                //{
                //    if (await ShowDialogManager.WithLoader(() => ProductDAO.Update(prod)))
                //    {
                //        MessageBoxHelper.ShowInfo("Update product success !");
                //        this.Close();
                //    }
                //    else
                //    {
                //        MessageBoxHelper.ShowWarning("Update product fail !");
                //    }
                //}
                //else
                //{
                //    if (await ShowDialogManager.WithLoader(() => ProductDAO.InsertNoImage(prod)))
                //    {
                //        MessageBoxHelper.ShowInfo("Update product success !");
                //        this.Close();
                //    }
                //    else
                //    {
                //        MessageBoxHelper.ShowWarning("Update product fail !");
                //    }
                //}
            }
        }
    }
}
