using StorageDLHI.BLL.StaffDAO;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Emp_DepGUI
{
    public partial class ucEmpDepManage : UserControl
    {
        public ucEmpDepManage()
        {
            InitializeComponent();
        }

        private async void ucEmpDepManage_Load(object sender, EventArgs e)
        {
            await LoadDeps();
        }

        private async Task LoadDeps()
        {
            var dt = await StaffDAO.GetDeps();
            cboDep.ComboBox.DataSource = dt;
            cboDep.ComboBox.DisplayMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_NAME_CODE;
            cboDep.ComboBox.ValueMember = QueryStatement.PROPERTY_FOR_COMBO_DEP_ID;
        }
    }
}
