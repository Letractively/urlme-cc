using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Web;

namespace play.Site.Models
{
    public class Object
    {
        public bool ToggleProperty<TEntity>(string entityId, string propertyName) where TEntity : class
        {
            try
            {
                using (var ctx = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    List<TEntity> records = ctx.GetTable<TEntity>().ToList();

                    PropertyInfo primaryKey = GetPrimaryKey<TEntity>();
                    TEntity record = records.FirstOrDefault(x => Convert.ToString(primaryKey.GetValue(x, null)) == entityId);

                    PropertyInfo prop = GetProperty<TEntity>(propertyName);
                    bool currentValue = Convert.ToBoolean(prop.GetValue(record, null));

                    prop.SetValue(record, !currentValue, null);

                    // try and update DateModified col
                    PropertyInfo dateModifiedProp = GetProperty<TEntity>("ModifyDate");
                    dateModifiedProp.SetValue(record, System.DateTime.Now, null);

                    ctx.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SetProperty<TEntity>(string entityId, string propertyName, object newValue) where TEntity : class
        {
            try
            {
                using (var context = new PlayDataContext { ObjectTrackingEnabled = true })
                {
                    List<TEntity> records = context.GetTable<TEntity>().ToList();

                    PropertyInfo primaryKey = GetPrimaryKey<TEntity>();
                    TEntity record = records.FirstOrDefault(x => Convert.ToString(primaryKey.GetValue(x, null)) == entityId);

                    PropertyInfo prop = GetProperty<TEntity>(propertyName);
                    prop.SetValue(record, newValue, null);

                    // try and update DateModified col
                    PropertyInfo dateModifiedProp = GetProperty<TEntity>("ModifyDate");
                    dateModifiedProp.SetValue(record, System.DateTime.Now, null);

                    context.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
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