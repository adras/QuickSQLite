using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite
{
	public class QSQLiteConnection : IDisposable
	{
		SqliteConnection connection;
		private bool isDisposed = false;
		private QueryBuilder queryBuilder;

		public SqliteConnection Connection { get => connection; private set => connection = value; }

		public QSQLiteConnection()
		{
			string connectionString = QDbConfiguration.CreateConnectionString();
			Connection = new SqliteConnection(connectionString);
			queryBuilder = new QueryBuilder(connection);
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

		~QSQLiteConnection()
		{
			Dispose(false);
		}

		/// <summary>
		/// Opens a new connection to the SQLite-database using the settings in <see cref="QDbConfiguration"/>
		/// E.g. the file-name of the sqlite database is configured in <see cref="QDbConfiguration"/>
		/// </summary>
		public void Open()
		{
			Connection.Open();
		}

		public void InsertObjects<T>(IEnumerable<T> objects) where T : IQModel<T>
		{
			using SqliteTransaction transaction = connection.BeginTransaction();

			foreach (T obj in objects)
			{
				string sql = queryBuilder.CreateRecord(obj.QName, obj.CreateValueDictionary());
				SqliteCommand command = connection.CreateCommand();
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}

			transaction.Commit();
		}


		#region OldStuff

		public async Task<SqliteDataReader> CreateSelectCommand(string tableName, params string[] columnNames)
		{
			string columnSql = string.Join(",", columnNames);

			string commandText = $"SELECT {columnSql} FROM {tableName}";

			SqliteCommand command = new SqliteCommand(commandText, connection);
			SqliteDataReader result = await command.ExecuteReaderAsync();

			return result;
		}

		public QParameterizedCommand CreateInsertCommand(string tableName, SqliteTransaction transaction = null, params string[] columnNames)
		{
			string valueNamePrefix = "@";

			string columnSql = string.Join(",", columnNames);

			// Same as columns, but also prefix each value with the prefix like: @ArticleId
			string values = string.Join(",", columnNames.Select(name => $"{valueNamePrefix}{name}"));

			string commandText = $"INSERT INTO {tableName} ({columnSql}) VALUES({values})";

			SqliteCommand command = new SqliteCommand(commandText, connection, transaction);

			Dictionary<string, SqliteParameter> parameters = new Dictionary<string, SqliteParameter>();
			foreach (string columnName in columnNames)
			{
				SqliteParameter columnParam = new SqliteParameter();
				columnParam.ParameterName = $"{valueNamePrefix}{columnName}";
				parameters[columnName] = columnParam;
				command.Parameters.Add(columnParam);
			}

			QParameterizedCommand result = new QParameterizedCommand(command, parameters);
			return result;
		}

		public IEnumerable<T> QueryAll<T>() where T: IQModel<T>
		{
			queryBuilder.ReadRecords<T>();
			return null;
			//queryBuilder.ReadRecords()
		}
		#endregion
	}
}
