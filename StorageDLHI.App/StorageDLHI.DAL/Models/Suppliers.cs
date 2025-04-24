using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Suppliers
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cert { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Viettat { get; set; }
        public string Address { get; set; }

        public Guid Bank_Id { get; set; }
    }
}
