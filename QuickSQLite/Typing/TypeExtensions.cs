using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Determines whether the specified type is nullable.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>True if the type is nullable, false otherwise.</returns>
		public static bool IsNullableType(this Type type)
		{
			if (!type.IsValueType) return true; // ref-type
			if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
			return false; // value-type
		}
	}
}
