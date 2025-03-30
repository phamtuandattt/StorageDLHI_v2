﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmConnectSystem());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (string.IsNullOrEmpty(Properties.Settings.Default.DbConnectionString))
            {
                // First-time setup
                Application.Run(new frmConnectSystem());
            }
            else
            {
                // Connection already configured
                Application.Run(new Form1());
            }
        }
    }
}
