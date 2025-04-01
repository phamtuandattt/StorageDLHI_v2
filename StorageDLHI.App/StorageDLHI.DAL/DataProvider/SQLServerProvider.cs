using StorageDLHI.Infrastructor;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.DataProvider
{
    public class SQLServerProvider
    {
        private SqlConnection _connection = null;

        public SQLServerProvider()
        {
            _connection = new SqlConnection("server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@");
        }

        public bool CheckConnection (string connectionString)
        {
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
                _connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable GetData(string sqlQuery, string tableName)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, _connection);
                da.Fill(ds, tableName);
                return ds.Tables[tableName];
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return null;
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateDatabase(string sqlQuery, DataTable tableUpdate)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, _connection);
                SqlCommandBuilder cmd = new SqlCommandBuilder(da);
                da.Update(tableUpdate);
                return true;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return false;
                throw new Exception(ex.Message);
            }
        }

        public int Insert(string sqlQuery)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }    
                SqlCommand cmd = new SqlCommand(sqlQuery, _connection);
                int rs = cmd.ExecuteNonQuery();
                _connection.Close();

                return rs;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return -1;
                throw new Exception(ex.Message);
            }
        }

        public int Update(string sqlQeuery)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }    
                SqlCommand cmd = new SqlCommand(sqlQeuery, _connection);
                int rs = cmd.ExecuteNonQuery();
                _connection.Close();

                return rs;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return -1;
                throw new Exception(ex.Message);
            }
        }

        public int Delete(string sqlQuery)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }    
                SqlCommand cmd = new SqlCommand(sqlQuery, _connection);
                int rs = cmd.ExecuteNonQuery();
                _connection.Close();

                return rs;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return -1;
                throw new Exception(ex.Message);
            }
        }
    }
}
