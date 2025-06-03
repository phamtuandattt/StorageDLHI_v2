using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class MoneyCalculateFormula
    {
        public Guid ID { get; set; }
        public string FormulaText {  get; set; }
        public string FormulaCalculate { get; set; }
        public string FormulaParas { get; set; }

    }
}
