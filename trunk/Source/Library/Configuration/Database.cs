using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Library.Configuration
{
    public class Database
    {
        public static string Default
        {
            get { return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;  }
        }
    }
}
