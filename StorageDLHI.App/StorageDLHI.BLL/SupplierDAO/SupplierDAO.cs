using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.SupplierDAO
{
    public class SupplierDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static DataTable GetSuppliers()
        {
            var supls = data.GetData(QueryStatement.GET_SUPPLIERS, "SUPPLIERS");
            return supls;
        }

        public static bool InsertSupplier(Suppliers model)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_SUPPLIERS, model.Id, model.Name, model.Cert, model.Email, model.Phone,
                model.Viettat, model.Address, model.Bank_Id);

            return data.Insert(sqlQuery) > 0;
        }

        public static DataTable GetBankBySupplier(Guid supId)
        {
            return data.GetData(string.Format(QueryStatement.GET_BANK_BY_SUPPLIER, supId), "BANK");
        }

        public static DataTable GetSupplierBanks()
        {
            var supls = data.GetData(QueryStatement.GET_SUPPLIER_BANKS, "SUPPLIER_BANKS");
            return supls;
        }
    }
}
