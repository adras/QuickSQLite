using QuickSQLite;
using QuickSQLite.Tables;
using System.Diagnostics;
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

			//QTableManager.CreateTable<Person>(connection, true);
			//QTableManager.DeleteTableData<Person>(connection);

			//QTableManager.DeleteTableData<QSelectablePerson>(connection);
			QTableManager.CreateTable<QSelectablePerson>(connection, true);


			//QTableManager.DropTable<Person>(connection);
			//QTableManager.DropTable<Person>(connection, true);

			Random r = new Random();

			List<QSelectablePerson> persons = new List<QSelectablePerson>();
			for (int i = 0; i < 100000; i++)
			{
				QSelectablePerson p = new QSelectablePerson();
				p.FirstName = r.Next(1029384, 2871944).ToString();
				p.LastName = r.Next(1029384, 2871944).ToString();

				p.Age = r.Next(5, 100);

				Dictionary<string, object> dict = p.CreateValueDictionary();

				persons.Add(p);
			}
			Stopwatch s = new Stopwatch();
			s.Start();
			connection.InsertObjects(persons);
			s.Stop();

			Console.WriteLine($"Insert complete, time: {s.Elapsed.ToString()}");

			s.Restart();
			IEnumerable<QSelectablePerson> queriedPersons = connection.QueryAll<QSelectablePerson>();
			s.Stop();
			Console.WriteLine($"Query call: {s.Elapsed.ToString()}");

			//QueryBuilder builder = new QueryBuilder(connection.Connection);
			//builder.
			//builder.CreateRecord()
		}
	}
}