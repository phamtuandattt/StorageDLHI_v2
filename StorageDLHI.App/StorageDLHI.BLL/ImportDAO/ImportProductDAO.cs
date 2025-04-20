using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.ImportDAO
{
    public static class ImportProductDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<DataTable> GetImportProducts()
        {
            return await data.GetDataAsync(QueryStatement.GET_IMPORTS, "IMPORTS");
        }

        public static async Task<DataTable> GetImportProductDetailByID(Guid guid)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_IMPORT_DETAIL_BY_ID, guid), $"IMPORT_DETAIL_BY_ID_{guid}");
        }

        public static async Task<DataTable> GetImportProductDetailForm()
        {
            return await data.GetDataAsync(QueryStatement.GET_IMPORT_DETAIL_FORM, "IMPORT_DETAIL_FORM");
        }

        public static bool Insert(Import_Products import_Products)
        {
            string sqlQuery = string.Format(QueryStatement.ADD_IMPORT_PRODUCT, import_Products.Id, import_Products.ImportDate,
                import_Products.ImportDay, import_Products.ImportMonth, import_Products.ImportYear, import_Products.Import_Total_Qty, 
                import_Products.Staff_Id, import_Products.FromPONo);
            return data.Insert(sqlQuery) > 0;
        }

        public static bool DeleteImportProduct(Guid guid)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_IMPORT_PRODUCT, guid)) > 0;
        }

        public static bool InsertImportProdDetail(DataTable dtImportDetails)
        {
            return data.UpdateDatabase(QueryStatement.GET_IMPORT_DETAILS, dtImportDetails);
        }
    }
}
