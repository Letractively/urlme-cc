using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data.Extensions;
using ianhd.core.Data;

namespace urlme.data.Models
{
    public sealed partial class Link
    {
        [IgnoreField]
        public string LongUrl
        {
            get
            {
                return this.DestinationUrl.Snippet();
            }
        }

        [IgnoreField]
        public string ShortUrl
        {
            get
            {
                return "http://urlme.cc/" + this.Path;
            }
        }

        [IgnoreField]
        public string Created
        {
            get
            {
                return this.CreateDate.ToString("MM/dd/yyyy");
            }
        }

        [IgnoreField]
        public string Hits
        {
            get
            {
                return this.HitCount.ToString("#,#0");
            }
        }

        public static bool Delete(int linkId)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "delete from [ihdavis].[Link] where LinkId=@linkId";
                var @params = new { linkId };

                return conn.Execute(query, @params) == 1;
            }
        }

        public static List<Link> Get(int userId)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "select * from [ihdavis].[Link] where UserId=@userId";
                var @params = new { userId };

                return conn.Query<Link>(query, @params).ToList();
            }
        }
    }
}
