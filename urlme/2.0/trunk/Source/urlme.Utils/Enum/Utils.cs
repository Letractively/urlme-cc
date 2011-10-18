using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace urlme.Utils.Enum
{
    public static class Utils
    {
        public static T EnumQuery<T>(string query)
        {
            if (!String.IsNullOrEmpty(query))
                foreach (string e in System.Enum.GetNames(typeof(T)))
                    if (e.ToLower().Contains(query.ToLower()))
                        return (T)System.Enum.Parse(typeof(T), e);
            return Activator.CreateInstance<T>();
        }

        #region HashTable

        public static Hashtable GetEnumForBind(Type enumeration)
        {
            string[] names = System.Enum.GetNames(enumeration);
            Array values = System.Enum.GetValues(enumeration);
            Hashtable ht = new Hashtable();
            for (int i = 0; i < names.Length; i++)
            {
                ht.Add(Convert.ToInt32(values.GetValue(i)).ToString(), names[i]);
            }
            return ht;
        }

        #endregion
    }
}
