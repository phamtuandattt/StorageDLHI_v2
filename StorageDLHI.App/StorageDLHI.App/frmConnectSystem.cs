using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.MainGUI;
using StorageDLHI.DAL.DataProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App
{
    public partial class frmConnectSystem : KryptonForm
    {
        private SQLServerProvider _services = new SQLServerProvider();
        public string ConnectionString {  get; set; }


        public frmConnectSystem()
        {
            InitializeComponent();
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerName.Text) || string.IsNullOrEmpty(txtDatabase.Text))
            {
                KryptonMessageBox.Show("Please fill in all information !", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtUser.Text))
            {
                ConnectionString = string.Format("server={0};database={1};Integrated Security = true;", txtServerName.Text, txtDatabase.Text);
            }
            else
            {
                ConnectionString = $"server={txtServerName.Text};database={txtDatabase.Text};Integrated Security = true;uid={txtUser.Text};pwd={txtPwd.Text}";
            }


            if (_services.CheckConnection(ConnectionString))
            {
                KryptonMessageBox.Show("Connected successfully !", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                KryptonMessageBox.Show("Connected unsuccessfully !", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Test connection before saving
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    // Save connection string to settings
                    Properties.Settings.Default.DbConnectionString = ConnectionString;
                    Properties.Settings.Default.Save();

                    KryptonMessageBox.Show("Connection saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Proceed to main application form
                    Main main = new Main();
                    this.Hide();
                    main.ShowDialog();
                    this.Close(); ;
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show("Unable to connect: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //C:\Users\[YourUserName]\AppData\Local\[YourAppName]\[YourAppName].exe_[hash]\[version]\user.config


        // Example: Reset connection string to force setup on next launch
        private void ResetConnectionString(object sender, EventArgs e)
        {
            Properties.Settings.Default.DbConnectionString = "";
            Properties.Settings.Default.Save();

            KryptonMessageBox.Show("Connection settings reset. Please restart the application.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
