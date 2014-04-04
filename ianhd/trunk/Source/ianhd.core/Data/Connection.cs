using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ianhd.core.Data
{
    public sealed class Connection : IDbConnection
    {
        public DbConnection DbConnection { get; set; }

        public string ConnectionString { get; set; }

        public int ConnectionTimeout { get; private set; }

        public string Database { get; private set; }

        public ConnectionState State { get; private set; }

        public Connection(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.DbConnection = new SqlConnection(connectionString);
            //this.DbConnection.Open();
        }

        public void Dispose()
        {
            //this.DbConnection.Close();
            this.DbConnection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return this.DbConnection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this.DbConnection.BeginTransaction(il);
        }

        public void Close()
        {
            this.DbConnection.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            this.DbConnection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return this.DbConnection.CreateCommand();
        }

        public void Open()
        {
            this.DbConnection.Open();
        }

        // DataSet

        public DataSet ExecuteDataSet(DbCommand command)
        {
            var dataSet = new DataSet();
            var dataAdapter = new SqlDataAdapter();

            if (command.Connection == null)
            {
                    command.Connection = this.DbConnection;

                    dataAdapter.SelectCommand = (SqlCommand)command;
                    dataAdapter.Fill(dataSet);
            }
            else
            {
                dataAdapter.SelectCommand = (SqlCommand)command;
                dataAdapter.Fill(dataSet);
            }

            return dataSet;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return ExecuteDataSet((DbCommand)command);
        }

        public DataSet ExecuteDataSet(string storedProcedureName, dynamic parameters)
        {
            var command = this.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add((object)parameter);
            }

            return ExecuteDataSet((DbCommand)command);
        }

        // DataTable

        public DataTable ExecuteDataTable(DbCommand command, int index = 0)
        {
            var dataSet = ExecuteDataSet(command);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.Tables[index];
            }

            return null;
        }

        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return ExecuteDataTable((DbCommand)command);
        }

        public DataTable ExecuteDataTable(string storedProcedureName, dynamic parameters)
        {
            var command = this.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add((object)parameter);
            }

            return ExecuteDataTable((DbCommand)command);
        }

        // Non-Query

        public int ExecuteNonQuery(DbCommand command)
        {
            if (command.Connection == null)
            {
                command.Connection = this.DbConnection;
                return command.ExecuteNonQuery();
            }

            return command.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return ExecuteNonQuery((DbCommand)command);
        }

        public int ExecuteNonQuery(string storedProcedureName, dynamic parameters)
        {
            var command = this.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add((object)parameter);
            }

            return ExecuteNonQuery((DbCommand)command);
            
        }

        // DataReader

        public IDataReader ExecuteReader(DbCommand command)
        {
            DbDataReader dataReader;

            if (command.Connection == null)
            {
                command.Connection = this.DbConnection;
                dataReader = command.ExecuteReader();
            }
            else
            {
                dataReader = command.ExecuteReader();
            }

            return dataReader;
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return ExecuteReader((DbCommand)command);
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText, int commandTimeout)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = commandTimeout;

            return ExecuteReader((DbCommand)command);
        }

        public IDataReader ExecuteReader(string storedProcedureName, dynamic parameters)
        {
            var command = this.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add((object)parameter);
            }

            return ExecuteReader((DbCommand)command);
        }

        // Scalar

        public object ExecuteScalar(DbCommand command)
        {
            object result;

            if (command.Connection == null)
            {
                command.Connection = this.DbConnection;
                result = command.ExecuteScalar();
            }
            else
            {
                result = command.ExecuteScalar();
            }

            return result;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            var command = this.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return ExecuteScalar((DbCommand)command);
        }

        public object ExecuteScalar(string storedProcedureName, dynamic parameters)
        {
            var command = this.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add((object)parameter);
            }

            return ExecuteScalar((DbCommand)command);
        }
    }
}
