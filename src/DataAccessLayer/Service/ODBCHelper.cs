using Utility.dataLayer.Abstract;
using DomainModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace Utility.dataLayer.Service
{
    public class ODBCHelper : IODBCHelper
    {
        private string ConnectionString { get; set; }

        private IOptions<ApplicationConfigurations> _myConfiguration { get; set; }
        private OdbcConnectionStringBuilder OdbcConnectionStringBuilder { get; set; }
        public ODBCHelper(IOptions<ApplicationConfigurations> myConfiguration)
        {
            _myConfiguration = myConfiguration;

            //ConnectionString = _myConfiguration.Value.UserConncetionString;
            ConnectionString = "";
            OdbcConnectionStringBuilder = new OdbcConnectionStringBuilder(ConnectionString);
            
        }

        public void CloseConnection(OdbcConnection connection)
        {
            connection.Close();
        }

        public OdbcConnection CreateConnection()
        {
            return new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString);
        }

        public OdbcParameter CreateParameter(string name, object value, OdbcType OdbcType)
        {
            return CreateParameter(name, 0, value, OdbcType, ParameterDirection.Input);
        }
        public OdbcParameter CreateParameter(string name, int size, object value, OdbcType OdbcType)
        {
            return CreateParameter(name, size, value, OdbcType, ParameterDirection.Input);
        }
        public OdbcParameter CreateParameter(string name, int size, object value, OdbcType OdbcType, ParameterDirection direction)
        {
            return new OdbcParameter
            {
                OdbcType = OdbcType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }
        public DataTable GetDataTable(string commandText, CommandType commandType, OdbcParameter[] parameters = null)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                           command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);                           
                        }
                    }
                    var dataset = new DataSet();
                    var dataAdaper = new OdbcDataAdapter(command);
                    dataAdaper.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
        public DataSet GetDataSet(string commandText, CommandType commandType, OdbcParameter[] parameters = null)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    var dataset = new DataSet();

                    var dataAdaper = new OdbcDataAdapter(command);

                    dataAdaper.Fill(dataset);
                    return dataset;
                }
            }
        }
        public IDataReader GetDataReader(string commandText, CommandType commandType, OdbcParameter[] parameters, out OdbcConnection connection)
        {
            IDataReader reader = null;
            connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString);
            connection.Open();
            var command = new OdbcCommand(commandText, connection);
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                }
            }
            reader = command.ExecuteReader();

            return reader;
        }
        public void Delete(string commandText, CommandType commandType, OdbcParameter[] parameters = null)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public int Insert(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            int lastId = 0;
            commandText += " SELECT SCOPE_IDENTITY()";
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = 1000;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    lastId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return lastId;
        }
        public long InsertL(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            long lastId = 0;
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }
            return lastId;
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            OdbcTransaction transactionScope = null;
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
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
        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, OdbcParameter[] parameters)
        {
            OdbcTransaction transactionScope = null;
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
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
        public void Update(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            OdbcTransaction transactionScope = null;
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
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
        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, OdbcParameter[] parameters)
        {
            OdbcTransaction transactionScope = null;
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
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
        public object GetScalarValue(string commandText, CommandType commandType, OdbcParameter[] parameters = null)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    return command.ExecuteScalar();
                }
            }
        }

        public void InsertWithIdentity(string commandText, CommandType commandType, OdbcParameter[] parameters)
        {
            using (var connection = new OdbcConnection(OdbcConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new OdbcCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = 1000;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter.ParameterName, parameter.OdbcType, parameter.Size).Value = parameter.Value;
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}