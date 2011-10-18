using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Utils.Configuration
{
    public static class Database
    {
        public static ConnectionStringSettings Default
        {
            get
            {
                return ConfigurationManager.Instance.ConnectionStrings["Default"];
            }
        }
    }
}
