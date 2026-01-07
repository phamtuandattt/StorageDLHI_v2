using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.ProjectDAO;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.MprGUI
{
    public partial class frmCustomInfoMPR_V2 : KryptonForm
    {
        private DataTable dtProjects = new DataTable();

        public frmCustomInfoMPR_V2()
        {
            InitializeComponent();
        }

        private void frmCustomInfoMPR_V2_Load(object sender, EventArgs e)
        {
            LoadDataProject();
        }

        private async void LoadDataProject()
        {
            // ----- LOAD DATA FOR PROD OF CREATE MPR
            if (!CacheManager.Exists(CacheKeys.PROJECT_DATATABLE_ALL_FOR_COMBOBOX))
            {
                var dtCommon = await ProjectDAO.GetProjects();
                if (dtCommon != null && dtCommon.dtProjects != null && dtCommon.dtProjectForCombox != null 
                    && dtCommon.dtProjects.AsEnumerable().Any() && dtCommon.dtProjectForCombox.AsEnumerable().Any())
                {
                    var dtCombobox = dtCommon.dtProjectForCombox;
                    dtProjects = dtCommon.dtProjects;

                    cboProject.DisplayMember = QueryStatement.PROPERTY_PROJECT_NAME;
                    cboProject.ValueMember = QueryStatement.PROPERTY_PROJECT_ID;
                }
                else
                {
                    MessageBoxHelper.ShowError("Please add project before create MPRs !");
                    return;
                }
                this.Close();
            }
            else
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
