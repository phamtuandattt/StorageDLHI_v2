using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.Infrastructor.Caches
{
    public static class CacheKeys
    {
        public const string ORIGIN_DATATABLE_ALLORIGIN = "Origin.DataTable.AllOrigin";
        public const string MATERIAL_TYPE_DATATABLE_ALLTYPE = "MaterialType.DataTable.AllType";
        public const string STANDARD_DATATABLE_ALLSTANDARD = "Standard.DataTable.AllStandard";

        public const string TAX_DATATABLE_ALLTAX = "Tax.DataTable.AllTax";
        public const string UNIT_DATATABLE_ALLUNIT = "Unit.DataTable.AllUnit";
        public const string COST_DATATABLE_ALLCOST = "Cost.DataTable.AllCost";

        public const string SUPPLIER_DATATABLE_ALL_SUPPLIER = "Supplier.DataTable.AllSupplier";
        public const string BANK_DETAIL_SUPPLIER_ID = "Bank.Detail.Supplier.Id.{0}";
        public const string SUPPLIER_BANK_DATATABLE_ALL_SUPPLIER_BANK = "SupplierBank.DataTable.AllSupplierBanks";

        public const string PRODUCT_DATATABLE_ALL_PRODS_FOR_EPR = "Product.DataTable.AllProForMpr";
        public const string PRODCT_DETAIL_BY_ID = "Product.Detail.Product.Id.{0}";

        public const string MPRS_DATATABLE_ALL_MPRS = "Mprs.DataTable.AllMprs";
        public const string MPR_DETAIL_BY_ID = "Mprs.Detail.Mpr.ID.{0}";

        public const string POS_DATATABLE_ALL_PO = "Po.DataTable.AllPos";
        public const string PO_DETAL_BY_ID = "Po.Detail.Po.ID.{0}";

        public const string IMPORT_PRODUCT_DATATABLE_ALL = "ImportProduct.DataTable.AllImportProduct";
        public const string IMPORT_PRODUCT_DETIAL_BY_ID = "ImportProduct.Detail.ImportProduct.ID.{0}";
    }
}
