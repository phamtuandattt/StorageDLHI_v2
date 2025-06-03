using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class FormulaParameter
    {
        public Guid ID { get; set; }
        public string ParaName { get; set; }
        public float ParaValue { get; set; }
        public string ParaDescription { get; set; }
    }
}
