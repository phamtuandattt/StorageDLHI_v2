using StorageDLHI.DAL;
using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static async Task<StaffLogin> GetEmpLogin(Guid staffId)
        {
            return await data.GetEntityByIdAsync(string.Format(QueryStatement.GET_STAFF_LOGIN_BY_ID, staffId), MappingProfile.MappStaffLogin);
        }

        public static async Task<DataTable> GetStaffsOfDeP(Guid id)
        {
            return await data.GetDataAsync(string.Format(QueryStatement.GET_LIST_STAFFS_OF_DEP, id), "STAFFS_OF_DEP");
        }

        public static async Task<DataTable> GetDeps()
        {
            var dt = await data.GetDataAsync(QueryStatement.GET_DEPS, "DEPS");
            var dtForCbo = new DataTable();
            dtForCbo.Columns.Add(QueryStatement.PROPERTY_FOR_COMBO_DEP_ID);
            dtForCbo.Columns.Add(QueryStatement.PROPERTY_FOR_COMBO_DEP_NAME_CODE);

            foreach (DataRow row in dt.Rows)
            {
                DataRow r = dtForCbo.NewRow();
                r[0] = row[0].ToString().Trim();
                r[1] = row[1].ToString().Trim() + "|" + row[2];
                dtForCbo.Rows.Add(r);
            }

            return dtForCbo;
        }
    }
}
