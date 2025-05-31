using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Taxs
    {
        public Guid Id { get; set; }
        public string Tax_Percent { get; set; }
        public float Tax_Value { get; set; }
    }
}
