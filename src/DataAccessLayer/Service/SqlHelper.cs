using Utility.dataLayer.Abstract;
using DomainModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace Utility.dataLayer.Service
{
    public class SqlHelper : ISqlHelper
    {
        private string ConnectionString { get; set; }
        private IOptions<ApplicationConfigurations> _myConfiguration { get; set; }
        private IOptions<ConnectionTimeOut> _connectionTimeOut { get; set; }
        private SqlConnectionStringBuilder sqlConnectionStringBuilder { get; set; }
        private readonly ILogger<SqlHelper> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myConfiguration"></param>
        /// <param name="connectionTimeOut"></param>
        /// <param name="logger"></param>
        /// <param name="valuesForAppScope"></param>
        public SqlHelper(ILogger<SqlHelper> logger, IOptions<ApplicationConfigurations> myConfiguration, IOptions<ConnectionTimeOut> connectionTimeOut)
        {
            _logger = logger;
            _myConfiguration = myConfiguration;
            _connectionTimeOut = connectionTimeOut;

                ConnectionString = _myConfiguration.Value.UserConncetionString;
            


            sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString)
            {
                ConnectTimeout = _connectionTimeOut.Value.TimeOut
            };           
        }

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        public SqlConnection CreateConnection(string conString = null)
        {
            return new SqlConnection(conString != null ? conString : sqlConnectionStringBuilder.ConnectionString);
        }

        public SqlParameter CreateParameter(string name, object value, DbType dbType)
        {
            return CreateParameter(name, 0, value, dbType, ParameterDirection.Input);
        }
        public SqlParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return CreateParameter(name, size, value, dbType, ParameterDirection.Input);
        }
        public SqlParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }
        public DataTable GetDataTable(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (commandType == CommandType.StoredProcedure)
            {
                _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started with SP: {commandText}");
            }
            else
            {
                _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            }
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    DataSet dataset = new DataSet();
                    SqlDataAdapter dataAdaper = new SqlDataAdapter(command);
                    dataAdaper.Fill(dataset);
                    watch.Stop();
                   // _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
                    _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds + (commandType == CommandType.StoredProcedure ? $" for SP {commandText}" : "")}");
                    return dataset.Tables[0];
                }
            }
        }
        public DataSet GetDataSet(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (commandType == CommandType.StoredProcedure)
            {
                _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started with SP: {commandText}");
            }
            else
            {
                _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            }
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    DataSet dataset = new DataSet();
                    SqlDataAdapter dataAdaper = new SqlDataAdapter(command);
                    dataAdaper.Fill(dataset);
                    //_logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
                    _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds + (commandType == CommandType.StoredProcedure ? $" for SP {commandText}" : "")}");
                    return dataset;
                }
            }
        }

        public void Execute(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (commandType == CommandType.StoredProcedure)
            {
                this._logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started with SP: {commandText}");
            }
            else
            {
                this._logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            }
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                    this._logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds + (commandType == CommandType.StoredProcedure ? $" for SP {commandText}" : "")}");
                }
            }
        }
        public IDataReader GetDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, out SqlConnection connection)
        {
            IDataReader reader = null;
            connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(commandText, connection)
            {
                CommandType = commandType,
                CommandTimeout = _connectionTimeOut.Value.TimeOut
            };
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            reader = command.ExecuteReader();

            return reader;
        }
        public void Delete(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
        }
        public int Insert(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            int lastId = 0;
            commandText += " SELECT SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    lastId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
            return lastId;
        }
        public void InsertWithIdentity(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
        }
        public long InsertL(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            long lastId = 0;
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
            return lastId;
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope = null;
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope = null;
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void Update(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope = null;
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope = null;
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public object GetScalarValue(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Started");
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = _connectionTimeOut.Value.TimeOut;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    _logger.LogInformation($"Method {MethodBase.GetCurrentMethod().Name} Executed. And Query Execution Time(in ms) is: {watch.ElapsedMilliseconds}");
                    return command.ExecuteScalar();
                }
            }
        }

    }
}
