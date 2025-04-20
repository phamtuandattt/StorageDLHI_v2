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

        public static async Task<DataTable> GetSuppliers()
        {
            var supls = await data.GetDataAsync(QueryStatement.GET_SUPPLIERS, "SUPPLIERS");
            return supls;
        }

        public static bool InsertSupplier(Suppliers model)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_SUPPLIERS, model.Id, model.Name, model.Cert, model.Email, model.Phone,
                model.Viettat, model.Address);

            return data.Insert(sqlQuery) > 0;
        }

        public static bool DeleteSupplier(Suppliers model)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_SUPPLIER, model.Id)) > 0;
        }

        public static bool InsertBankOfSup(DataTable supplier_Bank_DataTable)
        {
            return data.UpdateDatabase(QueryStatement.GET_SUPPLIER_BANKS, supplier_Bank_DataTable);
        }

        public static bool UpdateSupplier(Suppliers supplier)
        {
            string sqlQuery = string.Format(QueryStatement.UPDATE_SUPPLIER, supplier.Name, supplier.Cert, supplier.Email, supplier.Phone, supplier.Viettat, supplier.Address, supplier.Id);
            return data.Update(sqlQuery) > 0;    
        }

        public static bool UpdateSupplierBank(DataTable supplier_Bank_DataTable_Update, DataTable banks_Add)
        {
            int rs = 0;
            foreach (DataRow row in supplier_Bank_DataTable_Update.Rows)
            {
                string sqlQuery = string.Format(QueryStatement.UPDATE_BANK, row[2].ToString(), row[3].ToString(), row[4].ToString(), row[0].ToString());
                rs = data.Update(sqlQuery);
            }
            if (rs >= 0 && InsertBankOfSup(banks_Add))
            {
                return true;
            }
            return false;
        }

        public static async Task<DataTable> GetBankBySupplier(Guid supId)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_BANK_BY_SUPPLIER, supId), "BANK");
        }

        public static bool DeleteBank(Guid BankId)
        {
            return data.Delete(string.Format(QueryStatement.DELETE_BANK, BankId)) > 0;
        }

        public static async Task<DataTable> GetSupplierBanksForms()
        {
            var supls = await data.GetDataAsync(QueryStatement.GET_SUPPLIER_BANKS_FORM, "SUPPLIER_BANKS");
            return supls;
        }
    }
}
