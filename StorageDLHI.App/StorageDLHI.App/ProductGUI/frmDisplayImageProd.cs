using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.ProductGUI
{
    public partial class frmDisplayImageProd : KryptonForm
    {
        public frmDisplayImageProd(Products pModel)
        {
            InitializeComponent();
            picItem.Image = pModel.Image.Length == 100 ? picItem.InitialImage : Image.FromStream(new MemoryStream(pModel.Image));
            groupBoxImage.Values.Heading = pModel.Product_Name;
        }

        private void frmDisplayImageProd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.Handled = true; // Prevents the key press from being passed to other controls
            }
        }
    }
}
