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

        public static async Task<DataTable> GetWarehouses()
        {
            return await data.GetDataAsync(QueryStatement.GET_WAREHOUSES, "WAREHOUSES");
        } 

        public static async Task<DataTable> GetWarehosueForCbo()
        {
            return await data.GetDataAsync(QueryStatement.GET_WAREHOUSE_FOR_CBO, "WAREHOUSE_FOR_CBO");
        }

        public static async Task<DataTable> GetWarehouseForCboOfExportProd(Guid whId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_WAREHOUSE_FOR_COMBOBOX_EXPORT_PROD, whId), $"FOR_CBO_WAREHOUS_{whId}");
        }

        public static async Task<DataTable> GetWarehouseDetailForm()
        {
            return await data.GetDataAsync(QueryStatement.GET_WAREHOUSE_DETAIL_FORM, "WAREHOUSE_DETAIL_FORM");
        }

        public static bool UpdateQtyProdOfWarehouse(DataTable dtWarehouseDetail)
        {
            return data.UpdateRowExistOfTable(QueryStatement.UPDATE_OR_ADD_PRODUCT_EXIST_WAREHOUES, 
                QueryStatement.INPUT_FOR_PROC, QueryStatement.WAREHOUSE_TABLE_TYPE, dtWarehouseDetail, 
                QueryStatement.OUTPUT_FOR_PROC) > 0;
        }

        public static bool UpdateQtyProdOfWarehouse(DataTable dtWarehouseDetail, Warehouse_Detail warehousesDetail)
        {
            var isSuc = data.UpdateRowExistOfTable(QueryStatement.UPDATE_OR_ADD_PRODUCT_EXIST_WAREHOUES,
                    QueryStatement.INPUT_FOR_PROC, QueryStatement.WAREHOUSE_TABLE_TYPE, dtWarehouseDetail,
                    QueryStatement.OUTPUT_FOR_PROC) > 0;
            if (isSuc)
            {
                return data.Update(string.Format(QueryStatement.UPDATE_QTY_OF_PROD_AFTER_EXPORTED, warehousesDetail.Product_In_Stock, 
                    warehousesDetail.Warehosue_Id, warehousesDetail.ProductId)) > 0;
            }
            return false;
        }

        public static async Task<DataTable> GetWarehouseDetailByWarehouseId(Guid warehouseId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_WAREHOUSE_DETAIL_BY_WAREHOUSE_ID, warehouseId), $"WAREHOUES_DETAIL_{warehouseId}");
        }
    }
}
