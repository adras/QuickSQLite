using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewzIndexerLib.Database
{
    public class QDbConfiguration
    {
        static string SqLiteFileName { get; set; } = "TheNewzDatabase.sqlite";

        static string ConnectionStringTemplate { get; set; } = @"Data Source={0}";

        public static string CreateConnectionString(string dbName = null)
        {
            if (dbName == null)
            {
                dbName = SqLiteFileName;
            }

            string connectionString = string.Format(ConnectionStringTemplate, dbName);
            return connectionString;
        }
    }
}
