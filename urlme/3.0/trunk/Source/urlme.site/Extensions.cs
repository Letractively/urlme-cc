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
    }
}