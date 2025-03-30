using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Warehouses
    {
        public Guid Id { get; set; }
        public string Warehouse_Code { get; set; }
        public string Warehouse_Name { get; set; }
        public string Warehouse_Address { get; set; }
    }

    public class Warehouse_Detail
    {
        public Guid Id { get; set; }
        public Guid Warehosue_Id { get; set; }
        public Guid ProductId { get; set; }
        public int Product_In_Stock { get; set; }
    }
}
