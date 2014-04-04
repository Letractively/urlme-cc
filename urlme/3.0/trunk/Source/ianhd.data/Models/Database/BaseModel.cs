using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Data;

namespace ianhd.data.Models
{
    public class BaseModel
    {
        private static ianhd.core.Data.Database _db { get; set; }

        static BaseModel()
        {
            // Dapper.SetTypeMap(typeof(Models.BibleTranslation), new ColumnAttributeTypeMapper<Models.BibleTranslation>());
        }

        [IgnoreField]
        protected static ianhd.core.Data.Database Db
        {
            get
            {
                var connStr = "Data Source=sqlserver8.loosefoot.com;Initial Catalog=bakersdozen132;Persist Security Info=True;User ID=ihdavis2;Password=pchang";
                return _db ?? (_db = ianhd.core.Data.Database.CreateDatabase(connStr));
            }
        }
    }
}
