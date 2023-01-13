using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace QuickSQLite
{

	public class QueryBuilder
	{
		private SqliteConnection _connection;

		public QueryBuilder(SqliteConnection connection)
		{
			_connection = connection;
		}

		public string CreateRecord(string tableName, Dictionary<string, object> data)
		{
			string columns = string.Join(", ", data.Keys);
			string values = string.Join(", ", data.Values);

			string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
			return sql;
		}

		public IEnumerable<T> ReadRecords<T>() where T : IQModel<T>
		{
			//string sql = ReadRecords()
			string sql = "";

			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				SqliteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Dictionary<string, object> record = new Dictionary<string, object>();
					//for (int i = 0; i < reader.FieldCount; i++)
					//{
					//	record.Add(reader.GetName(i), reader.GetValue(i));
					//}
					//records.Add(record);
					T result = (T)Activator.CreateInstance(typeof(T));
					//result.CreateObjectFromCurrentRow(reader);
				}
			}

			return null;
		}


		public string ReadRecordsSql(string tableName, Dictionary<string, object> filters = null, int pageSize = 100, int pageNumber = 1, string sortColumn = null, string sortOrder = "ASC")
		{
			List<string> whereClauses = new List<string>();
			if (filters != null)
			{
				foreach (var filter in filters)
				{
					whereClauses.Add($"{filter.Key} = '{filter.Value}'");
				}
			}

			string whereClause = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

			string sortClause = !string.IsNullOrEmpty(sortColumn) ? $"ORDER BY {sortColumn} {sortOrder}" : "";

			string paginationClause = $"LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}";

			string sql = $"SELECT * FROM {tableName} {whereClause} {sortClause} {paginationClause}";

			return sql;
		}

		public List<Dictionary<string, object>> ReadRecords(string tableName, Dictionary<string, object> filters = null, int pageSize = 100, int pageNumber = 1, string sortColumn = null, string sortOrder = "ASC")
		{
			List<string> whereClauses = new List<string>();
			if (filters != null)
			{
				foreach (var filter in filters)
				{
					whereClauses.Add($"{filter.Key} = '{filter.Value}'");
				}
			}

			string whereClause = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

			string sortClause = !string.IsNullOrEmpty(sortColumn) ? $"ORDER BY {sortColumn} {sortOrder}" : "";

			string paginationClause = $"LIMIT {pageSize} OFFSET {(pageNumber - 1) * pageSize}";

			string sql = $"SELECT * FROM {tableName} {whereClause} {sortClause} {paginationClause}";

			List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();

			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				SqliteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Dictionary<string, object> record = new Dictionary<string, object>();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						record.Add(reader.GetName(i), reader.GetValue(i));
					}
					records.Add(record);
				}
			}

			return records;
		}

		public void UpdateRecord(string tableName, Dictionary<string, object> data, Dictionary<string, object> filters)
		{
			List<string> updateClauses = new List<string>();
			foreach (var item in data)
			{
				updateClauses.Add($"{item.Key} = '{item.Value}'");
			}

			List<string> whereClauses = new List<string>();
			foreach (var filter in filters)
			{
				whereClauses.Add($"{filter.Key} = '{filter.Value}'");
			}

			string setClause = "SET " + string.Join(", ", updateClauses);
			string whereClause = "WHERE " + string.Join(" AND ", whereClauses);

			string sql = $"UPDATE {tableName} {setClause} {whereClause}";

			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				command.ExecuteNonQuery();
			}
		}

		public void DeleteRecord(string tableName, Dictionary<string, object> filters)
		{
			List<string> whereClauses = new List<string>();
			foreach (var filter in filters)
			{
				whereClauses.Add($"{filter.Key} = '{filter.Value}'");
			}

			string whereClause = "WHERE " + string.Join(" AND ", whereClauses);

			string sql = $"DELETE FROM {tableName} {whereClause}";

			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				command.ExecuteNonQuery();
			}
		}
	}
}