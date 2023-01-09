using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickSQLite.Typing
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Returns the SQLite data type for the specified .NET type.
		/// </summary>
		/// <param name="type">The .NET type.</param>
		/// <returns>The SQLite data type.</returns>
		/// <exception cref="ArgumentException">Thrown if the specified type is not supported.</exception>
		/// <remarks>ALL HAIL TO CHATGPT</remarks>
		public static string GetSQLiteDataType(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			if (type == typeof(int) || type == typeof(long))
			{
				return "INTEGER";
			}
			else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
			{
				return "REAL";
			}
			else if (type == typeof(string))
			{
				return "TEXT";
			}
			else if (type == typeof(bool))
			{
				return "BOOLEAN";
			}
			else if (type == typeof(byte[]))
			{
				return "BLOB";
			}
			else
			{
				throw new ArgumentException($"The type '{type.Name}' is not supported. Only the following types are supported: int, long, float, double, decimal, string, bool, byte[].");
			}
		}

		/// <summary>
		/// Determines whether the specified PropertyInfo is nullable.
		/// </summary>
		/// <param name="propertyInfo">The PropertyInfo to check.</param>
		/// <returns>True if the PropertyInfo is nullable, false otherwise.</returns>
		public static bool IsNullableType(this PropertyInfo propertyInfo)
		{
			if (!propertyInfo.PropertyType.IsValueType)
			{
				// Make an exception for string which are by default always nullable
				if (propertyInfo.PropertyType == typeof(string))
				{
					// Check if a CustomAttribute for the ? exists, and return true
					if ((bool)(propertyInfo.GetMethod?.CustomAttributes.Any(ca => ca.AttributeType.Name == "NullableContextAttribute")))
					{
						return true;
					}
					// If string is not explicitely declared as nullable return false
					return false;
				}

				return true; // ref-type
			}
			if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
			{
				return true; // Nullable<T>
			}
			return false; // value-type
		}
	}
}
