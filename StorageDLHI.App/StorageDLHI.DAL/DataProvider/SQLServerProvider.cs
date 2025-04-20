using StorageDLHI.Infrastructor;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
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
            _connection = new SqlConnection("server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@;MultipleActiveResultSets=True;");
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

        public async Task<DataTable> GetDataAsync(string sqlQuery, string tableName)
        {
            //GetDataAsync
            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, _connection))
                {
                    if (_connection.State != ConnectionState.Open)
                        await _connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader); // this is synchronous but fast
                        return dt;
                    }
                }
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return null;
            }
        }

        public bool UpdateDatabase(string sqlQuery, DataTable tableUpdate)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, _connection);
                SqlCommandBuilder cmd = new SqlCommandBuilder(da);
                da.Update(tableUpdate);
                LoggerConfig.Logger.Info($"Update database \"{sqlQuery}\" by {ShareData.UserName}");
                return true;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return false;
                throw new Exception(ex.Message);
            }
        }

        public int UpdateRowExistOfTable(string procName, string parameterString, string typeName, DataTable tableUpdate, string resultCode)
        {
            using (var cmd = new SqlCommand(procName, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var paramTable = new SqlParameter($"@{parameterString}", SqlDbType.Structured)
                {
                    TypeName = $"dbo.{typeName}",
                    Value = tableUpdate
                };

                var paramResultCode = new SqlParameter($"@{resultCode}", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(paramTable);
                cmd.Parameters.Add(paramResultCode);

                _connection.Open();
                cmd.ExecuteNonQuery();
                _connection.Close();
                // ✅ Return the result value from SQL
                return (int)paramResultCode.Value;
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
                LoggerConfig.Logger.Info($"Insert database \"{sqlQuery}\" by {ShareData.UserName}");

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
                LoggerConfig.Logger.Info($"Update record \"{sqlQeuery}\" by {ShareData.UserName}");

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
                LoggerConfig.Logger.Info($"Delete \"{sqlQuery}\" by {ShareData.UserName}");

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
