using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Material_Types
    {
        public Guid Id { get; set; }
        public string Type_Code { get; set; }
        public string Type_Des { get; set; }
    }
}
