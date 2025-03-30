using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Mprs
    {
        public Guid Id { get; set; }
        public string Mpr_No { get; set; }
        public string Mpr_Wo_No { get; set; }
        public string Mpr_Project_Name_Code { get; set; }
        public string Mpr_Rev_Total { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Expected_Delivery_Date { get; set; }
        public string Mpr_Prepared {  get; set; }
        public string Mpr_Reviewed { get; set; }
        public string Mpr_Approved { get; set; }

        public Guid Staff_Id { get; set; }
    }

    public class Mpr_Detail
    {
        public Guid Id { get; set; }
        public Guid Mpr_Id { get; set; }
        public Guid Products_Id { get; set; }
        public int Mpr_Qty { get; set; }
        public string Usage { get; set; }
        public string MPS { get; set; }
        public int Rev { get; set; }
        public string DWG_BOQRECEIVE_DATE { get; set; }
        public string Issue_Date { get; set; }
        public string Req_Date { get; set; }
        public string Mpr_Remarks { get; set; }
    }
}
