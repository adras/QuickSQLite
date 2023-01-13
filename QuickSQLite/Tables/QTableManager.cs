using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite.Tables
{
	public class QTableManager
    {
        public static void CreateTable<T>(QSQLiteConnection connection, bool includeIfNotExists) where T : IQModel<T>
        {
            QTableCreater.CreateTable<T>(connection, includeIfNotExists);
        }

        public static void UpdateTable<T>(QSQLiteConnection connection) where T : class
        {
            QTableUpdater.UpdateTable<T>(connection);
        }

        public static void DropTable<T>(QSQLiteConnection connection, bool includeIfExists = false) where T : class
        {
            QTableDeleter.DropTable<T>(connection, includeIfExists);
        }

		public static void DeleteTableData<T>(QSQLiteConnection connection) where T : class
		{
			QTableDeleter.DeleteTableData<T>(connection);
		}

	}
}
