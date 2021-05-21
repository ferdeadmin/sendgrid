using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace Utility.dataLayer.Abstract
{
    public interface IODBCHelper
    {
        OdbcConnection CreateConnection();
        void CloseConnection(OdbcConnection connection);
        OdbcParameter CreateParameter(string name, object value, OdbcType OdbcType);
        OdbcParameter CreateParameter(string name, int size, object value, OdbcType OdbcType);
        OdbcParameter CreateParameter(string name, int size, object value, OdbcType OdbcType, ParameterDirection direction);
        DataTable GetDataTable(string commandText, CommandType commandType, OdbcParameter[] parameters = null);
        DataSet GetDataSet(string commandText, CommandType commandType, OdbcParameter[] parameters = null);
        IDataReader GetDataReader(string commandText, CommandType commandType, OdbcParameter[] parameters, out OdbcConnection connection);
        void Delete(string commandText, CommandType commandType, OdbcParameter[] parameters = null);
        int Insert(string commandText, CommandType commandType, OdbcParameter[] parameters);
        long InsertL(string commandText, CommandType commandType, OdbcParameter[] parameters);
        void InsertWithTransaction(string commandText, CommandType commandType, OdbcParameter[] parameters);
        void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, OdbcParameter[] parameters);
        void Update(string commandText, CommandType commandType, OdbcParameter[] parameters);
        void UpdateWithTransaction(string commandText, CommandType commandType, OdbcParameter[] parameters);
        void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, OdbcParameter[] parameters);
        object GetScalarValue(string commandText, CommandType commandType, OdbcParameter[] parameters = null);
        void InsertWithIdentity(string query, CommandType text, OdbcParameter[] sqlParameter);
    }
}
