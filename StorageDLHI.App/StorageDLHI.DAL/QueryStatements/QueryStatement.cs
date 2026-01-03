using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.QueryStatements
{
    public class QueryStatement
    {
        // Display combobox property
        public const string PROPERTY_FOR_ORI_TYPE_STAND_VALUE = "ValueMember";
        public const string PROPERTY_FOR_ORI_TYPE_STAND_DISPLAY = "DisplayMember";

        // Supplier
        public const string GET_SUPPLIERS = "EXEC GET_SUPPLIERS";
        public const string INSERT_SUPPLIERS = "EXEC INSERT_SUPPLIER '{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}'";
        //public const string UPDATE_SUPPLIER = "UPDATE SUPPLIERS SET NAME = N'{0}', CERT = '{1}', EMAIL = N'{2}', PHONE = '{3}', VIETTAT = N'{4}', ADDRESS = N'{5}' WHERE ID = '{6}'";
        public const string UPDATE_SUPPLIER = "EXEC UPDATE_SUPPLIER '{6}', N'{0}', '{1}', N'{2}', '{3}', N'{4}', N'{5}'";
        //public const string DELETE_SUPPLIER = "DELETE SUPPLIERS WHERE ID = '{0}'";
        public const string DELETE_SUPPLIER = "EXEC DELETE_SUPPLIER '{0}'";
        public const string PROPERTY_SUPPLIER_ID = "ID";
        public const string PROPERTY_SUPPLIER_NAME = "NAME";
        public const string PROPERTY_SUPPLIER_CERT = "CERT";
        public const string PROPERTY_SUPPLIER_EMAIL = "EMAIL";
        public const string PROPERTY_SUPPLIER_PHONE = "PHONE";
        public const string PROPERTY_SUPPLIER_VIETTAT = "VIETTAT";
        public const string PROPERTY_SUPPLIER_ADDRESS = "ADDRESS";
        public const string GET_SUPPLIER = "EXEC GET_SUPPLIER_BY_ID '{0}'";

        // Supplier banks
        public const string GET_SUPPLIER_BANKS = "EXEC GET_SUPPLIER_BANK";
        public const string GET_SUPPLIER_BANKS_FORM = "EXEC GET_SUPPLIER_BANK_FORM";
        public const string GET_BANK_BY_SUPPLIER = "EXEC GET_SUPPLIER_BANK_BY_ID '{0}'";
        public const string UPDATE_BANK = "EXEC UPDATE_SUPPLIER_BANK '{3}', '{0}', N'{1}', N'{2}'";
        public const string DELETE_BANK = "EXEC DELETE_SUPPLIER_BANK '{0}'";
        public const string PROPERTY_SUPPLIER_BANK_ID = "ID";
        public const string PROPERTY_SUPPLIER_BANK_SUPPLIER_ID = "SUPPLIER_ID";
        public const string PROPERTY_SUPPLIER_BANK_BANK_ACCOUNT = "BANK_ACCOUNT";
        public const string PROPERTY_SUPPLIER_BANK_NAME = "BANK_NAME";
        public const string PROPERTY_SUPPLIER_BANK_BENEFICIAL = "BANK_BENEFICIAL";

        // Customer
        public const string INSERT_CUSTOMER = "INSERT INTO CUSTOMERS (ID, CUSTOMER_NAME, CLIENT_IN_CHARGE, CUSTOMER_PHONE, CUSTOMER_EMAIL) VALUES ('{0}', N'{1}', N'{2}', '{3}', N'{4}')";
        public const string GET_CUSTOMER_FOR_CBO = "SELECT ID, CUSTOMER_NAME FROM CUSTOMERS";
        public const string PROPERTY_CUSTOMER_ID = "ID";
        public const string PROPERTY_CUSTOMER_NAME = "CUSTOMER_NAME";

        // Material Origins
        public const string GET_ORIGINS = "EXEC GET_ORIGINS";
        public const string INSERT_ORIGIN = "EXEC INSERT_ORIGIN '{0}', '{1}', N'{2}'";
        public const string UPDATE_ORIGIN = "EXEC UPDATE_ORIGIN '{2}', '{0}', N'{1}'";
        public const string PROPERTY_ORIGIN_CODE = "ORIGIN_CODE";
        public const string PROPERTY_ORIGIN_NAME = "ORIGIN_NAME";


        // Material Types
        public const string INSERT_MATERIAL_TYPE = "INSERT INTO MATERIAL_TYPES VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_TYPE = "UPDATE MATERIAL_TYPES SET TYPE_CODE = '{0}', TYPE_DES = N'{1}' WHERE ID = '{2}'";
        public const string PROPERTY_M_TYPE_CODE = "TYPE_CODE";
        public const string PROPERTY_M_TYPE_DES = "TYPE_DES";
        public const string GET_TYPES = "SELECT *FROM MATERIAL_TYPES";
        public const string GET_MATERIAL_OF_TYPE = "SELECT *FROM MATERIAL_TYPE_DETAIL";
        public const string GET_ITEMS_OF_MATERIAL_TYPE = "SELECT *FROM MATERIAL_TYPE_DETAIL_ITEM";

        // Material Standards
        public const string GET_MATERIAL_STANDARDS = "SELECT *FROM MATERIAL_STANDARD";
        public const string INSERT_MATEIAL_STANDARD = "INSERT INTO MATERIAL_STANDARD VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_STANDARD = "UPDATE MATERIAL_STANDARD SET STANDARD_CODE = '{0}', STANDARD_DES = N'{1}' WHERE ID = '{2}'";
        public const string PROPERTY_M_STANDARD_CODE = "STANDARD_CODE";
        public const string PROPERTY_M_STANDARD_DES = "STANDARD_DES";

        // Tax
        public const string GET_TAXS = "SELECT *FROM TAX";
        public const string INSERT_TAX = "INSERT INTO TAX VALUES ('{0}', N'{1}', {2})";
        public const string UPDATE_TAX = "UPDATE TAX SET TAX_PERCENT = N'{0}', TAX_VALUE = {1} WHERE ID = '{2}'";
        public const string PROPERTY_TAX_ID = "ID";
        public const string PROPERTY_TAX_PERCENT = "TAX_PERCENT";
        public const string GET_TAX_CUSTOM = "SELECT ID, CONCAT(TAX_PERCENT, N' ~ ', TAX_VALUE) AS VALUE FROM TAX";
        public const string PROPERTY_TAX_CUSTOM_VALUE = "VALUE";

        // Formula
        public const string GET_FORMULAR_CAL = "SELECT ID, FORMULA_TEXT FROM MONEY_CALCULATE_FORMULA";
        public const string GET_FORMUAL_PARA = "SELECT FORMULA_TEXT, FORMULA_PARAS FROM MONEY_CALCULATE_FORMULA";
        public const string PROPERTY_FORMULA_ID = "ID";
        public const string PROPERTY_FORMULA_TEXT = "FORMULA_TEXT";
        public const string PROPERTY_FORMULA_CAL = "FORMULA_CALCULATE";
        public const string PROPERTY_FORMULA_PARAS = "FORMULA_PARAS";
        public const string PRICE_PARA = "PRICE";
        public const string QTY_PARA = "QTY";
        public const string NETCASH_PARA = "NET_CASH";
        public const string TAXVALUE_PARA = "TAX";

        // Unit
        public const string GET_UNIT = "SELECT *FROM UNITS";
        public const string INSERT_UNIT = "INSERT INTO UNITS VALUES ('{0}', N'{1}')";
        public const string UPDATE_UNIT = "UPDATE UNITS SET UNIT_CODE = N'{0}' WHERE ID = '{1}'";
        public const string PROPERTY_UNIT_ID = "ID";
        public const string PROPERTY_UNIT_CODE = "UNIT_CODE";

        // Cost
        public const string GET_COST = "SELECT *FROM COST";
        public const string INSERT_COST = "INSERT INTO COST VALUES ('{0}', N'{1}', N'{2}', {3}, N'{4}')";
        public const string UPDATE_COST = "UPDATE COST SET COST_NAME = N'{0}', CURRENCY_CODE = N'{2}', CURRENCY_VALUE = '{3}', CURRENCY = N'{4}' WHERE ID = '{1}'";
        public const string UPDATE_EXCHANGE_RATE_COST = "UPDATE COST SET CURRENCY_VALUE = {0} WHERE ID = '{1}'";
        public const string PROPERTY_COST_ID = "ID";
        public const string PROPERTY_COST_NAME = "COST_NAME";
        public const string PROPERTY_CURRENCY = "CURRENCY";
        public const string PROPERTY_CURRENCY_CODE = "CURRENCY_CODE";
        public const string PROPERTY_CURRENCY_VALUE = "CURRENCY_VALUE";

        // Product
        public const string ADD_PROD = "INSERT INTO PRODUCTS \r\n(ID, \r\nPRODUCT_NAME, \r\nPRODUCT_DES_2, \r\nPRODUCT_CODE, \r\nPRODUCT_MATERIAL_CODE, \r\nPICTURE_LINK, \r\nPICTURE, \r\nA_THINHNESS, \r\nB_DEPTH, \r\nC_WIDTH, \r\nD_WEB,\r\nE_FLAG, \r\nF_LENGTH, \r\nG_WEIGHT, \r\nUSED_NOTE,\r\nUNIT_ID,\r\nORIGIN_ID,\r\nM_TYPE_ID,\r\nSTANDARD_ID) \r\nVALUES \r\n('{0}',\r\nN'{1}', \r\n'{2}', \r\n'{3}',\r\n'{4}',\r\nN'{5}',\r\n(SELECT *FROM OPENROWSET(BULK N'{6}', SINGLE_BLOB) AS IMAGE), \r\n'{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', N'{15}', '{16}', '{17}', '{18}')";
        public const string ADD_PROD_NO_IMAGE = "INSERT INTO PRODUCTS \r\n(ID, \r\nPRODUCT_NAME, \r\nPRODUCT_DES_2, \r\nPRODUCT_CODE, \r\nPRODUCT_MATERIAL_CODE, \r\nA_THINHNESS, \r\nB_DEPTH, \r\nC_WIDTH, \r\nD_WEB,\r\nE_FLAG, \r\nF_LENGTH, \r\nG_WEIGHT, \r\nUSED_NOTE,\r\nUNIT_ID,\r\nORIGIN_ID,\r\nM_TYPE_ID,\r\nSTANDARD_ID) \r\nVALUES \r\n('{0}',\r\nN'{1}', \r\n'{2}', \r\n'{3}',\r\n'{4}', \r\n'{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', N'{13}', '{14}', '{15}', '{16}')";
        public const string GET_PROD = "SELECT *FROM PRODUCTS WHERE ID = '{0}'";
        public const string PROPERTY_PROD_ID = "ID";
        public const string PROPERTY_PROD_NAME = "PRODUCT_NAME";
        public const string PROPERTY_PROD_DES_2 = "PRODUCT_DES_2";
        public const string PROPERTY_PROD_CODE = "PRODUCT_CODE";
        public const string PROPERTY_PROD_MATERIAL_CODE = "PRODUCT_MATERIAL_CODE";
        public const string PROPERTY_PROD_PICTURE_LINK = "PICTURE_LINK";
        public const string PROPERTY_PROD_PICTURE = "PICTURE";
        public const string PROPERTY_PROD_A = "A_THINHNESS";
        public const string PROPERTY_PROD_B = "B_DEPTH";
        public const string PROPERTY_PROD_C = "C_WIDTH";
        public const string PROPERTY_PROD_D = "D_WEB";
        public const string PROPERTY_PROD_E = "E_FLAG";
        public const string PROPERTY_PROD_F = "F_LENGTH";
        public const string PROPERTY_PROD_G = "G_WEIGHT";
        public const string PROPERTY_PROD_USAGE = "USED_NOTE";
        public const string PROPERTY_PROD_UNIT_ID = "UNIT_ID";
        public const string PROPERTY_PROD_ORIGIN_ID = "ORIGIN_ID";
        public const string PROPERTY_PROD_M_TYPE_ID = "M_TYPE_ID";
        public const string PROPERTY_PROD_STANDARD_ID = "STANDARD_ID";
        public const string PROPERTY_PROD_UNIT_CODE = "UNIT_CODE";
        public const string UPDATE_PROD = "UPDATE PRODUCTS\r\nSET PRODUCT_NAME = N'{0}', PRODUCT_DES_2 = '{1}', PRODUCT_CODE = '{2}', PRODUCT_MATERIAL_CODE = '{3}',\r\nPICTURE_LINK = N'{4}', PICTURE = (SELECT *FROM OPENROWSET(BULK N'{5}', SINGLE_BLOB) AS IMAGE),\r\nA_THINHNESS = '{6}', B_DEPTH = '{7}', C_WIDTH = '{8}', D_WEB ='{9}', E_FLAG = '{10}', F_LENGTH = '{11}', G_WEIGHT = '{12}',\r\nUSED_NOTE = N'{13}', UNIT_ID = '{14}', \tORIGIN_ID = '{15}', M_TYPE_ID = '{16}', STANDARD_ID = '{17}'\r\nWHERE ID = '{18}'";
        public const string GET_PRODUCTS_FOR_CREATE_MPR = "EXEC GET_PRODUCTS";
        public const string GET_ITEM_NUMBER_OF_MATERIAL_TYPE = "EXEC GET_ITEM_NUMBER_OF_MATERIAL_TYPE '{0}'";

        // MPRs
        public const string GET_MPR_DETAILs = "SELECT *FROM MPR_DETAIL";
        //public const string GET_MPR_DETAIL_BY_MPR_ID = "SELECT *FROM MPR_DETAIL WHERE MPR_ID = '{0}'";
        public const string ADD_MPR = "SET DATEFORMAT DMY INSERT INTO MPRS (ID, MPR_NO, MPR_WO_NO, MPR_PROJECT_NAME, MPR_REV_TOTAL, \r\nMPR_CREATE_DATE, MPR_EXPECTED_DELIVERY_DATE, MPR_PREPARED, MPR_REVIEWED, MPR_APPROVED, STAFF_ID, IS_MAKE_PO)\r\nVALUES ('{0}', '{1}', '{2}', '{3}', N'{4}', N'{5}', N'{6}', N'{7}', '{8}', '{9}', '{10}', '{11}')";
        public const string ADD_MPR_DETAIL = "";
        public const string DELETE_MPR = "DELETE FROM MPRS WHERE ID = '{0}'";
        public const string GET_MPR_DETAIL_FORM = "SELECT *FROM MPR_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string GET_MPRs = "SELECT *FROM MPRS";
        public const string UPDATE_MPR_INFO = "SET DATEFORMAT DMY UPDATE MPRS SET MPR_EXPECTED_DELIVERY_DATE = '{0}', MPR_PREPARED = N'{1}', MPR_REVIEWED = N'{2}', MPR_APPROVED = N'{3}' WHERE ID = '{4}'";
        public const string GET_MPR_DETAIL_BY_ID = "EXEC GET_MPR_DETAIL '{0}'";
        public const string UPDATE_MPRS_IS_MAKE_PO = "UPDATE MPRS SET IS_MAKE_PO = '{0}' WHERE ID = '{1}'";
        public const string GET_MPRS_FOR_MAKE_PO = "SELECT *FROM MPRS WHERE IS_MAKE_PO = 'FALSE'";
        public const string GET_MPR_FOR_EXPORT = "EXEC GET_MPR_FOR_EXPORT '{0}'";
        public const string PROPERTY_MPR_MPR_CREATE_DATE = "MPR_CREATE_DATE";
        public const string PROPERTY_MPR_MPR_EXPECTED_DELIVERY_DATE = "MPR_EXPECTED_DELIVERY_DATE";
        public const string PROPERTY_MPR_MPR_NO = "MPR_NO";
        public const string PROPERTY_MPR_MPR_WO_NO = "MPR_WO_NO";
        public const string PROPERTY_MPR_MPR_PROJECT_NAME = "MPR_PROJECT_NAME";
        public const string PROPERTY_MPR_MPR_PREPARED = "MPR_PREPARED";
        public const string PROPERTY_MPR_MPR_REVIEWED = "MPR_REVIEWED";
        public const string PROPERTY_MPR_MPR_APPROVED = "MPR_APPROVED";
        public const string GET_NUMBER_OF_MPR_CREATED = "EXEC GET_NUMBER_OF_MPR_CREATED '{0}'";

        // PO
        public const string INSERT_PO = "SET DATEFORMAT DMY " +
            "INSERT INTO POS VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', N'{5}', '{6}', '{7}', N'{8}', N'{9}', N'{10}', N'{11}', N'{12}', N'{13}', {14}, " +
            //"'{15}', '{16}', " +
            "'{15}', '{16}', '{17}')  ";
        public const string GET_PO_DETAIL_FORM = "SELECT *FROM PO_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string GET_PO_DETAILS = "SELECT *FROM PO_DETAIL";
        public const string DELETE_PO_BY_ID = "DELETE FROM POS WHERE ID = '{0}'";
        public const string GET_PO_DETAIL_BY_PO_ID = "EXEC GET_PO_DETAIL '{0}'";
        public const string GET_PO_DETAIL_BY_PO_ID_FOR_IMPORT = "EXEC GET_PO_DETAIL_FOR_IMPORT '{0}'";
        public const string GET_POS = "EXEC GET_POS";
        public const string PROPERTY_PO_NO = "PO_NO";
        public const string PROPERTY_PO_MPR_NO = "PO_MPR_NO";
        public const string PROPERTY_PO_WO_NO = "PO_WO_NO";
        public const string PROPERTY_PO_PROJECT_NAME = "PO_PROJECT_NAME";
        public const string PROPERTY_PO_REV_TOTAL = "PO_REV_TOTAL";
        public const string PROPERTY_PO_CREATE_DATE = "PO_CREATE_DATE";
        public const string PROPERTY_PO_EXPECTED_DELIVERY_DATE = "PO_EXPECTED_DELIVERY_DATE";
        public const string PROPERTY_PO_PREPARED = "PO_PREPARED";
        public const string PROPERTY_PO_REVIEWED = "PO_REVIEWED";
        public const string PROPERTY_PO_AGREEMENT = "PO_AGREEMENT";
        public const string PROPERTY_PO_APPROVED = "PO_APPROVED";
        public const string PROPERTY_PO_PAYMENT_TERM = "PO_PAYMENT_TERM";
        public const string PROPERTY_PO_DISPATCH_BOX = "PO_PLACE_OF_CONTRY";
        public const string PROPERTY_PO_TOTAL_AMOUNT = "PO_TOTAL_AMOUNT";
        public const string GET_POS_FOR_IMPORT_PRODUCT = "GET_POS_FOR_IMPORT_PRODUCT";
        public const string PROPERTY_PO_PO_CREATE_DATE = "PO_CREATE_DATE";
        public const string PROPERTY_PO_PO_EXPECTED_DELIVERY_DATE = "PO_EXPECTED_DELIVERY_DATE";
        public const string UPDATE_PO_IS_IMPORTED = "UPDATE POS SET IS_IMPORTED = '{0}' WHERE ID = '{1}'";

        public const string PROPERTY_PO_DETAIL_REMARKS = "PO_REMARKS";
        public const string PROPERTY_PO_DETAIL_RECEVIE = "PO_RECEVIE";

        // Warehouse
        public const string INSERT_WAREHOUSE = "INSERT INTO WAREHOUSES VALUES ('{0}', '{1}', N'{2}', N'{3}')";
        public const string PROPERTY_WAREHOUSE_DETAIL_ID = "WAREHOUSE_ID";
        public const string PROPERTY_WAREHOUSE_NAME = "WAREHOUSE_NAME";
        public const string PROPERTY_WAREHOUSE_ID = "ID";
        public const string PROPERTY_WAREHOUSE_CODE = "WAREHOUSE_CODE";
        public const string GET_WAREHOUSE_FOR_CBO = "SELECT  ID, WAREHOUSE_NAME FROM WAREHOUSES";
        public const string GET_WAREHOUSE_DETAIL_FORM = "SELECT *FROM WAREHOUSE_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string GET_WAREHOUSE_DETAILS = "SELECT *FROM WAREHOUSE_DETAIL";
        public const string GET_WAREHOUSE_DETAIL_BY_WAREHOUSE_ID = "EXEC GET_WAREHOUSE_DETAILBY_ID '{0}'";
        public const string GET_WAREHOUSES = "SELECT *FROM WAREHOUSES";
        public const string UPDATE_OR_ADD_PRODUCT_EXIST_WAREHOUES = "UpdateWarehouseStock";
        public const string WAREHOUSE_TABLE_TYPE = "WarehouseImportType";
        public const string GET_WAREHOUSE_FOR_COMBOBOX_EXPORT_PROD = "SELECT *FROM WAREHOUSES WHERE ID != '{0}'";
        public const string UPDATE_QTY_OF_PROD_AFTER_EXPORTED = "UPDATE WAREHOUSE_DETAIL SET PRODUCT_IN_STOCK = PRODUCT_IN_STOCK - {0} WHERE WAREHOUSE_ID = '{1}' AND PRODUCT_ID = '{2}'";


        public const string INPUT_FOR_PROC = "Items";
        public const string OUTPUT_FOR_PROC = "ResultCode";


        // Import 
        public const string PROPERTY_IMPORT_ID = "";
        public const string GET_IMPORT_DETAILS = "SELECT *FROM IMPORT_PRODUCT_DETAIL";
        public const string GET_IMPORT_DETAIL_FORM = "SELECT *FROM IMPORT_PRODUCT_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string ADD_IMPORT_PRODUCT = "SET DATEFORMAT DMY  INSERT INTO IMPORT_PRODUCTS VALUES('{0}', '{1}', {2}, {3}, {4}, {5}, '{6}',  N'{7}')";
        public const string DELETE_IMPORT_PRODUCT = "DELETE FROM IMPORT_PRODUCTS WHERE ID = '{0}'";
        public const string GET_IMPORTS = "EXEC GET_IMPORTS";
        public const string GET_IMPORT_DETAIL_BY_ID = "EXEC GET_IMPORT_DETAIL_BY_ID '{0}'";
        public const string PROPERTY_IMPORT_PRODUCT_FROM_PO_NO = "FROM_PO_NO";
        public const string PROPERTY_IMPORT_PRODUCT_IMPORT_DATE = "IMPORT_DATE";
        public const string PROPERTY_IMPORT_PRODUCT_STAFF_NAME = "STAFF_NAME";

        // Export
        public const string GET_DELIVERY_DETAIL_FORM = "SELECT *FROM DELIVERY_PRODUCT_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string PROPERTY_DELIVERY_DETAIL_ID = "ID";
        public const string PROPERTY_DELIVERY_DETAIL_PRODUCT_ID = "DELIVERY_PRODUCT_ID";
        public const string PROPERTY_DELIVERY_DETAIL_PRODUCT_PRODUCT_ID = "PRODUCT_ID";
        public const string PROPERTY_DELIVERY_DETAIL_FROM_WAREHOUSE_ID = "TO_WAREHOUSE_ID";
        public const string PROPERTY_DELIVERY_DETAIL_QTY = "QTY";
        public const string ADD_DELIVERY_PRODUCTS = "SET DATEFORMAT DMY INSERT INTO DELIVERY_PRODUCTS VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, '{6}', '{7}')";
        public const string DELETE_DELIVERY_PRODUCTS = "DELETE FROM DELIVERY_PRODUCTS WHERE ID = '{0}'";
        public const string GET_DELIVERY_PRODUCT_DETAIL = "SELECT *FROM DELIVERY_PRODUCT_DETAIL";

        // Staff
        public const string PROPERTY_STAFF_ID = "ID";
        public const string PROPERTY_STAFF_CODE = "STAFF_CODE";
        public const string PROPERTY_STAFF_NAME = "STAFF_NAME";
        public const string PROPERTY_STAFF_DEVICE_NAME = "STAFF_DEVICE_NAME";
        public const string GET_STAFF_BY_DEVICE = "SELECT *FROM STAFFS WHERE STAFF_DEVICE_NAME = '{0}'";
        public const string GET_STAFF_LOGIN_BY_ID = "SELECT STAFFS.ID, DEPARMENTS.DEP_CODE FROM STAFFS, DEPARMENTS WHERE STAFFS.DEPARMENT_ID = DEPARMENTS.ID AND STAFFS.ID = '{0}'";

        // Deparment
        public const string PROPERTY_DEPARTMENT_DEP_CODE = "DEP_CODE";
        public const string GET_LIST_STAFFS_OF_DEP = "EXEC GET_LIST_STAFFS_OF_DEPARMENT '{0}'";
        public const string GET_DEPS = "SELECT *FROM DEPARMENTS";
        public const string PROPERTY_FOR_COMBO_DEP_ID = "DEP_ID";
        public const string PROPERTY_FOR_COMBO_DEP_NAME_CODE = "DEP_NAME_CODE";

        // Project
        public const string INSERT_PROJECT = "INSERT INTO PROJECTS VALUES('{0}', N'{1}', N'{2}', N'{3}', N'{4}', {5}, N'{6}', '{7}')";
    }
}
