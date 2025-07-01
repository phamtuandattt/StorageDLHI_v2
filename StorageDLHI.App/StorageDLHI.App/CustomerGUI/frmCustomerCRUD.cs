using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.CustomerDAO;
using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.CustomerGUI
{
    public partial class frmCustomerCRUD : KryptonForm
    {
        public bool IsAdd = true;

        public frmCustomerCRUD(bool isAdd)
        {
            InitializeComponent();
            IsAdd = isAdd;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers()
            {
                Id = Guid.NewGuid(),
                Name = txtName.Text.Trim(),
                ClientInCharge = txtPersonInCharge.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
            };

            if (IsAdd)
            {
                if (await CustomerDAO.Add(customers))
                {
                    MessageBoxHelper.ShowInfo("Add customer succesful !");
                    this.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Add Customer fail !");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtName.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtName.Text != cleaned)
            {
                int pos = txtName.SelectionStart - 1;
                txtName.Text = cleaned;
                txtName.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtPersonInCharge_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtPersonInCharge.Text, Infrastructor.Commons.Common.REGEX_VALID_DES, "");
            if (txtPersonInCharge.Text != cleaned)
            {
                int pos = txtPersonInCharge.SelectionStart - 1;
                txtPersonInCharge.Text = cleaned;
                txtPersonInCharge.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtPhone.Text, Infrastructor.Commons.Common.REGEX_VALID_PHONE, "");
            if (txtPhone.Text != cleaned)
            {
                int pos = txtPhone.SelectionStart - 1;
                txtPhone.Text = cleaned;
                txtPhone.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtEmail.Text, Infrastructor.Commons.Common.REGEX_VALID_EMAIL, "");
            if (txtEmail.Text != cleaned)
            {
                int pos = txtEmail.SelectionStart - 1;
                txtEmail.Text = cleaned;
                txtEmail.SelectionStart = Math.Max(pos, 0);
            }
        }
    }
}
