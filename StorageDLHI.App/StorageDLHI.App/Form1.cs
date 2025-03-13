using StorageDLHI.BLL.SupplierDAO;
using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageDLHI.App
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection("server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@");
            LoadData();
        }

        private void LoadData()
        {
            var data = SupplierDAO.GetSuppliers();
            grdSuppliers.DataSource = data;
            //try
            //{
            //    if (connection.State == ConnectionState.Closed)
            //    {
            //        connection.Open();
            //    }

            //    // Câu lệnh lấy dữ liệu
            //    string sqlQuery = "SELECT *FROM SUPPLIERS";

            //    DataSet ds = new DataSet();
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            //    da.Fill(ds, "SUPPLIERS");
            //    grdSuppliers.DataSource = ds.Tables["SUPPLIERS"];
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (connection.State == ConnectionState.Closed)
            //{
            //    connection.Open();
            //}
            //string sqlQuery = string.Format("INSERT INTO SUPPLIERS VALUES ('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}', N'{6}', '{7}')", 
            //    Guid.NewGuid(), txtName.Text, txtCert.Text, txtEmail.Text, txtPhone.Text, "", "", "9d6312d2-733c-4743-9b8e-92f0c5965656");
            //SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            //int rs = cmd.ExecuteNonQuery();
            //connection.Close();
            var model = new Suppliers()
            {
                Id = Guid.NewGuid(),
                Name = txtName.Text,
                Cert = txtCert.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Viettat = "",
                Address = "",
                Bank_Id = Guid.Parse("9d6312d2-733c-4743-9b8e-92f0c5965656")
            };
            if (SupplierDAO.Insert(model))
            {
                MessageBox.Show("Success !");
            } 
            else
            {
                MessageBox.Show("Failure !");
            }

            LoadData();
        }

        private void btnLoaddata_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
