using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Staffs
    {
        public Guid Id { get; set; }
        public string Staff_Code { get; set; }
        public string Staff_Pwd { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid Staff_RoleId { get; set; }

    }

    public class StaffLogin
    {
        public Guid Id { get; set; }
        public string DepCode { get; set; }
    }
}
