using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.CustomerDAO
{
    public class CustomerDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<bool> Add(Customers customers)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_CUSTOMER, customers.Id, customers.Name, customers.ClientInCharge, customers.Phone, customers.Email);

            return await data.Insert(sqlQuery) > 0;
        }

        public static async Task<DataTable> GetCusForCbo()
        {
            return await data.GetDataAsync(QueryStatement.GET_CUSTOMER_FOR_CBO, "CUS_FOR_CBO");
        }
    }
}
