using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.ExportDAO
{
    public static class ExportProductDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<DataTable> GetDeliveryDetailForm()
        {
            return await data.GetDataAsync(QueryStatement.GET_DELIVERY_DETAIL_FORM, "DELIVERY_DETAIL_FORM");
        }

        public static async Task<bool> InsertDelivery(Delivery_Products delivery_Products, DataTable dtDeliveryDetail)
        {
            string sqlQuery = string.Format(QueryStatement.ADD_DELIVERY_PRODUCTS, delivery_Products.Id, delivery_Products.DeliveryDate,
                delivery_Products.DeliveryDay, delivery_Products.DeliveryMonth, delivery_Products.DeliveryYear,
                delivery_Products.Delivery_Total_Qty, delivery_Products.From_Warehouse_Id, delivery_Products.Staff_Id);

            if (await data.Insert(sqlQuery) > 0
                && data.UpdateDatabase(QueryStatement.GET_DELIVERY_PRODUCT_DETAIL, dtDeliveryDetail))
            {
                return true;
            }
            var rs = data.Delete(string.Format(QueryStatement.DELETE_DELIVERY_PRODUCTS, delivery_Products.Id));
            return false;
        }
    }
}
