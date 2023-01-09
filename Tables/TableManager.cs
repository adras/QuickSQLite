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
        public static void CreateTable<T>(QSQLiteConnection connection) where T : class
        {
            TableCreater.CreateTable<T>(connection);
        }
    }
}
