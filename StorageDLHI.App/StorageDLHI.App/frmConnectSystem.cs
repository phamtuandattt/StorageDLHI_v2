using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.App.MainGUI;
using StorageDLHI.BLL.StaffDAO;
using StorageDLHI.DAL.DataProvider;
using StorageDLHI.Infrastructor;
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
        private AppSettings _settings = new AppSettings();
        private string _computer_name = "";

        public string ConnectionString {  get; set; }


        public frmConnectSystem()
        {
            InitializeComponent();

            _computer_name = System.Environment.MachineName;

            if (_computer_name.ToUpper().Equals("DESKTOP-KD2BPDJ")
                || _computer_name.ToUpper().Equals("DAVIDHOANG"))
            {
                txtUser.Text = "sa";
            }
            else
            {
                FillInfo();
            }
        }

        private async void FillInfo()
        {
            var mStaff = await StaffDAO.GetStaff(_computer_name);
            txtUser.Text = mStaff.Name;
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerName.Text) || string.IsNullOrEmpty(txtDatabase.Text))
            {
                MessageBoxHelper.ShowWarning("Please fill in all information !");
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

            // Import DB before connection sql
            _services.ImportDatabase(txtServerName.Text, txtDatabase.Text, txtUser.Text, txtPwd.Text);

            if (_services.CheckConnection(ConnectionString))
            {
                MessageBoxHelper.ShowInfo("Connected successfully !\nHope you have an enjoyable experience.");
                btnConnect.Enabled = true;
            }
            else
            {
                MessageBoxHelper.ShowWarning("Connected unsuccessfully !\nPlease check with your admin !");
                return;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            // Test connection before saving
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    // Save connection string to settings

                    _settings.SetConnectionString("StorageDLHI", ConnectionString);

                    // Get and Save ID Staff for remember 
                    var mStaff = await StaffDAO.GetStaff(_computer_name);
                    if (mStaff == null)
                    {
                        MessageBoxHelper.ShowError("Unable to connect database");
                        LoggerConfig.Logger.Error($"Staff is null || Computer name: {_computer_name} invalid");
                        return;
                    }
                    
                    string userId = mStaff.Id.ToString().Trim();
                    string userName = mStaff.Name.Trim().ToUpper();
                    string device = mStaff.DeviceName.Trim().ToUpper();

                    Properties.Settings.Default.DbConnectionString = ConnectionString;
                    Properties.Settings.Default.RememberLogin = $"{userId}|{userName}|{device}";
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
            Properties.Settings.Default.RememberLogin = "";
            Properties.Settings.Default.Save();

            KryptonMessageBox.Show("Connection settings reset. Please restart the application.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
