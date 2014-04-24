﻿using System;
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

        public static Link Get(int linkId)
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
                var insert = conn.Query<Link>("select LinkId from [ihdavis].[Link] where LinkId=@linkId"
                    , new { linkId = source.LinkId }).FirstOrDefault() == null; // todo: need linkId = ?

                if (insert)
                {
                    return conn.Execute(
                        @"insert [ihdavis].[Link] (UserId,Path,DestinationUrl) 
                          values (@userId,@path,@destinationUrl)"
                        , new { source.UserId, source.Path, source.DestinationUrl }
                        ) == 1;
                }
                else
                {
                    return conn.Execute(
                        @"update [ihdavis].[Link] 
                          set UserId=@userId, Path=@path, DestinationUrl=@destinationUrl 
                          where UserId=@userId and LinkId=@linkId"
                        , new { source.UserId, source.Path, source.DestinationUrl, source.LinkId }
                        ) == 1;
                }
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
                var query = "select LinkId,DestinationUrl,Path,HitCount,CreateDate from [ihdavis].[Link] where UserId=@userId";
                // query = "select * from [ihdavis].[Link] where UserId=@userId";
                var @params = new { userId };

                return conn.Query<Link>(query, @params).ToList();
            }
        }
    }
}
