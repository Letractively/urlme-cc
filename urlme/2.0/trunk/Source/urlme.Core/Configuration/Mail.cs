using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Core.Configuration
{
    public static class Mail
    {

        public static string SmtpHost
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["SmtpHost"];
            }
        }

        public static int SmtpPort
        {
            get
            {
                return int.Parse(ConfigurationManager.Instance.AppSettings["SmtpPort"]);
            }
        }
    }
}
