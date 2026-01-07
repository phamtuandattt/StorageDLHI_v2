using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static async Task<ProjectResponse> GetProjects()
        {
            string sqlQuery = string.Format(QueryStatement.GET_PROJECTS);
            var dt = await data.GetDataAsync(sqlQuery, "PROJECTS");
            var dtForCombox = new DataTable();
            dtForCombox.Columns.Add(QueryStatement.PROPERTY_PROJECT_ID);
            dtForCombox.Columns.Add(QueryStatement.PROPERTY_PROJECT_NAME);

            var dtProjects = new DataTable();
            dtProjects.Columns.Add(QueryStatement.PROPERTY_PROJECT_ID);
            dtProjects.Columns.Add(QueryStatement.PROPERTY_PROJECT_NAME);
            dtProjects.Columns.Add(QueryStatement.PROPERTY_PROJECT_CODE);
            dtProjects.Columns.Add(QueryStatement.PROPERTY_PROJECT_NO);
            dtProjects.Columns.Add(QueryStatement.PROPERTY_PROJECT_WO);

            foreach (DataRow item in dt.Rows)
            {
                DataRow rC = dtForCombox.NewRow();
                rC[0] = item[QueryStatement.PROPERTY_PROJECT_ID].ToString().Trim();
                rC[1] = item[QueryStatement.PROPERTY_PROJECT_NAME].ToString().Trim();
                dtForCombox.Rows.Add(rC);

                DataRow rP = dtProjects.NewRow();
                rP[0] = item[QueryStatement.PROPERTY_PROJECT_ID].ToString().Trim();
                rP[1] = item[QueryStatement.PROPERTY_PROJECT_NAME].ToString().Trim();
                rP[2] = item[QueryStatement.PROPERTY_PROJECT_CODE].ToString().Trim();
                rP[3] = item[QueryStatement.PROPERTY_PROJECT_NO].ToString().Trim();
                rP[4] = item[QueryStatement.PROPERTY_PROJECT_WO].ToString().Trim();
                dtProjects.Rows.Add(rP);
            }

            var projectRes = new ProjectResponse()
            {
                dtProjectForCombox = dtForCombox,
                dtProjects = dtProjects,
            };

            return projectRes;
        }
    }

    public class ProjectResponse
    {
        public DataTable dtProjectForCombox { get; set; }
        public DataTable dtProjects { get; set; }
    }
}
