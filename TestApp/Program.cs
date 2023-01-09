using NewzIndexerLib.Database;
using QuickSQLite.Tables;
using TestApp.Models;

namespace TestApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Every library needs to be used. This app uses the library to check out how it
			// feels like

			QSQLiteConnection connection = new QSQLiteConnection();
			connection.Open();

			TableManager.CreateTable<Person>(connection, true);

			//TableManager.UpdateTable<AnotherPerson>(connection)
		}
	}
}