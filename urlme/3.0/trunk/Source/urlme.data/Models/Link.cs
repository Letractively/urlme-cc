using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data.Extensions;
using ianhd.core.Data;
using ianhd.core.Net;
using System.Net;

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

        public static Link GetByLinkId(int linkId)
        {
            using (var conn = Db.CreateConnection())
            {
                return conn.Query<Link>("select * from [ihdavis].[Link] where LinkId=@linkId", new { linkId }).FirstOrDefault();
            }
        }

        public static Link Get(string path)
        {
            using (var conn = Db.CreateConnection())
            {
                return conn.Query<Link>("select * from [ihdavis].[Link] where Path=@path", new { path }).FirstOrDefault();
            }
        }
        
        public static bool Save(Link source)
        {
            using (var conn = Db.CreateConnection())
            {
                return false;
            }
        }

        public static bool Delete(int linkId, int userId)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "delete from [ihdavis].[Link] where LinkId=@linkId and UserId=@userId";
                var @params = new { linkId, userId };

                return conn.Execute(query, @params) == 1;
            }
        }

        public static List<Link> GetByUserId(int userId)
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
