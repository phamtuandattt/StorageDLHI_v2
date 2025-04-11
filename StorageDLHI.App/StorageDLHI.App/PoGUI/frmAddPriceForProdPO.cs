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
    public partial class frmAddPriceForProdPO : KryptonForm
    {
        public Int32 Price { get; set; }
        public string Recevie {  get; set; }
        public string Remark { get; set; }

        public frmAddPriceForProdPO()
        {
            InitializeComponent();
        }

        private void btnAddProdIntoMpr_Click(object sender, EventArgs e)
        {
            this.Price = (Int32)txtPrice.Value;
            this.Recevie = txtRecevie.Text.Trim();
            this.Remark = txtRemark.Text.Trim();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Price = 0;
            this.Close();
        }
    }
}
