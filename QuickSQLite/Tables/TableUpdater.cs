using NewzIndexerLib.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QuickSQLite.Typing;
using Microsoft.Data.Sqlite;

namespace QuickSQLite.Tables
{
	public class TableUpdater
	{
		public static void UpdateTable<T>(QSQLiteConnection connection)
		{
			string sql = AlterTableSql<T>(connection);

			using SqliteCommand cmd = connection.Connection.CreateCommand();

			cmd.CommandText = sql;
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// Only supports adding new columns. Changing the type or removing columns is not yet supported
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection"></param>
		/// <returns></returns>
		private static string AlterTableSql<T>(QSQLiteConnection connection)
		{
			Type type = typeof(T);
			string tableName = type.Name;

			// Get the existing columns in the table
			// Note, converting enumerable to list - shouldn't be an issue though since a table shouldn't have that many columns
			// furthermore we're using contains anyway, which requires the complete enumerable result
			List<string> existingColumns = GetTableColumns(connection, tableName).ToList();

			string alterTableSql = "";

			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo property in properties)
			{
				// Check if the column exists in the table
				if (!existingColumns.Contains(property.Name))
				{
					// Get the data type from the property's type
					string dataType = property.PropertyType.GetSQLiteDataType();

					alterTableSql += $"ADD COLUMN {property.Name} {dataType}, ";
				}
			}

			// Remove the trailing comma and space
			alterTableSql = alterTableSql.TrimEnd(',', ' ');

			return $"ALTER TABLE {tableName} {alterTableSql}";
		}

		private static IEnumerable<string> GetTableColumns(QSQLiteConnection connection, string tableName)
		{
			string sql = $"PRAGMA table_info({tableName})";

			using (SqliteCommand command = new SqliteCommand(sql, connection.Connection))
			{
				using (SqliteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						// Get the "name" column from the query result
						yield return reader["name"].ToString();
					}
				}
			}
		}
	}
}
