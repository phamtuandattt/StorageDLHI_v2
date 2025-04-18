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

        public static DataTable GetWarehouses()
        {
            return data.GetData(QueryStatement.GET_WAREHOUSES, "WAREHOUSES");
        } 

        public static DataTable GetWarehosueForCbo()
        {
            return data.GetData(QueryStatement.GET_WAREHOUSE_FOR_CBO, "WAREHOUSE_FOR_CBO");
        }

        public static DataTable GetWarehouseDetailForm()
        {
            return data.GetData(QueryStatement.GET_WAREHOUSE_DETAIL_FORM, "WAREHOUSE_DETAIL_FORM");
        }

        public static bool InsertProdIntoWarehouseDetail(DataTable dtWarehouseDetail)
        {
            return data.UpdateRowExistOfTabl(QueryStatement.UPDATE_OR_ADD_PRODUCT_EXIST_WAREHOUES, 
                QueryStatement.INPUT_FOR_PROC, QueryStatement.WAREHOUSE_TABLE_TYPE, dtWarehouseDetail, 
                QueryStatement.OUTPUT_FOR_PROC) > 0;
        }

        public static DataTable GetWarehouseDetailByWarehouseId(Guid warehouseId)
        {
            return data.GetData(string.Format(QueryStatement.GET_WAREHOUSE_DETAIL_BY_WAREHOUSE_ID, warehouseId), $"WAREHOUES_DETAIL_{warehouseId}");
        }
    }
}
