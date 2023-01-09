using QuickSQLite;
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

			QTableManager.CreateTable<Customer>(connection, true);

			QTableManager.DeleteTableData<Person>(connection);

			QTableManager.DropTable<Person>(connection);
			QTableManager.DropTable<Person>(connection, true);

		}
	}
}