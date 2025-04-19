using StorageDLHI.DAL.DataProvider;
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

        public static DataTable GetDeliveryDetailForm()
        {
            return data.GetData(QueryStatement.GET_DELIVERY_DETAIL_FORM, "DELIVERY_DETAIL_FORM");
        }
    }
}
