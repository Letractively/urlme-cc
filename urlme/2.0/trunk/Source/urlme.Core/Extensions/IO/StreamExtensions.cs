using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace urlme.Core.Extensions.IO
{
    public static class StreamExtensions
    {
        public static string StreamToString(this System.IO.Stream stream)
        {
            string ret = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                ret = reader.ReadToEnd();
            }
            return ret;
        }

        public static string StreamToString(this System.IO.MemoryStream stream)
        {
            string ret = string.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                ret = reader.ReadToEnd();
            }
            return ret;
        }
    }
}
