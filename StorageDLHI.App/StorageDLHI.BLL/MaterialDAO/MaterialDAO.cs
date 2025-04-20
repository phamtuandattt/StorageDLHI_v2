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

        public static bool UpdateOrigin(Origins origin)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_ORIGIN, origin.Origin_Code, origin.Origin_Des, origin.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static async Task<bool> InsertMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATERIAL_TYPE, type.Id, type.Type_Code, type.Type_Des);
            return await data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_TYPE, type.Type_Code, type.Type_Des, type.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static async Task<bool> InsertMaterialStandards(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATEIAL_STANDARD, standards.Id, standards.Standard_Code, standards.Standard_Des);
            return await data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateMaterialStandard(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_STANDARD, standards.Standard_Code, standards.Standard_Des, standards.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetOriginForCombobox()
        {
            return await GetDataForCombobox(QueryStatement.GET_ORIGINS, "OrginForCob");
        }

        public static async Task<DataTable> GetMTypeForCombobox()
        {
            return await GetDataForCombobox(QueryStatement.GET_MATERIAL_TYPES, "MTypeForCob");
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
                r[1] = row[1].ToString().Trim() + "|" + row[2];
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
            return await data.GetDataAsync(QueryStatement.GET_MATERIAL_TYPES, "Types");
        }

        public static async Task<DataTable> GetMaterialStandards()
        {
            return await data.GetDataAsync(QueryStatement.GET_MATERIAL_STANDARDS, "Standards");
        }

        public static async Task<bool> InsertTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_TAX, taxs.Id, taxs.Tax_Percent);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> InsertUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_UNIT, units.Id, units.Unit_Code);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> InsertCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_COST, costs.Id, costs.Cost_Name);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_TAX, taxs.Tax_Percent, taxs.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_UNIT, units.Unit_Code, units.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> UpdateCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_COST, costs.Cost_Name, costs.Id);
            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetTaxs()
        {
            return  await data.GetDataAsync(QueryStatement.GET_TAXS, "TAXS");
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
