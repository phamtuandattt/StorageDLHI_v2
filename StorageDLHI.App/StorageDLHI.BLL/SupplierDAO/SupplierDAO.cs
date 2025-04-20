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

        public static async Task<bool> InsertSupplier(Suppliers model)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_SUPPLIERS, model.Id, model.Name, model.Cert, model.Email, model.Phone,
                model.Viettat, model.Address);

            return await data.Insert(sqlQuery) > 0;
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
            var dtBanks = new DataTable();
            dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_ID); // 0
            dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_SUPPLIER_ID); // 1
            dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_BANK_ACCOUNT); // 2
            dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_NAME); // 3
            dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_BENEFICIAL); // 4
            dtBanks.Columns.Add("IS_ADD", typeof(bool)); // 5

            var dt = await data.GetDataAsync(string.Format(QueryStatement.GET_BANK_BY_SUPPLIER, supId), "BANK");

            foreach (DataRow row in dt.Rows)
            {
                DataRow dataRow = dtBanks.NewRow();
                dataRow[0] = row[0];
                dataRow[1] = row[1];
                dataRow[2] = row[2];
                dataRow[3] = row[3];
                dataRow[4] = row[4];
                dataRow[5] = false;

                dtBanks.Rows.Add(dataRow);
            }

            return dtBanks;
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
