using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;

namespace movies.Model
{
    public class PostalCode
    {
        public static string GetShowtimes(string date, string zip)
        {
            return Cache.GetValue<string>(
                string.Format("codejkjk.movies.Model.PostalCode.GetShowtimes-{0}-{1}", date, zip),
                () =>
                {
                    return API.Flixster.GetTheaters(date, zip);
                });
        }
    }
}
