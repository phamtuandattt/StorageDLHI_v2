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

        public static DataTable GetPOs()
        {
            return data.GetData(QueryStatement.GET_POS, "POS");
        }

        public static DataTable GetPODetailById(Guid poId)
        {
            string sqlQuery = string.Format(QueryStatement.GET_PO_DETAIL_BY_PO_ID, poId);
            return data.GetData(sqlQuery, $"PO_DETAILS.{poId}");
        }

        public static bool InsertPO(Pos pos)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_PO, pos.Id, pos.Po_No, pos.Po_Mpr_No, pos.Po_Wo_No, pos.Po_Project_Name,
                pos.Po_Rev_Total, pos.Po_CreateDate, pos.Po_Expected_Delivery_Date, pos.Po_Prepared, pos.Po_Reviewed, pos.Po_Agrement,
                pos.Po_Approved, pos.Po_Payment_Term, pos.Po_Dispatch_Box, pos.Po_Total_Amount, pos.CostId, pos.TaxId, pos.SupplierId, pos.Staff_Id);

            return data.Insert(sqlQuery) > 0;
        }

        public static bool InsertPODetail(DataTable dtPODetail)
        {
            return data.UpdateDatabase(QueryStatement.GET_PO_DETAILS, dtPODetail);
        }

        public static DataTable GetPODetailForm()
        {
            return data.GetData(QueryStatement.GET_PO_DETAIL_FORM, "PO_DETAIL_FORM");
        }

        public static bool DeletePO(Guid id)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_PO_BY_ID, id)) > 0;
        }
    }
}
