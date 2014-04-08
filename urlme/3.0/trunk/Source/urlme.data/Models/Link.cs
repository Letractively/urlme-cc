using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data.Extensions;
using ianhd.core.Data;

namespace urlme.data.Models
{
    public sealed partial class Link
    {
        public static List<Link> Get(int userId)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "select * from [ihdavis].[Link] where UserId=@userId";
                var @params = new { userId };

                var results = conn.Query<Link>(query, new { userId }).ToList();

                return results;
            }
        }
    }
}
