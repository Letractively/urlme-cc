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
                return _db ?? (_db = ianhd.core.Data.Database.CreateDatabase("conn-str"));
            }
        }
    }
}
