using System;
using System.Collections.Generic;
using System.Linq;
using ianhd.core.Data.Extensions;
using ianhd.core.Data;

namespace ianhd.data.Models
{
    public sealed partial class User
    {
        public static User Get(string email)
        {
            using (var conn = Db.CreateConnection())
            {
                var query = "select * from [ihdavis].[User] where Email=@email";
                var @params = new { email };

                var user = conn.Query<User>(query, @params).FirstOrDefault();
                if (user == null)
                {
                    conn.Execute("insert [ihdavis].[User] (Email) values (@email)", @params);
                    user = conn.Query<User>(query, @params).FirstOrDefault();
                }
                return user;
            }
        }
    }
}
