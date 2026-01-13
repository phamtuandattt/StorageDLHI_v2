using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.PoDAO
{
    public static class PoDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<DataTable> GetPOs()
        {
            return await data.GetDataAsync(QueryStatement.GET_POS, "POS");
        }

        public static async Task<DataTable> GetPOs_V2(Guid projectId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_POS_V2, projectId), $"POS_{projectId}");
        }

        public static async Task<DataTable> GetPosForImportProduct()
        {
            return await data.GetDataAsync(QueryStatement.GET_POS_FOR_IMPORT_PRODUCT, "PO_FOR_IMRPOT_PROD");
        }

        public static async Task<bool> UpdateIsImportedForPO(bool isImported, Guid id)
        {
            return await data.Update(string.Format(QueryStatement.UPDATE_PO_IS_IMPORTED, isImported, id)) > 0;
        } 

        public static async Task<DataTable> GetPODetailById(Guid poId)
        {
            string sqlQuery = string.Format(QueryStatement.GET_PO_DETAIL_BY_PO_ID, poId);
            return await data.GetDataAsync(sqlQuery, $"PO_DETAILS.{poId}");
        }

        public static async Task<DataTable> GetPODetailByIdForImport(Guid poId)
        {
            string sqlQuery = string.Format(QueryStatement.GET_PO_DETAIL_BY_PO_ID_FOR_IMPORT, poId);
            return await data.GetDataAsync(sqlQuery, $"PO_DETAILS_FOR_IMPORT.{poId}");
        }

        public static async Task<bool> InsertPO(Pos pos)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_PO, pos.Id, pos.Po_No, pos.Po_Mpr_No, pos.Po_Wo_No, pos.Po_Project_Name,
                pos.Po_Rev_Total, pos.Po_CreateDate, pos.Po_Expected_Delivery_Date, pos.Po_Prepared, pos.Po_Reviewed, pos.Po_Agrement,
                pos.Po_Approved, pos.Po_Payment_Term, pos.Po_Dispatch_Box, pos.Po_Total_Amount,
                 //pos.CostId, pos.TaxId, 
                 pos.IsImported, pos.SupplierId, pos.Staff_Id);

            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<bool> InsertPO_V2(Pos pos)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_PO_V2, pos.Id, pos.Po_No, pos.Po_Mpr_No, pos.Po_Wo_No, pos.Po_Project_Name,
                pos.Po_Rev_Total, pos.Po_CreateDate, pos.Po_Expected_Delivery_Date, pos.Po_Prepared, pos.Po_Reviewed, pos.Po_Agrement,
                pos.Po_Approved, pos.Po_Payment_Term, pos.Po_Dispatch_Box, pos.Po_Total_Amount,
                 //pos.CostId, pos.TaxId, 
                 pos.IsImported, pos.SupplierId, pos.Staff_Id, pos.Project_Id, pos.ReviewBy, pos.AgrementBy, pos.ApprovedBy);

            return await data.Insert(sqlQuery) > 0;
        }

        public static bool InsertPODetail(DataTable dtPODetail)
        {
            return data.UpdateDatabase(QueryStatement.GET_PO_DETAILS, dtPODetail);
        }

        public static async Task<DataTable> GetPODetailForm()
        {
            return await data.GetDataAsync(QueryStatement.GET_PO_DETAIL_FORM, "PO_DETAIL_FORM");
        }

        public static bool DeletePO(Guid id)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_PO_BY_ID, id)) > 0;
        }
    }
}
