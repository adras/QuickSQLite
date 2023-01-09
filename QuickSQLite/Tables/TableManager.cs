using NewzIndexerLib.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite.Tables
{
    public class TableManager
    {
        public static void CreateTable<T>(QSQLiteConnection connection, bool includeIfNotExists) where T : class
        {
            TableCreater.CreateTable<T>(connection, includeIfNotExists);
        }

        public static void UpdateTable<T>(QSQLiteConnection connection) where T : class
        {
            TableUpdater.UpdateTable<T>(connection);
        }
    }
}
