using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.PoGUI
{
    public partial class frmSeacrhPOFromDate : KryptonForm
    {
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;

        public bool IsSearch { get; set; } = false;

        public frmSeacrhPOFromDate()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FromDate = dtpFrom.Value;
            ToDate = dtpTo.Value;
            IsSearch = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSeacrhPOFromDate_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
