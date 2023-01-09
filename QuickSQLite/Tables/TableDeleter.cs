using Microsoft.Data.Sqlite;
using NewzIndexerLib.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite.Tables
{
    public class TableDeleter
    {
        public static void DropTable<T>(QSQLiteConnection connection, bool includeIfExists = false)
        {
			string sql = DropTableSql<T>(includeIfExists);
			using SqliteCommand cmd = connection.Connection.CreateCommand();
			cmd.CommandText = sql;
			cmd.ExecuteNonQuery();
		}

        public static void DeleteTableData<T>(QSQLiteConnection connection)
        {
			string sql = DeleteAllSql<T>();
			using SqliteCommand cmd = connection.Connection.CreateCommand();
			cmd.CommandText = sql;
			cmd.ExecuteNonQuery();
		}

		private static string DeleteAllSql<T>()
		{
			Type type = typeof(T);
			string tableName = type.Name;

			return $"DELETE FROM {tableName}";
		}

		private static string DropTableSql<T>(bool includeIfExists = false)
		{
			Type type = typeof(T);
			string tableName = type.Name;

			string ifExists = (includeIfExists) ? "IF EXISTS" : "";

			return $"DROP TABLE {ifExists} {tableName}";
		}

	}
}
