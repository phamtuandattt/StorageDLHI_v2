using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.Infrastructor.Commons
{
    public static class Common
    {
        public static Int32 CheckOrReturnNumber(string numberString)
        {
            return !string.IsNullOrEmpty(numberString.Trim())
                && numberString.Trim().Length > 0
                ? Int32.Parse(numberString.Trim()) : 0;
        }
    }
}
