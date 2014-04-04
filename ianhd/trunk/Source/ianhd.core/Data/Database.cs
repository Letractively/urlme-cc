namespace ianhd.core.Data
{
    public sealed class Database
    {
        public static Database CreateDatabase(string connectionString)
        {
            return new Database(connectionString);
        }

        public string ConnectionString { get; private set; }

        internal Database(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public Connection CreateConnection()
        {
            return new Connection(this.ConnectionString);
        }
    }
}