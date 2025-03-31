using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.QueryStatements
{
    public class QueryStatement
    {
        // Supplier
        public const string GET_SUPPLIERS = "SELECT *FROM SUPPLIERS";
        public const string INSERT_SUPPLIERS = "INSERT INTO SUPPLIERS VALUES ('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}', '{7}')";

        // Supplier banks
        public const string GET_SUPPLIER_BANKS = "select *from SUPPLIER_BANKS";

        // Material Origins
        public const string GET_ORIGINS = "SELECT *FROM ORIGINS";
        public const string INSERT_ORIGIN = "INSERT INTO ORIGINS VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_ORIGIN = "UPDATE ORIGINS SET ORIGIN_CODE = '{0}', ORIGIN_NAME = N'{1}' WHERE ID = '{2}'";

        // Material Types
        public const string GET_MATERIAL_TYPES = "SELECT *FROM MATERIAL_TYPES";
        public const string INSERT_MATERIAL_TYPE = "INSERT INTO MATERIAL_TYPES VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_TYPE = "UPDATE MATERIAL_TYPES SET TYPE_CODE = '{0}', TYPE_DES = N'{1}' WHERE ID = '{2}'";

        // Material Standards
        public const string GET_MATERIAL_STANDARDS = "SELECT *FROM MATERIAL_STANDARD";
        public const string INSERT_MATEIAL_STANDARD = "INSERT INTO MATERIAL_STANDARD VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_STANDARD = "UPDATE MATERIAL_STANDARD SET STANDARD_CODE = '{0}', STANDARD_DES = N'{1}' WHERE ID = '{2}'";
    }
}
