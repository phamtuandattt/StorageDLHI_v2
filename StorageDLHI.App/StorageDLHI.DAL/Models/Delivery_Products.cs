using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Delivery_Products
    {
        public Guid Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryDay { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int Delivery_Total_Qty { get; set; }
        public Guid WarehosueId { get; set; }
        public Guid Staff_Id { get; set; }
    }

    public class Delivery_Product_Detail
    {
        public Guid Id { get; set; }
        public Guid Delivery_Product_Id { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
    }
}
