using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Costs
    {
        public Guid Id { get; set; }
        public string Cost_Name { get; set; }
        public string Currency {  get; set; }
        public string Currency_code { get; set; }
        public decimal Currency_Value { get; set; }
    }
}
