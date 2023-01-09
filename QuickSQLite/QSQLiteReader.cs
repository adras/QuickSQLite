using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite
{
    public class QSQLiteReader 
    {
        
    }

    public class QSQLiteReaderFactory
    {
        Dictionary<Type, List<PropertyInfo>> propertyCache;

        public QSQLiteReaderFactory()
        {
            propertyCache = new Dictionary<Type, List<PropertyInfo>>();
        }

        public static QSQLiteReader CreateReader (SqliteDataReader sqliteReader, Type classType)
        {
            return null;
        }
    }
}
