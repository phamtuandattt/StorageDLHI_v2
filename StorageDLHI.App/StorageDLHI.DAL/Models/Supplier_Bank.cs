using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Supplier_Bank
    {
        public Guid Id { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankBeneficial { get; set; }
    }
}
