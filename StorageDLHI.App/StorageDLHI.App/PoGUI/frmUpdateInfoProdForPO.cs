using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Enums;
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
        public CustomProdOfPO prodOfPO { get; set; }

        private Guid prodId = Guid.Empty;
        
        public frmUpdateInfoProdForPO()
        {
            InitializeComponent();
        }

        public frmUpdateInfoProdForPO(string title, CustomProdOfPO customProdOfPO, Products prod)
        {
            InitializeComponent();
            this.Text = title;
            this.prodOfPO = customProdOfPO;
            this.prod = prod;
            this.prodId = prod.Id;

            txtThinh.Text = prod.A_Thinhness;
            txtDep.Text = prod.B_Depth;
            txtWidth.Text = prod.C_Witdh;
            txtWeb.Text = prod.D_Web;
            txtFlag.Text = prod.E_Flag;
            txtLength.Text = prod.F_Length;
            txtWeigth.Text = prod.G_Weight;
            txtQty.Text = this.prodOfPO.Qty.ToString();
            txtPrice.Value = this.prodOfPO.Price;
            txtRecevie.Text = this.prodOfPO.Recevie;
            txtRemark.Text = this.prodOfPO.Remark;
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
            this.prodOfPO.Qty = int.Parse(txtQty.Text.Trim());
            this.prodOfPO.Price = (Int32)txtPrice.Value;
            this.prodOfPO.Recevie = txtRecevie.Text.Trim();
            this.prodOfPO.Remark = txtRemark.Text.Trim();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.prodOfPO.Qty = 0;
            this.Close();
        }
    }
}
