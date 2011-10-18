using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace urlme.Utils.Extensions
{
    public static class DataExtensions
    {
        public static bool ContainsData(this System.Data.DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)
                return true;
            return false;
        }

        public static IEnumerable<T> ToTypedList<T>(this DataSet ds)
        {
            IList<T> ret = new List<T>();
            if (ds.ContainsData())
            {
                PropertyInfo[] pi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, new ParameterModifier[] { });
                    T obj = (T)constructor.Invoke(null);
                    foreach (PropertyInfo p in pi)
                    {
                        if (ds.Tables[0].Columns.Contains(p.Name))
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(dr[p.Name].ToString()))
                                {
                                    Type columnType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                                    object value = dr[p.Name];
                                    p.SetValue(obj, Convert.ChangeType(value, columnType), null);
                                }
                            }
                            catch (System.Exception)
                            {
                                /*Silent*/
                            }
                        }
                    }
                    ret.Add((T)Convert.ChangeType(obj, typeof(T)));
                }
            }
            return ret;
        }
    }
}
