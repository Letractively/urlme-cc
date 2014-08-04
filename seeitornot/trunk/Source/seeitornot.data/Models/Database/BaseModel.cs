using ianhd.core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeitornot.data.Models.Database
{
    class BaseModel
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
                var connStr = "Data Source=sqlserver8.loosefoot.com;Initial Catalog=seeitornot;Persist Security Info=True;User ID=seeitornot;Password=3Am1g0s";
                return _db ?? (_db = ianhd.core.Data.Database.CreateDatabase(connStr));
            }
        }
    }
}
