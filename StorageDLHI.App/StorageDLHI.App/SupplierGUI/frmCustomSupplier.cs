using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.Infrastructor.Caches;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App.SupplierGUI
{
    public partial class frmCustomSupplier : KryptonForm
    {
        private bool status = true;
        private DataTable dtBanks = null;
        private Suppliers spModel = null;

        public frmCustomSupplier()
        {
            InitializeComponent();
        }

        public frmCustomSupplier(string title, bool status, DataTable dtBanks, Suppliers spModel) // status: true -> Add || Update
        {
            InitializeComponent();
            this.Text = title;
            this.status = status;
            
            if (!status)
            {
                // Load exist banks
                if (dtBanks.Rows.Count > 0)
                {
                    this.dtBanks = dtBanks; 
                    dgvBankOfSup.DataSource = dtBanks;
                    dgvBankOfSup.Rows[0].Selected = true;
                }

                this.spModel = spModel;
                txtName.Text = this.spModel.Name;
                txtCert.Text = this.spModel.Cert;
                txtEmail.Text = this.spModel.Email;
                txtPhone.Text = this.spModel.Phone;
                txtViettat.Text = this.spModel.Viettat;
                txtAddress.Text = this.spModel.Address;
            }
        }

        private void LoadData()
        {

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (status)
            {
                Suppliers sup = new Suppliers()
                {
                    Id = Guid.NewGuid(),
                    Name = txtName.Text.Trim(),
                    Cert = txtCert.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Viettat = txtViettat.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                };

                DataTable dtForm = await SupplierDAO.GetSupplierBanksForms();
                foreach (DataGridViewRow row in dgvBankOfSup.Rows)
                {
                    DataRow r_new = dtForm.NewRow();
                    r_new[0] = Guid.NewGuid();
                    r_new[1] = sup.Id; // Supplier ID
                    r_new[2] = row.Cells[2].Value;
                    r_new[3] = row.Cells[3].Value;
                    r_new[4] = row.Cells[4].Value;

                    dtForm.Rows.Add(r_new);
                }

                if (SupplierDAO.InsertSupplier(sup))
                {
                    if (SupplierDAO.InsertBankOfSup(dtForm))
                    {
                        MessageBoxHelper.ShowInfo("Add Supplier and Bank account success !");
                        this.Close();
                    }
                    else
                    {
                        SupplierDAO.DeleteSupplier(sup);
                        MessageBoxHelper.ShowWarning("Add Supplier and Bank account fail !");
                        this.Close();
                    }
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Add Supplier and Bank account fail !");
                    this.Close();
                }
            }
            else
            {
                // Update
                Suppliers sup = new Suppliers()
                {
                    Id = this.spModel.Id,
                    Name = txtName.Text.Trim(),
                    Cert = txtCert.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Viettat = txtViettat.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                };

                DataTable dtFormUp = await SupplierDAO.GetSupplierBanksForms();
                DataTable dtFormAdd = await SupplierDAO.GetSupplierBanksForms();

                foreach (DataGridViewRow row in dgvBankOfSup.Rows)
                {
                    if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()) 
                        && string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    {
                        // Add new record
                        DataRow r_new_add = dtFormAdd.NewRow();
                        r_new_add[0] = Guid.NewGuid();
                        r_new_add[1] =  sup.Id; // Supplier ID
                        r_new_add[2] = row.Cells[2].Value;
                        r_new_add[3] = row.Cells[3].Value;
                        r_new_add[4] = row.Cells[4].Value;

                        dtFormAdd.Rows.Add(r_new_add);
                    }
                    else
                    {
                        // Update exist record
                        DataRow r_new = dtFormUp.NewRow();
                        r_new[0] = row.Cells[0].Value;
                        r_new[1] = row.Cells[1].Value; // Supplier ID
                        r_new[2] = row.Cells[2].Value;
                        r_new[3] = row.Cells[3].Value;
                        r_new[4] = row.Cells[4].Value;

                        dtFormUp.Rows.Add(r_new);
                    }
                }

                if (SupplierDAO.UpdateSupplier(sup))
                {
                    if (SupplierDAO.UpdateSupplierBank(dtFormUp, dtFormAdd))
                    {
                        MessageBoxHelper.ShowInfo("Update Supplier and Bank account success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Update Supplier and Bank account fail !");
                        this.Close();
                    }
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Update Supplier and Bank account fail !");
                    this.Close();
                }
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void tlsAddBank_Click(object sender, EventArgs e)
        {
            if (this.dtBanks is null)
            {
                this.dtBanks = await SupplierDAO.GetSupplierBanksForms();
                DataRow r_new = this.dtBanks.NewRow();
                this.dtBanks.Rows.Add(r_new);
                dgvBankOfSup.DataSource = this.dtBanks;
            }
            else
            {
                DataRow r_new = this.dtBanks.NewRow();
                this.dtBanks.Rows.Add(r_new);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBankOfSup.Rows.Count <= 0) return;
            int rsl = dgvBankOfSup.CurrentRow.Index;
            var bankId = Guid.Parse(dgvBankOfSup.Rows[rsl].Cells[0].Value.ToString().Trim());
            var bankAcc = dgvBankOfSup.Rows[rsl].Cells[2].Value.ToString().Trim();

            if (!MessageBoxHelper.Confirm($"Are you sure DELETE Bank account [{bankAcc}] ?"))
            {
                return;
            }
            if (SupplierDAO.DeleteBank(bankId))
            {
                foreach (DataRow dr in dtBanks.Rows)
                {
                    if (Guid.Parse(dr["ID"].ToString().Trim()) == bankId)
                    {
                        dtBanks.Rows.Remove(dr);
                        break;
                    }
                }
                dgvBankOfSup.DataSource = dtBanks;
                MessageBoxHelper.ShowInfo("Delete completed !");
            }
            else
            {
                MessageBoxHelper.ShowWarning("Delete uncompleted !");
            }
        }
    }
}
