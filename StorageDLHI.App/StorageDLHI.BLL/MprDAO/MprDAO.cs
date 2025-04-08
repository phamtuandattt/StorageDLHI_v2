using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.MprDAO
{
    public static class MprDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static bool InsertMpr(Mprs mprs)
        {
            string sqlQuery = string.Format(QueryStatement.ADD_MPR, mprs.Id, mprs.Mpr_No, mprs.Mpr_Wo_No, mprs.Mpr_Project_Name_Code, mprs.Mpr_Rev_Total,
                mprs.CreateDate, mprs.Expected_Delivery_Date, mprs.Mpr_Prepared, mprs.Mpr_Reviewed, mprs.Mpr_Approved);

            return data.Insert(sqlQuery) > 0;
        }

        public static bool InsertMprDetail(DataTable dtMprDetail)
        {
            return data.UpdateDatabase(QueryStatement.GET_MPR_DETAILs, dtMprDetail);
        }

        public static bool DeleteMpr(Guid mprId)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_MPR, mprId)) > 0;
        }

        public static DataTable GetMprDetailForm()
        {
            return data.GetData(QueryStatement.GET_MPR_DETAIL_FORM, "MPR_DETAIL_FORM");
        }
    }
}
