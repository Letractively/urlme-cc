using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Core.Configuration
{
    public static class WebServices
    {
        public static string WebServiceKey
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["WebServiceKey"];
            }
        }

        public static string LuceneServiceUrl
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["LuceneServiceUrl"];
            }
        }

        public static string NewsletterServiceUrl
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["NewsletterServiceUrl"];
            }
        }

        public static string ZcastServiceUrl
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["ZcastServiceUrl"];
            }
        }

        public static string eCardServiceUrl
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["eCardServiceUrl"];
            }
        }

        public static string GodTubeFeedUrl
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["GodTubeFeedUrl"];
            }
        }
    }
}
