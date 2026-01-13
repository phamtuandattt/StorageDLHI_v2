using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Pos
    {
        public Guid Id { get; set; }
        public string Po_No { get; set; }
        public string Po_Mpr_No { get; set; }
        public string Po_Wo_No { get; set; }
        public string Po_Project_Name { get; set; }
        public int Po_Rev_Total { get; set; }
        public DateTime Po_CreateDate { get; set; }
        public DateTime Po_Expected_Delivery_Date { get; set; }
        public string Po_Prepared {  get; set; }
        public string Po_Reviewed { get; set; }
        public string Po_Agrement { get; set; }
        public string Po_Approved { get; set; }
        public string Po_Payment_Term { get; set; }
        public string Po_Dispatch_Box { get; set; }
        public double Po_Total_Amount { get; set; }
        public bool IsImported { get; set; }
        

        public Guid SupplierId { get; set; }
        public Guid Staff_Id { get; set; }

        public Guid Project_Id { get; set; }

        public Guid ReviewBy { get; set; } 
        public Guid AgrementBy { get; set; }
        public Guid ApprovedBy { get; set; }
    }

    public class Po_Detail
    {
        public Guid Id { get; set; }
        public Guid Po_Id { get; set; }
        public Guid ProductId { get; set; }
        public int Po_Qty { get; set; }
        public Int64 Po_Price { get; set; }
        public Int64 Po_Amount { get; set; }
        public DateTime Req_Date { get; set; }
        public string Po_Recevie { get; set; }
        public string Po_Remarks { get; set; }

        public Guid CostId { get; set; }
        public Guid TaxId { get; set; }
    }
}
