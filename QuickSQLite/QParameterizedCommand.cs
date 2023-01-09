using Microsoft.Data.Sqlite;

namespace NewzIndexerLib.Database
{
    public class QParameterizedCommand : IDisposable
    {
        SqliteCommand cmd;
        Dictionary<string, SqliteParameter> parameters;

        public SqliteCommand Cmd
        {
            get => cmd;
            set => cmd = value;
        }

        public Dictionary<string, SqliteParameter> Parameters
        {
            get => parameters;
            set => parameters = value;
        }

        public QParameterizedCommand(SqliteCommand cmd, Dictionary<string, SqliteParameter> parameters)
        {
            Cmd = cmd;
            Parameters = parameters;
        }



        public void Dispose()
        {
            cmd.Dispose();
        }
    }
}
