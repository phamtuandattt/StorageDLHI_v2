using StorageDLHI.App.Common.CommonGUI;
using StorageDLHI.App.MprGUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public static class ShowDialogManager
    {
        public static async void ShowDialogHelp()
        {
            var frm = new LoadingForm();
            frm.Show();
            await Task.Delay(3000); // simulate task
            frm.Close();
        }
    }
}
