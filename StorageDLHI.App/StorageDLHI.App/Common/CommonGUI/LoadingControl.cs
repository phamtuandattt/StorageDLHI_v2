using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common.CommonGUI
{
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(100, 0, 0, 0); // semi-transparent
            this.Dock = DockStyle.Fill;
            this.Visible = false;
        }

        public void ShowLoader()
        {
            this.Visible = true;
            this.BringToFront();
        }

        public void HideLoader()
        {
            this.Visible = false;
        }
    }
}
