using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewzIndexerLib.Database
{
    public class SQLiteContext : IDisposable
    {
        SqliteConnection connection;
        private bool isDisposed = false;

        public SqliteConnection Connection { get => connection; private set => connection = value; }

        public SQLiteContext()
        {
            string connectionString = DbConfiguration.CreateConnectionString();
            Connection = new SqliteConnection(connectionString);
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);

            // Suppress calling the destructor, this avoids Disposing twice
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                }
                // Normally we'd do the dispose in the if-block above,
                // but this connection should also be disposed properly when it's not disposed
                // explicitely and the destructor was initiated
                Connection.Close();
                Connection.Dispose();
                Connection = null;
                File.WriteAllText("IGOTDISPOSED", "YEAH");
            }
        }
        #endregion

        ~SQLiteContext()
        {
            Dispose(false);
        }

        public void Open()
        {
            Connection.Open();
        }

        public ParameterizedCommand CreateInsertCommand(string tableName, SqliteTransaction transaction = null, params string[] columnNames)
        {
            string valueNamePrefix = "@";

            string columns = string.Join(",", columnNames);
            
            // Same as columns, but also prefix each value with the prefix like: @ArticleId
            string values = string.Join(",", columnNames.Select(name => $"{valueNamePrefix}{name}"));
            
            string commandText = $"INSERT INTO {tableName} ({columns}) VALUES({values})";

            SqliteCommand command = new SqliteCommand(commandText, connection, transaction);

            Dictionary<string, SqliteParameter> parameters = new Dictionary<string, SqliteParameter>();
            foreach(string columnName in columnNames)
            {
                SqliteParameter columnParam = new SqliteParameter();
                columnParam.ParameterName = $"{valueNamePrefix}{columnName}";
                parameters[columnName] = columnParam;
                command.Parameters.Add(columnParam);
            }
            
            ParameterizedCommand result = new ParameterizedCommand(command, parameters);
            return result;
        }
    }

    public class ParameterizedCommand : IDisposable
    {
        SqliteCommand cmd;
        Dictionary<string, SqliteParameter> parameters;

        public ParameterizedCommand(SqliteCommand cmd, Dictionary<string, SqliteParameter> parameters)
        {
            Cmd = cmd;
            Parameters = parameters;
        }

        public SqliteCommand Cmd { get => cmd; set => cmd = value; }
        public Dictionary<string, SqliteParameter> Parameters { get => parameters; set => parameters = value; }

        public void Dispose()
        {
            cmd.Dispose();
        }
    }
}
