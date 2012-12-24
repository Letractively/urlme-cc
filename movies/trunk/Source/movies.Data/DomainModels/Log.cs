using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using movies.Core.Extensions;

namespace movies.Data.DomainModels
{
    public class Log
    {
        private static readonly Data.Repository.DirectRepository repo = new Repository.DirectRepository();

        public static bool Save(string message, Enumerations.LogType logType)
        {
            Data.Log dbLog = new Data.Log
            {
                CreateDate = System.DateTime.Now,
                LogType = logType.ToString(),
                Message = message,
                ModifyDate = null
            };

            return repo.LogSave(dbLog);
        }
    }
}
