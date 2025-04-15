using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.WarehouseDAO
{
    public static class WarehouseDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static bool Insert(Warehouses warehouses)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_WAREHOUSE, warehouses.Id, warehouses.Warehouse_Code,
                                    warehouses.Warehouse_Name, warehouses.Warehouse_Address);
            return data.Insert(sqlQuery) > 0;
        }
        
        public static DataTable GetWarehosueForCbo()
        {
            return data.GetData(QueryStatement.GET_WAREHOUSE_FOR_CBO, "WAREHOUSE_FOR_CBO");
        }
    }
}
