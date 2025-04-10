using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.DAL.Models;
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
    public partial class frmUpdateInfoProdForPO : KryptonForm
    {
        public Products prod { get; set; }
        public int qty { get; set; }
        private Guid prodId = Guid.Empty;
        
        public frmUpdateInfoProdForPO()
        {
            InitializeComponent();
        }

        public frmUpdateInfoProdForPO(string title, int qty, Products prod)
        {
            InitializeComponent();
            this.Text = title;
            this.qty = qty;
            this.prod = prod;
            this.prodId = prod.Id;

            txtThinh.Text = prod.A_Thinhness;
            txtDep.Text = prod.B_Depth;
            txtWidth.Text = prod.C_Witdh;
            txtWeb.Text = prod.D_Web;
            txtFlag.Text = prod.E_Flag;
            txtLength.Text = prod.F_Length;
            txtWeigth.Text = prod.G_Weight;
            txtQty.Text = qty.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.prod.Id = this.prodId;
            this.prod.A_Thinhness = txtThinh.Text.Trim();
            this.prod.B_Depth = txtDep.Text.Trim();
            this.prod.C_Witdh = txtWidth.Text.Trim();
            this.prod.D_Web = txtWeb.Text.Trim();
            this.prod.E_Flag = txtFlag.Text.Trim();
            this.prod.F_Length = txtLength.Text.Trim();
            this.prod.G_Weight = txtWeigth.Text.Trim();
            this.qty = int.Parse(txtQty.Text.Trim());

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.qty = 0;
            this.Close();
        }
    }
}
