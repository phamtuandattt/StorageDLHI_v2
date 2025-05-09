using ComponentFactory.Krypton.Toolkit;
using StorageDLHI.App.Common;
using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using StorageDLHI.Infrastructor.Caches;
using StorageDLHI.Infrastructor.Commons;
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

namespace StorageDLHI.App.SupplierGUI
{
    public partial class frmCustomSupplier : KryptonForm
    {
        private bool status = true;
        private DataTable dtBanks = new DataTable();
        private Suppliers spModel = null;

        public bool IsCompleted { get; set; } = true;

        public frmCustomSupplier()
        {
            InitializeComponent();
        }

        public frmCustomSupplier(string title, bool status, DataTable dtBanks, Suppliers spModel) // status: true -> Add || Update
        {
            InitializeComponent();
            this.Text = title;
            this.status = status;

            this.dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_ID); // 0
            this.dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_SUPPLIER_ID); // 1
            this.dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_BANK_ACCOUNT); // 2
            this.dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_NAME); // 3
            this.dtBanks.Columns.Add(QueryStatement.PROPERTY_SUPPLIER_BANK_BENEFICIAL); // 4
            this.dtBanks.Columns.Add("IS_ADD", typeof(bool)); // 5
            

            if (!status)
            {
                // Load exist banks
                if (dtBanks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBanks.Rows)
                    {
                        DataRow dataRow = this.dtBanks.NewRow();
                        dataRow[0] = dr[0];
                        dataRow[1] = dr[1];
                        dataRow[2] = dr[2];
                        dataRow[3] = dr[3];
                        dataRow[4] = dr[4];
                        dataRow[5] = dr[5];

                        this.dtBanks.Rows.Add(dataRow);
                    }
                    
                    dgvBankOfSup.DataSource = this.dtBanks;
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

                if (await ShowDialogManager.WithLoader(() => SupplierDAO.InsertSupplier(sup)))
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
                    //if (string.IsNullOrEmpty(row.Cells[0].Value.ToString()) 
                    //    && string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    if (bool.Parse(row.Cells[5].Value.ToString()) == true)
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

                if (await  SupplierDAO.UpdateSupplier(sup))
                {
                    if (await ShowDialogManager.WithLoader(() => SupplierDAO.UpdateSupplierBank(dtFormUp, dtFormAdd)))
                    {
                        MessageBoxHelper.ShowInfo("Update Supplier and Bank account success !");
                        this.Close();
                    }
                    else
                    {
                        MessageBoxHelper.ShowWarning("Update Supplier and Bank account fail !");
                    }
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Update Supplier and Bank account fail !");
                }
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCompleted = false;
            this.Close();
        }

        private void tlsAddBank_Click(object sender, EventArgs e)
        {
            if (this.dtBanks.Rows.Count <= 0)
            {
                DataRow r_new = this.dtBanks.NewRow();
                r_new[0] = Guid.NewGuid();
                r_new[1] = Guid.Parse(this.spModel.Id.ToString());
                r_new[5] = true;
                this.dtBanks.Rows.Add(r_new);
                dgvBankOfSup.DataSource = this.dtBanks;
            }
            else
            {
                DataRow r_new = this.dtBanks.NewRow();
                r_new[0] = Guid.NewGuid();
                r_new[1] = Guid.Parse(this.spModel.Id.ToString());
                r_new[5] = true;
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

        private void dgvBankOfSup_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.Common.RenderNumbering(sender, e, this.Font);
        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtAddress.Text, Infrastructor.Commons.Common.REGEX_VALID_VIETNAM_ADDRESS, "");
            if (txtAddress.Text != cleaned)
            {
                int pos = txtAddress.SelectionStart - 1;
                txtAddress.Text = cleaned;
                txtAddress.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtName.Text, Infrastructor.Commons.Common.REGEX_VALID_NAME, "");
            if (txtName.Text != cleaned)
            {
                int pos = txtName.SelectionStart - 1;
                txtName.Text = cleaned;
                txtName.SelectionStart = Math.Max(pos, 0);
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

        private void txtViettat_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtViettat.Text, Infrastructor.Commons.Common.REGEX_VALID_VIETTAT, "");
            if (txtViettat.Text != cleaned)
            {
                int pos = txtViettat.SelectionStart - 1;
                txtViettat.Text = cleaned;
                txtViettat.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtCert_TextChanged(object sender, EventArgs e)
        {
            string cleaned = Regex.Replace(txtCert.Text, Infrastructor.Commons.Common.REGEX_VALID_CERT, "");
            if (txtCert.Text != cleaned)
            {
                int pos = txtCert.SelectionStart - 1;
                txtCert.Text = cleaned;
                txtCert.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void txtViettat_TextAlignChanged(object sender, EventArgs e)
        {

        }
    }
}
