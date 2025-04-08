using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string GET_SUPPLIERS = "SELECT *FROM SUPPLIERS";
        public const string INSERT_SUPPLIERS = "INSERT INTO SUPPLIERS VALUES ('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}')";
        public const string UPDATE_SUPPLIER = "UPDATE SUPPLIERS SET NAME = N'{0}', CERT = '{1}', EMAIL = N'{2}', PHONE = '{3}', VIETTAT = N'{4}', ADDRESS = N'{5}' WHERE ID = '{6}'";
        public const string DELETE_SUPPLIER = "DELETE SUPPLIERS WHERE ID = '{0}'";

        // Supplier banks
        public const string GET_SUPPLIER_BANKS = "SELECT *FROM SUPPLIER_BANKS";
        public const string GET_SUPPLIER_BANKS_FORM = "SELECT *FROM SUPPLIER_BANKS WHERE ID = '00000000-0000-0000-0000-000000000000'";
        public const string GET_BANK_BY_SUPPLIER = "SELECT *FROM SUPPLIER_BANKS WHERE SUPPLIER_ID = '{0}'";
        public const string UPDATE_BANK = "UPDATE SUPPLIER_BANKS SET BANK_ACCOUNT = '{0}', BANK_NAME = N'{1}', BANK_BENEFICIAL = N'{2}' WHERE ID = '{3}'";
        public const string DELETE_BANK = "DELETE SUPPLIER_BANKS WHERE ID = '{0}'";

        // Material Origins
        public const string GET_ORIGINS = "SELECT *FROM ORIGINS";
        public const string INSERT_ORIGIN = "INSERT INTO ORIGINS VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_ORIGIN = "UPDATE ORIGINS SET ORIGIN_CODE = '{0}', ORIGIN_NAME = N'{1}' WHERE ID = '{2}'";
        public const string PROPERTY_ORIGIN_CODE = "ORIGIN_CODE";
        public const string PROPERTY_ORIGIN_NAME = "ORIGIN_NAME";


        // Material Types
        public const string GET_MATERIAL_TYPES = "SELECT *FROM MATERIAL_TYPES";
        public const string INSERT_MATERIAL_TYPE = "INSERT INTO MATERIAL_TYPES VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_TYPE = "UPDATE MATERIAL_TYPES SET TYPE_CODE = '{0}', TYPE_DES = N'{1}' WHERE ID = '{2}'";
        public const string PROPERTY_M_TYPE_CODE = "TYPE_CODE";
        public const string PROPERTY_M_TYPE_DES = "TYPE_DES";

        // Material Standards
        public const string GET_MATERIAL_STANDARDS = "SELECT *FROM MATERIAL_STANDARD";
        public const string INSERT_MATEIAL_STANDARD = "INSERT INTO MATERIAL_STANDARD VALUES ('{0}', '{1}', N'{2}')";
        public const string UPDATE_MATERIAL_STANDARD = "UPDATE MATERIAL_STANDARD SET STANDARD_CODE = '{0}', STANDARD_DES = N'{1}' WHERE ID = '{2}'";
        public const string PROPERTY_M_STANDARD_CODE = "STANDARD_CODE";
        public const string PROPERTY_M_STANDARD_DES = "STANDARD_DES";

        // Tax
        public const string GET_TAXS = "SELECT *FROM TAX";
        public const string INSERT_TAX = "INSERT INTO TAX VALUES ('{0}', N'{1}')";
        public const string UPDATE_TAX = "UPDATE TAX SET TAX_PERCENT = N'{0}' WHERE ID = '{1}'";

        // Unit
        public const string GET_UNIT = "SELECT *FROM UNITS";
        public const string INSERT_UNIT = "INSERT INTO UNITS VALUES ('{0}', N'{1}')";
        public const string UPDATE_UNIT = "UPDATE UNITS SET UNIT_CODE = N'{0}' WHERE ID = '{1}'";
        public const string PROPERTY_UNIT_ID = "ID";
        public const string PROPERTY_UNIT_CODE = "UNIT_CODE";

        // Cost
        public const string GET_COST = "SELECT *FROM COST";
        public const string INSERT_COST = "INSERT INTO COST VALUES ('{0}', N'{1}')";
        public const string UPDATE_COST = "UPDATE COST SET COST_NAME = N'{0}' WHERE ID = '{1}'";

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

        // MPRs
        public const string GET_MPR_DETAILs = "SELECT *FROM MPR_DETAIL";
        public const string GET_MPR_DETAIL_BY_MPR_ID = "SELECT *FROM MPR_DETAIL WHERE MPR_ID = '{0}'";
        public const string ADD_MPR = "SET DATEFORMAT DMY INSERT INTO MPRS (ID, MPR_NO, MPR_WO_NO, MPR_PROJECT_NAME, MPR_REV_TOTAL, \r\nMPR_CREATE_DATE, MPR_EXPECTED_DELIVERY_DATE, MPR_PREPARED, MPR_REVIEWED, MPR_APPROVED)\r\nVALUES ('{0}', '{1}', '{2}', '{3}', N'{4}', N'{5}', N'{6}', N'{7}', '{8}', '{9}')";
        public const string ADD_MPR_DETAIL = "";
        public const string DELETE_MPR = "DELETE FROM MPRS WHERE ID = '{0}'";
        public const string GET_MPR_DETAIL_FORM = "SELECT *FROM MPR_DETAIL WHERE ID = '00000000-0000-0000-0000-000000000000'";

    }
}
