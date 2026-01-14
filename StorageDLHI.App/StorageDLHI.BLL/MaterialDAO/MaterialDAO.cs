using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.MaterialDAO
{
    public class MaterialDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<bool> InsertOrigin(Origins origin)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_ORIGIN, origin.Id, origin.Origin_Code, origin.Origin_Des);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateOrigin(Origins origin)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_ORIGIN, origin.Origin_Code, origin.Origin_Des, origin.Id);
            return await data.Update(sqlQuery) > 0;
        }

        public static async Task<bool> InsertMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATERIAL_TYPE, type.Id, type.Type_Code, type.Type_Des);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_TYPE, type.Type_Code, type.Type_Des, type.Id);
            return await data.Update(sqlQuery) > 0;
        }

        public static async Task<bool> InsertMaterialStandards(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATEIAL_STANDARD, standards.Id, standards.Standard_Code, standards.Standard_Des);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateMaterialStandard(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_STANDARD, standards.Standard_Code, standards.Standard_Des, standards.Id);
            return await data.Update(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetOriginForCombobox()
        {
            return await GetDataForCombobox(QueryStatement.GET_ORIGINS, "OrginForCob");
        }

        /// <summary>
        /// Get Types of Product
        /// </summary>
        /// <returns></returns>
        public static async Task<DataTable> GetMTypeForCombobox()
        {
            return await GetDataForCombobox(QueryStatement.GET_TYPES, "MTypeForCob");
        }

        /// <summary>
        /// Get Materials of Type
        /// </summary>
        /// <returns></returns>
        public static async Task<DataTable> GetMaterialOfTypeForCombobox()
        {
            return await data.GetDataAsync(QueryStatement.GET_MATERIAL_OF_TYPE_FOR_COMBOBOX, "Material_Of_TypeForCob");
        }

        /// <summary>
        /// Get the list item of material type
        /// </summary>
        /// <returns></returns>
        public static async Task<DataTable> GetItemOfMaterialTypeForCombobox()
        {
            return await data.GetDataAsync(QueryStatement.GET_ITEMS_OF_MATERIAL_TYPE, "Item_Of_Material_TypeForCob");
        }

        public static async Task<DataTable> GetStandForCombobox()
        {
            return await GetDataForCombobox(QueryStatement.GET_MATERIAL_STANDARDS, "StandForCob");
        }

        public static async Task<DataTable> GetDataForCombobox(string sqlQuery, string tableName)
        {
            var dt = await data.GetDataAsync(sqlQuery, tableName);
            var dtForCbo = new DataTable();
            dtForCbo.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_VALUE);
            dtForCbo.Columns.Add(QueryStatement.PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY);

            foreach (DataRow row in dt.Rows)
            {
                DataRow r = dtForCbo.NewRow();
                r[0] = row[0].ToString().Trim();
                r[1] = row[2].ToString().Trim() + "|" + row[1].ToString().Trim();
                dtForCbo.Rows.Add(r);
            }

            return dtForCbo;
        }

        public static async Task<DataTable> GetOrigins()
        {
            return await data.GetDataAsync(QueryStatement.GET_ORIGINS, "Origins");
        }

        public static async Task<DataTable> GetMaterialTypes() 
        {
            return await data.GetDataAsync(QueryStatement.GET_MATERIAL_OF_TYPE, "Types");
        }

        public static async Task<DataTable> GetMaterialTypes_V2(Guid typeId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_MATERIAL_TYPE_BY_TYPE_ID, typeId) , $"Types_{typeId}");
        }

        public static async Task<DataTable> GetMaterialStandards()
        {
            return await data.GetDataAsync(QueryStatement.GET_MATERIAL_STANDARDS, "Standards");
        }

        public static async Task<bool> InsertTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_TAX, taxs.Id, taxs.Tax_Percent, taxs.Tax_Value);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> InsertUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_UNIT, units.Id, units.Unit_Code);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> InsertCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_COST, costs.Id, costs.Cost_Name, costs.Currency_code, costs.Currency_Value, costs.Currency);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_TAX, taxs.Tax_Percent, taxs.Tax_Value, taxs.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_UNIT, units.Unit_Code, units.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_COST, costs.Cost_Name, costs.Id, costs.Currency_code, costs.Currency_Value, costs.Currency);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateExchangeRateCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_EXCHANGE_RATE_COST, costs.Currency_Value, costs.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetTaxs()
        {
            return  await data.GetDataAsync(QueryStatement.GET_TAXS, "TAXS");
        }

        public static async Task<DataTable> GetTaxCustoms()
        {
            return await data.GetDataAsync(QueryStatement.GET_TAX_CUSTOM, "TAX_CUS");
        }

        public static async Task<DataTable> GetFormulas()
        {
            return await data.GetDataAsync(QueryStatement.GET_FORMULAR_CAL, "FORMULAS");
        }
        
        public static async Task<DataTable> GetFormulaParas()
        {
            return await data.GetDataAsync(QueryStatement.GET_FORMUAL_PARA, "PARAS");
        }

        public static async Task<DataTable> GetUnits()
        {
            return await data.GetDataAsync(QueryStatement.GET_UNIT, "UNITS");
        }

        public static async Task<DataTable> GetCosts()
        {
            return await data.GetDataAsync(QueryStatement.GET_COST, "COSTS");
        }
    }
}
