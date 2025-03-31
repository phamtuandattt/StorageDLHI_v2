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
    }
}
