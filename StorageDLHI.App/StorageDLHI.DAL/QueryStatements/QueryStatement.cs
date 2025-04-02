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
        public const string INSERT_SUPPLIERS = "INSERT INTO SUPPLIERS VALUES ('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}')";
        public const string UPDATE_SUPPLIER = "UPDATE SUPPLIERS SET NAME = N'{0}', CERT = '{1}', EMAIL = N'{2}', PHONE = '{3}', VIETTAT = N'{4}', ADDRESS = N'{5}' WHERE ID = '{6}'";
        public const string DELETE_SUPPLIER = "DELETE SUPPLIERS WHERE ID = '{0}'";

        // Supplier banks
        public const string GET_SUPPLIER_BANKS = "SELECT *FROM SUPPLIER_BANKS";
        public const string GET_SUPPLIER_BANKS_FORM = "SELECT *FROM SUPPLIER_BANKS WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string GET_BANK_BY_SUPPLIER = "SELECT *FROM SUPPLIER_BANKS WHERE SUPPLIER_ID = '{0}'";
        public const string UPDATE_BANK = "UPDATE SUPPLIER_BANKS SET BANK_ACCOUNT = '{0}', BANK_NAME = N'{1}', BANK_BENEFICIAL = N'{2}' WHERE ID = '{3}'";

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

        // Tax
        public const string GET_TAXS = "SELECT *FROM TAX";
        public const string INSERT_TAX = "INSERT INTO TAX VALUES ('{0}', N'{1}')";
        public const string UPDATE_TAX = "UPDATE TAX SET TAX_PERCENT = N'{0}' WHERE ID = '{1}'";

        // Unit
        public const string GET_UNIT = "SELECT *FROM UNITS";
        public const string INSERT_UNIT = "INSERT INTO UNITS VALUES ('{0}', N'{1}')";
        public const string UPDATE_UNIT = "UPDATE UNITS SET UNIT_CODE = N'{0}' WHERE ID = '{1}'";

        // Cost
        public const string GET_COST = "SELECT *FROM COST";
        public const string INSERT_COST = "INSERT INTO COST VALUES ('{0}', N'{1}')";
        public const string UPDATE_COST = "UPDATE COST SET COST_NAME = N'{0}' WHERE ID = '{1}'";

    }
}
