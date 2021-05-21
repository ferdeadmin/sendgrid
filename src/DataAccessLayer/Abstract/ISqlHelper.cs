using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.dataLayer.Abstract
{
    public interface ISqlHelper
    {
        SqlConnection CreateConnection(string conString = null);
        void CloseConnection(SqlConnection connection);
        void Execute(string commandText, CommandType commandType, SqlParameter[] parameters = null);
        SqlParameter CreateParameter(string name, object value, DbType dbType);
        SqlParameter CreateParameter(string name, int size, object value, DbType dbType);
        SqlParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction);
        DataTable GetDataTable(string commandText, CommandType commandType, SqlParameter[] parameters = null);
        DataSet GetDataSet(string commandText, CommandType commandType, SqlParameter[] parameters = null);
        IDataReader GetDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, out SqlConnection connection);
        void Delete(string commandText, CommandType commandType, SqlParameter[] parameters = null);
        int Insert(string commandText, CommandType commandType, SqlParameter[] parameters);
        long InsertL(string commandText, CommandType commandType, SqlParameter[] parameters);
        void InsertWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters);
        void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters);
        void Update(string commandText, CommandType commandType, SqlParameter[] parameters);
        void UpdateWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters);
        void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters);
        object GetScalarValue(string commandText, CommandType commandType, SqlParameter[] parameters = null);
        void InsertWithIdentity(string query, CommandType text, SqlParameter[] sqlParameter);
    }
}
