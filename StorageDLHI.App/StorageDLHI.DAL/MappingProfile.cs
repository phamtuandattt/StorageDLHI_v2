using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL
{
    public static class MappingProfile
    {
        public static Suppliers MapSupplier(SqlDataReader reader)
        {
            return new Suppliers
            {
                Id = reader.GetGuid(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_ID)),
                Name = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_NAME)),
                Cert = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_CERT)),
                Email = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_EMAIL)),
                Phone = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_PHONE)),
                Viettat = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_VIETTAT)),
                Address = reader.GetString(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_ADDRESS)),
                Bank_Id = reader.GetGuid(reader.GetOrdinal(QueryStatement.PROPERTY_SUPPLIER_BANK_ID)),
            };
        }
    }
}
