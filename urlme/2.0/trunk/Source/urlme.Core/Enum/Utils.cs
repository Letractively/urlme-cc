using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Core.Enum
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
    }
}
