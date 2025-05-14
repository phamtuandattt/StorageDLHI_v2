using StorageDLHI.DAL;
using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.StaffDAO
{
    public class StaffDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<Staffs> GetStaff(string deviceName)
        {
            return await data.GetEntityByIdAsync(string.Format(QueryStatement.GET_STAFF_BY_DEVICE, deviceName), MappingProfile.MapSaff);
        }
    }
}
