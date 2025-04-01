using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.Common
{
    public static class MessageBoxHelper
    {
        /// <summary>
        /// Shows an information MessageBox with the provided message and an optional title.
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            KryptonMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an error MessageBox with the provided message and an optional title.
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            KryptonMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning MessageBox with the provided message and an optional title.
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            KryptonMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a confirmation MessageBox and returns true if user clicks Yes.
        /// </summary>
        public static bool Confirm(string message, string title = "Confirm")
        {
            return KryptonMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
