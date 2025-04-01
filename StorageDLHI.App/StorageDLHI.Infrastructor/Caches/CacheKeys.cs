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
    }
}
