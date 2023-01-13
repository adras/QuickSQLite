using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using QuickSQLite.Typing;

namespace QuickSQLite.Tables
{
	internal class QTableCreater
	{
		public static void CreateTable<T>(QSQLiteConnection connection, bool includeIfNotExists = false) where T : IQModel<T>
		{
			string sql = CreateTableSql<T>(includeIfNotExists);
			using SqliteCommand cmd = connection.Connection.CreateCommand();
			cmd.CommandText = sql;
			cmd.ExecuteNonQuery();
		}

		private static string CreateTableSql<T>(bool includeIfNotExists = false) where T : IQModelCached
		{
			Type type = typeof(T);
			string tableName = type.Name;

			string columnDefinitions = "";
			IEnumerable<PropertyInfo> properties = QReflectionModelCache.GetPropertiesForType<T>();
			
			foreach (PropertyInfo property in properties)
			{
				// Get the data type from the property's type
				string dataType = property.PropertyType.GetSQLiteDataType();

				// Check if the property type is nullable
				bool isNullable = property.IsNullableType();
				if (isNullable)
				{
					dataType += " NULL";
				}
				else
				{
					dataType += " NOT NULL";
				}

				columnDefinitions += $"{property.Name} {dataType}, ";
			}

			// Remove the trailing comma and space
			columnDefinitions = columnDefinitions.TrimEnd(',', ' ');

			string ifNotExists = (includeIfNotExists) ? "IF NOT EXISTS" : "";
			return $"CREATE TABLE {ifNotExists} {tableName} ({columnDefinitions})";
		}
	}
}
