using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Library.Configuration
{
    public class Page
    {
        public static int DestinationUrlSnippetLength
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DestinationUrlSnippetLength"]);
            }
        }
    }
}
