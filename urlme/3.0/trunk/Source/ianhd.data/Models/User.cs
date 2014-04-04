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
                return conn.Query<User>("select * from [bakersdozen132].[ihdavis].[User] where Email=@email", new { email }).FirstOrDefault();
            }
        }
    }
}
