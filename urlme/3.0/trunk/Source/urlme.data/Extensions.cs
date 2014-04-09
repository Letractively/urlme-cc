using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urlme.data
{
    public static class Extensions
    {
        public static string Snippet(this string s, int ifMoreThan = 30)
        {
            if (string.IsNullOrWhiteSpace(s)) { return s; }

            s = s.Replace("http://", "").Replace("https://", "");

            if (s.Length > ifMoreThan)
            {
                return s.Substring(0, ifMoreThan) + "...";
            }

            return s;
        }
    }
}
