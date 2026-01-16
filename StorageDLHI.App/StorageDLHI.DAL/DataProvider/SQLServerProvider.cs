using StorageDLHI.Infrastructor;
using StorageDLHI.Infrastructor.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace StorageDLHI.DAL.DataProvider
{
    public class SQLServerProvider
    {
        private SqlConnection _connection = null;
        private string _connString = "";
        private AppSettings AppSettings = new AppSettings();

        public SQLServerProvider()
        {
            //_connString = "server=DESKTOP-KD2BPDJ;database=DLHI_v2;Integrated Security = true;uid=sa;pwd=Aa123456@;MultipleActiveResultSets=True;";
            _connString = AppSettings.GetConnectionString("StorageDLHI");
            _connection = new SqlConnection(_connString);
        }

        public void ImportDatabase(string serverName, string databaseName, string userName, string pwd)
        {
            //// Path to SqlPackage.exe (bundled with the application)
            //string sqlPackagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlPackage", "SqlPackage.exe");

            //// Path to the .bacpac file
            //string bacpacFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "your_database.bacpac");

            //// SQL Server connection details
            //string targetServerName = serverName;
            //string targetDatabaseName = databaseName;

            //// Check if SqlPackage exists
            //if (!File.Exists(sqlPackagePath))
            //{
            //    throw new FileNotFoundException("SqlPackage.exe not found. Please ensure it is bundled with the application.");
            //}

            //// Run SqlPackage to import the database
            //ProcessStartInfo processStartInfo = new ProcessStartInfo
            //{
            //    FileName = sqlPackagePath,
            //    Arguments = $"/Action:Import /SourceFile:\"{bacpacFile}\" /TargetServerName:\"{targetServerName}\" /TargetDatabaseName:\"{targetDatabaseName}\"",
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};

            //using (Process process = Process.Start(processStartInfo))
            //{
            //    process.WaitForExit();
            //    string output = process.StandardOutput.ReadToEnd();
            //    string error = process.StandardError.ReadToEnd();

            //    if (process.ExitCode != 0)
            //    {
            //        throw new Exception($"Error importing database: {error}");
            //    }
            //}
        }

        //public static bool IsDatabaseImported(string databaseName, string serverName)
        //{
        //    // Connect to SQL Server
        //    Server server = new Server(serverName);

        //    // Check if the database exists
        //    return server.Databases.Cast<Database>().Any(db => db.Name == databaseName);
        //}

        public bool CheckConnection(string connectionString)
        {
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
                _connection.Close();
                LoggerConfig.Logger.Info($"Connection string: {connectionString} - Success !");
                return true;
            }
            catch (Exception)
            {
                LoggerConfig.Logger.Error($"Connection string: {connectionString} - Failed !");
                return false;
            }
        }

        public bool CreateAndGrantPermissionForUser(string databaseName, string loginName, string loginPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    // 1. Create SQL Login
                    string createLoginQuery = $@"
                            IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = '{loginName}')
                            BEGIN
                                CREATE LOGIN [{loginName}] WITH PASSWORD = '{loginPassword}';
                            END";

                    ExecuteNonQuery(conn, createLoginQuery);

                    // 2. Create user in your database
                    string createUserQuery = $@"
                            USE [{databaseName}];
                            IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '{loginName}')
                            BEGIN
                                CREATE USER [{loginName}] FOR LOGIN [{loginName}];
                            END";

                    ExecuteNonQuery(conn, createUserQuery);

                    // 3. Grant SELECT permission on all tables in schema dbo
                    string grantPermissionQuery = $@"
                                USE [{databaseName}];
                                GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO [{loginName}];
                                GRANT EXECUTE ON SCHEMA::dbo TO [{loginName}];";

                    ExecuteNonQuery(conn, grantPermissionQuery);

                    LoggerConfig.Logger.Info("User and permissions created successfully!");
                    return true;
                }
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return false;
            }
        }

        private void ExecuteNonQuery(SqlConnection connection, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public async Task<T> GetEntityByIdAsync<T>(string sqlQuery, Func<SqlDataReader, T> mapFunc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return mapFunc(reader);
                        }
                    }
                }
                LoggerConfig.Logger.Info($"Get data \"{mapFunc}\" by {ShareData.UserName}");
                return default;
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return default;
            }
            
        }

        public async Task<DataTable> GetDataAsync(string sqlQuery, string tableName)
        {
            //GetDataAsync
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString)) // NOT _connection
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader); // synchronous, but OK here
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

        public async Task<int> Insert(string sqlQuery)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    await conn.OpenAsync();
                    int rs = await cmd.ExecuteNonQueryAsync();
                    _connection.Close();
                    LoggerConfig.Logger.Info($"Insert database \"{sqlQuery}\" by {ShareData.UserName}");
                    return rs;
                }
            }
            catch (SqlException ex)
            {
                LoggerConfig.Logger.Error($"{ex.Message} by {ShareData.UserName}");
                return -1;
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Update(string sqlQeuery)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                using (SqlCommand cmd = new SqlCommand(sqlQeuery, conn))
                {
                    // Add parameters
                    await conn.OpenAsync();
                    int rs = await cmd.ExecuteNonQueryAsync();
                    _connection.Close();
                    LoggerConfig.Logger.Info($"Update record \"{sqlQeuery}\" by {ShareData.UserName}");
                    return rs; // return number of rows affected
                }
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
                //using (SqlConnection conn = new SqlConnection(_connStr))
                //using (SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE ID = @id", conn))
                //{
                //    cmd.Parameters.AddWithValue("@id", empId);
                //    await conn.OpenAsync();
                //    int rows = await cmd.ExecuteNonQueryAsync();
                //    return rows > 0;
                //}
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
