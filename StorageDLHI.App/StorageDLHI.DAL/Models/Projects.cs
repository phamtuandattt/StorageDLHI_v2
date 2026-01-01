using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Projects
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProjectNo { get; set; }
        public string ProductInfo { get; set; }
        public decimal Weight { get; set; }

        public Guid CustomerId { get; set; }
        //public Guid AddressId { get; set; }
    }
}
