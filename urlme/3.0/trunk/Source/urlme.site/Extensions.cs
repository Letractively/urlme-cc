using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urlme.site
{
    public static class Extensions
    {
        public static int UserId(this string s) {
            return int.Parse(s.Split('^')[0]);
        }

        public static string Email(this string s)
        {
            return s.Split('^')[1];
        }

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