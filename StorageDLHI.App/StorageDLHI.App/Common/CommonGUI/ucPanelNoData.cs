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
    public partial class ucPanelNoData : UserControl
    {
        public Panel pnlNoData { get; set; } = new Panel();
        public ucPanelNoData(string message)
        {
            InitializeComponent();

            pnlNoData.BackColor = Color.White;
            pnlNoData.Visible = false;
            pnlNoData.BorderStyle = BorderStyle.None;
            pnlNoData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;// Show on top of the grid

            Label lblMessage = new Label();
            lblMessage.Text = "😕 " + message;
            lblMessage.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblMessage.ForeColor = Color.Gray;
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(30, 30);

            //PictureBox pic = new PictureBox();
            //pic.Image = Image.From(Properties.Resources.picture_bg); // Your image path here
            //pic.SizeMode = PictureBoxSizeMode.AutoSize;
            //pic.Location = new Point(30, 70);

            pnlNoData.Controls.Add(lblMessage);

            //pnlNoData.Controls.Add(pic);

        }
    }
}
