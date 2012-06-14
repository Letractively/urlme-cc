namespace movies.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;
    using movies.Data;
    using System.Reflection;
    using System.Data.Linq.Mapping;

    public class RepositoryBase
    {
        protected bd13DataContext CreateContext()
        {
            return this.CreateContext(false);
        }

        protected bd13DataContext CreateContext(int timeoutSeconds)
        {
            return this.CreateContext(false, false, timeoutSeconds);
        }

        protected bd13DataContext CreateContext(bool objectTrackingEnabled)
        {
            return this.CreateContext(objectTrackingEnabled, false, 60);
        }

        protected bd13DataContext CreateContext(bool objectTrackingEnabled, bool deferredLoadingEnabled, int timeoutSeconds = 60)
        {
            string connectionString = "tofix"; // Core.Configuration.ConfigurationManager.Instance.ConnectionStrings["CrossBridge"].ConnectionString;

            var context = new bd13DataContext(connectionString)
            {
                ObjectTrackingEnabled = objectTrackingEnabled,
                DeferredLoadingEnabled = deferredLoadingEnabled,
                CommandTimeout = timeoutSeconds // default is just 30 secs.
            };

            return context;
        }

        protected System.Data.SqlClient.SqlConnection CreateConnection(int timeoutSeconds)
        {
            string connectionString = "tofix"; // Core.Configuration.ConfigurationManager.Instance.ConnectionStrings["CrossBridge"].ConnectionString;
            connectionString += string.Format(";Timeout={0}", timeoutSeconds);
            var cn = new System.Data.SqlClient.SqlConnection(connectionString);
            return cn;
        }

        protected static PropertyInfo GetPrimaryKey<T>()
        {
            PropertyInfo[] infos = typeof(T).GetProperties();

            foreach (PropertyInfo info in infos)
            {
                var column = info.GetCustomAttributes(false)
                    .Where(x => x.GetType() == typeof(ColumnAttribute))
                    .FirstOrDefault(x => ((ColumnAttribute)x).IsPrimaryKey && ((ColumnAttribute)x).DbType.Contains("NOT NULL"));
                if (column != null)
                {
                    return info;
                }
            }

            throw new NotSupportedException(typeof(T).ToString() + " has no Primary Key");
        }

        protected static PropertyInfo GetProperty<TEntity>(string propertyName)
        {
            PropertyInfo[] infos = typeof(TEntity).GetProperties();

            PropertyInfo propInfo = infos.FirstOrDefault(i => i.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)
                && i.GetCustomAttributes(false)
                .Any(x => x.GetType() == typeof(ColumnAttribute)));

            // make sure it's a col
            if (propInfo != null)
            {
                return propInfo;
            }

            throw new NotSupportedException(typeof(TEntity).ToString() + " has no '" + propertyName + "' field");
        }
    }
}
