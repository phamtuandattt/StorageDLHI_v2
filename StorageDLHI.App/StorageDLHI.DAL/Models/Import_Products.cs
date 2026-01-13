using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Import_Products
    {
        public Guid Id { get; set; }
        public string FromPONo { get; set; }
        public DateTime ImportDate { get; set; }
        public int ImportDay { get; set; }
        public int ImportMonth { get; set; }
        public int ImportYear { get; set; }
        public Int32 Import_Total_Qty { get; set; }
        public Guid Staff_Id { get; set; }
        public Guid Project_Id {  get; set; }
    }

    public class Import_Product_Detail
    {
        public Guid Id { get; set; }
        public Guid Import_Product_Id { get; set; }
        public Guid Product_Id { get; set; }
        public Guid WarehouseId { get; set; }
        public int Qty { get; set; }
    }
}
