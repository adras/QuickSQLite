using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite
{
    public interface IQSelectableCached
    { }

    public interface IQSelectable<T> : IQSelectableCached
    {
        public T CreateObjectFromCurrentRow(SqliteDataReader reader, SqliteConnection connection);
    }

    internal class QSelectableCache
    {
        private static readonly Dictionary<IQSelectableCached, PropertyInfo[]> propertyCache = new Dictionary<IQSelectableCached, PropertyInfo[]>();

        internal static PropertyInfo[] GetPropertiesForType<T>(T item) where T : IQSelectableCached
        {
            if (!propertyCache.ContainsKey(item))
            {
                PropertyInfo[] properties = GetProperties(item);
                propertyCache[item] = properties;
            }
            return propertyCache[item];
        }

        private static PropertyInfo[] GetProperties<T>(T item) 
        {
            // TODO: Add an attribute to allow the user to ignore properties
            Type itemType = item.GetType();
            PropertyInfo[] props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return props;
        }
    }

    public class QSelectable<T> : IQSelectable<T> where T : IQSelectableCached
    {
        private T instance;

        public QSelectable(T instance)
        {
            this.instance = instance;   
        }

        public T CreateObjectFromCurrentRow(SqliteDataReader reader, SqliteConnection connection)
        {
            PropertyInfo[] x = QSelectableCache.GetPropertiesForType(instance);
            T result = Activator.CreateInstance<T>();
            // TODO: Implement this

            return result;
        }
    }
}
