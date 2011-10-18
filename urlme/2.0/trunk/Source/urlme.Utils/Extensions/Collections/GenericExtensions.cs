using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace urlme.Utils.Extensions.Collections
{
    public static class GenericExtensions
    {
        public static DataSet ToDataSet(this IList list)
        {
            DataSet ds = new DataSet();
            if (list.Count > 0)
            {
                Type listType = list[0].GetType();
                DataTable dt = new DataTable("Row");
                ds = new DataSet(listType.Name);
                ds.Tables.Add(dt);

                PropertyInfo[] pi = listType.GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    Type columnType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    dt.Columns.Add(new DataColumn(p.Name, columnType));
                }

                foreach (object obj in list)
                {
                    object[] row = new object[pi.Length];
                    int i = 0;

                    foreach (PropertyInfo p in pi)
                    {
                        row[i++] = p.GetValue(obj, null);
                    }

                    dt.Rows.Add(row);
                }
            }
            return ds;
        }
    }
}
