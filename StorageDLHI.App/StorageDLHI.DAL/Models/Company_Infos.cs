using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Company_Infos
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cert {  get; set; }
        public string Phone { get; set; }
    }
}
