using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Material_Standards
    {
        public Guid Id { get; set; }
        public string Standard_Code { get; set; }
        public string Standard_Des { get; set; }
    }
}
