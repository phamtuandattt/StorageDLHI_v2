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

namespace StorageDLHI.App.MainGUI
{
    public partial class Main : KryptonForm
    {
        public Main()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetConnectionString(sender, e);
        }

        private void tlsPO_Click(object sender, EventArgs e)
        {

        }

        private void tlsMPR_Click(object sender, EventArgs e)
        {

        }

        private void tlsImport_Click(object sender, EventArgs e)
        {

        }

        private void tlsExport_Click(object sender, EventArgs e)
        {

        }

        private void tlsInventory_Click(object sender, EventArgs e)
        {

        }

        private void ResetConnectionString(object sender, EventArgs e)
        {
            Properties.Settings.Default.DbConnectionString = "";
            Properties.Settings.Default.Save();

            KryptonMessageBox.Show("Connection settings reset. Please restart the application.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }
    }
}
