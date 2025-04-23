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
        public static async void ShowDialogHelp(int delay)
        {
            var frm = new LoadingForm();
            frm.Show();
            await Task.Delay(delay); // simulate task
            frm.Close();
        }

        public static async Task<T> WithLoader<T>(Func<Task<T>> operation)
        {
            LoadingForm loader = new LoadingForm();
            Task show = Task.Run(() => loader.ShowDialog());

            try
            {
                return await operation();
            }
            finally
            {
                await Task.Delay(200); // Allow UI to "breathe"
                loader.Invoke((MethodInvoker)(() => loader.Close()));
            }
        }

        public static async Task WithLoader(Func<Task> operation)
        {
            LoadingForm loader = new LoadingForm();
            Task showTask = Task.Run(() => loader.ShowDialog());

            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                loader.Invoke(new Action(() =>
                {
                    loader.Close();
                }));
            }
        }
    }
}
