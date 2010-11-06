using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static string FlattenToString(this object[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in array)
            {
                sb.Append(obj.ToString());
            }
            return sb.ToString();
        }
    }
}
