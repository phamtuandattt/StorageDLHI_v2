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

namespace StorageDLHI.App.MprGUI
{
    public partial class frmGetQty : KryptonForm
    {
        public int Qty { get; set; }
        public string UsageNote { get; set; }

        public frmGetQty()
        {
            InitializeComponent();
        }

        private void btnAddProdIntoMpr_Click(object sender, EventArgs e)
        {
            Qty = int.Parse(txtQtyProd.Value.ToString().Trim());
            UsageNote = txtUsage.Text.Trim();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Qty = 0;
            this.Close();
        }

        private void frmGetQty_Load(object sender, EventArgs e)
        {

        }
    }
}
