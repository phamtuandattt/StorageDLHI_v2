using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.MprDAO
{
    public static class MprDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<DataTable> GetDataForExportAsync(Guid mprId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_MPR_FOR_EXPORT, mprId), $"DATA_EXPORT_FOR_{mprId}");
        }

        public static async Task<bool> UpdateMprIsMakePO(Guid mprID)
        {
            return await data.Update(string.Format(QueryStatement.UPDATE_MPRS_IS_MAKE_PO, true, mprID)) > 0;
        }

        public static async Task<bool> InsertMpr(Mprs mprs)
        {
            string sqlQuery = string.Format(QueryStatement.ADD_MPR, mprs.Id, mprs.Mpr_No, mprs.Mpr_Wo_No, mprs.Mpr_Project_Name_Code, mprs.Mpr_Rev_Total,
                mprs.CreateDate, mprs.Expected_Delivery_Date, mprs.Mpr_Prepared, mprs.Mpr_Reviewed, mprs.Mpr_Approved, mprs.Staff_Id, mprs.IsMakePO);

            return await data.Insert(sqlQuery) > 0;
        }

        public static bool InsertMprDetail(DataTable dtMprDetail)
        {
            return data.UpdateDatabase(QueryStatement.GET_MPR_DETAILs, dtMprDetail);
        }

        public static bool DeleteMpr(Guid mprId)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_MPR, mprId)) > 0;
        }

        public static async Task<DataTable> GetMprDetailForm()
        {
            return await data.GetDataAsync(QueryStatement.GET_MPR_DETAIL_FORM, "MPR_DETAIL_FORM");
        }

        public static async Task<DataTable> GetMprs()
        {
            return await data.GetDataAsync(QueryStatement.GET_MPRs, "MPRs");
        }

        public static async Task<DataTable> GetMprsForMakePO()
        {
            return await data.GetDataAsync(QueryStatement.GET_MPRS_FOR_MAKE_PO, "MPRS_FOR_MPO");
        }

        public static async Task<bool> UpdateMprInfo(Mprs mprs)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_MPR_INFO, mprs.Expected_Delivery_Date, mprs.Mpr_Prepared, mprs.Mpr_Reviewed,
                mprs.Mpr_Approved, mprs.Id);
            return await data.Update(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetMprDetailByMpr(Guid mprId)
        {
            string sqlQuery = string.Format(QueryStatement.GET_MPR_DETAIL_BY_ID, mprId);
            var dtMprDetail = await data.GetDataAsync(sqlQuery, "MPR_DETAIL_BY_MPR_ID");
            foreach (DataRow row in dtMprDetail.Rows)
            {
                row[7] = Common.CheckOrReturnNumber((row[7].ToString()));
                row[6] = Common.CheckOrReturnNumber((row[6].ToString()));
                row[8] = Common.CheckOrReturnNumber((row[8].ToString()));
                row[9] = Common.CheckOrReturnNumber((row[9].ToString()));
                row[10] = Common.CheckOrReturnNumber(row[10].ToString());
                row[11] = Common.CheckOrReturnNumber(row[11].ToString());
                row[12] = Common.CheckOrReturnNumber(row[12].ToString());
                row[13] = Common.CheckOrReturnNumber(row[13].ToString());
            }

            return dtMprDetail;
        }

        public static async Task<DataTable> GetNumberMprCreated(string projectName)
        {
            string sqlQurey = string.Format(QueryStatement.GET_NUMBER_OF_MPR_CREATED, projectName);
            return await data.GetDataAsync(sqlQurey, "NUMBER_OF_CREATED");
        }
    }
}
