using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite
{
	public interface IQModelCached
	{ }

	public interface IQModel<T> : IQModelCached
	{
		public void CreateObjectFromCurrentRow<T>(SqliteDataReader reader);

		public Dictionary<string, object> CreateValueDictionary();

		public string QName { get; }
	}

	internal class QReflectionModelCache
	{
		private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> propertyCache = new Dictionary<Type, IEnumerable<PropertyInfo>>();

		internal static IEnumerable<PropertyInfo> GetPropertiesForType(Type type)
		{
			if (!propertyCache.ContainsKey(type))
			{
				IEnumerable<PropertyInfo> properties = GetProperties(type);
				propertyCache[type] = properties;
			}
			return propertyCache[type];
		}

		internal static IEnumerable<PropertyInfo> GetPropertiesForType<T>() where T : IQModelCached
		{
			Type type = typeof(T);
			IEnumerable<PropertyInfo> result = GetPropertiesForType(type);

			return result;
		}

		private static IEnumerable<PropertyInfo> GetProperties(Type itemType)
		{
			// TODO: Add an attribute to allow the user to ignore properties
			IEnumerable<PropertyInfo> properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// Ignore QName since it's an internal feature. The QName is not supposed to exist
			// as a column in the database. Therefore this property should not be included
			// in any queries which are generated.
			IEnumerable<PropertyInfo> result = properties.Where(p => p.Name != "QName");

			return result;
		}
	}

	public class QReflectionModel<T> : IQModel<T> where T : IQModelCached
	{
		public QReflectionModel()
		{
		}

		public string QName
		{
			get
			{
				string name = typeof(T).Name;
				return name;
			}
		}

		public void CreateObjectFromCurrentRow<T>(SqliteDataReader reader)
		{
			Type thisType = GetType();
			Type baseType = thisType.BaseType;

			// TODO: Implement next call and foreach loop
			IEnumerable<PropertyInfo> x = QReflectionModelCache.GetPropertiesForType(baseType);

			//foreach (PropertyInfo property in x)
			//{
			//	// Now things are getting a bit tricky.
			//	// Reader allows only to get a column by number, not by name,
			//	// so we need a cache which allows to look up the number by a property name

			//	// Then depending on the type of the property, we need to call
			//	// getstring, get int etc

			/*
			Type information:
			
			reader.GetFieldType(int ordinal)
			will return the .NET type of the field, while:

			reader.GetDataTypeName(int ordinal)
			will return a string representing the data type of the field in the data source (e.g. varchar). GetFieldType is likely to be more useful to you given the use case you describe

			*/
			//	//property.SetValue(this)
			//}

			return;
		}

		public Dictionary<string, object> CreateValueDictionary()
		{
			IEnumerable<PropertyInfo> properties = QReflectionModelCache.GetPropertiesForType<T>();
			Dictionary<string, object> values = new Dictionary<string, object>();

			foreach (PropertyInfo p in properties)
			{
				string name = p.Name;
				object value = p.GetValue(this);
				values.Add(name, value);
			}
			return values;
		}
	}
}
