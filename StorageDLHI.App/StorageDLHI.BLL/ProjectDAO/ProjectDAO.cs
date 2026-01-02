using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.ProjectDAO
{
    public class ProjectDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static async Task<bool> Insert(Projects projects)
        {
            string sqlQuery = string.Format(QueryStatement.INSERT_PROJECT, projects.Id, projects.Name, projects.Code, projects.ProjectNo,
                projects.ProductInfo, projects.Weight, projects.WorkOrderNo, projects.CustomerId);

            return await data.Insert(sqlQuery) > 0;
        }
    }
}
