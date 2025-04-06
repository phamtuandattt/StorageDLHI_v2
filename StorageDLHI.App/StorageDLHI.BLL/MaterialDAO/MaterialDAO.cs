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

        public static bool InsertOrigin(Origins origin)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_ORIGIN, origin.Id, origin.Origin_Code, origin.Origin_Des);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateOrigin(Origins origin)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_ORIGIN, origin.Origin_Code, origin.Origin_Des, origin.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static bool InsertMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATERIAL_TYPE, type.Id, type.Type_Code, type.Type_Des);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateMaterialType(Material_Types type)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_TYPE, type.Type_Code, type.Type_Des, type.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static bool InsertMaterialStandards(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_MATEIAL_STANDARD, standards.Id, standards.Standard_Code, standards.Standard_Des);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateMaterialStandard(Material_Standards standards)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MATERIAL_STANDARD, standards.Standard_Code, standards.Standard_Des, standards.Id);
            return data.Update(sqlQuery) > 0;
        }

        public static DataTable GetOriginForCombobox()
        {
            return GetDataForCombobox(QueryStatement.GET_ORIGINS, "OrginForCob");
        }

        public static DataTable GetMTypeForCombobox()
        {
            return GetDataForCombobox(QueryStatement.GET_MATERIAL_TYPES, "MTypeForCob");
        }

        public static DataTable GetStandForCombobox()
        {
            return GetDataForCombobox(QueryStatement.GET_MATERIAL_STANDARDS, "StandForCob");
        }

        public static DataTable GetDataForCombobox(string sqlQuery, string tableName)
        {
            var dt = data.GetData(sqlQuery, tableName);
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

        public static DataTable GetOrigins()
        {
            return data.GetData(QueryStatement.GET_ORIGINS, "Origins");
        }

        public static DataTable GetMaterialTypes() 
        {
            return data.GetData(QueryStatement.GET_MATERIAL_TYPES, "Types");
        }

        public static DataTable GetMaterialStandards()
        {
            return data.GetData(QueryStatement.GET_MATERIAL_STANDARDS, "Standards");
        }

        public static bool InsertTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_TAX, taxs.Id, taxs.Tax_Percent);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool InsertUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_UNIT, units.Id, units.Unit_Code);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool InsertCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_COST, costs.Id, costs.Cost_Name);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateTax(Taxs taxs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_TAX, taxs.Tax_Percent, taxs.Id);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateUnit(Units units)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_UNIT, units.Unit_Code, units.Id);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool UpdateCost(Costs costs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_COST, costs.Cost_Name, costs.Id);
            return data.Insert(sqlQuery) > 0;
        }

        public static DataTable GetTaxs()
        {
            return data.GetData(QueryStatement.GET_TAXS, "TAXS");
        }

        public static DataTable GetUnits()
        {
            return data.GetData(QueryStatement.GET_UNIT, "UNITS");
        }

        public static DataTable GetCosts()
        {
            return data.GetData(QueryStatement.GET_COST, "COSTS");
        }
    }
}
