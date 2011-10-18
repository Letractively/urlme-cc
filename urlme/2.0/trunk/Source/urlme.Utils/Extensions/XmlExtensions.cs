using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace urlme.Utils.Extensions
{
    public static class XmlExtensions
    {
        public static string XmlSerializeToString(this object obj)
        {
            string xml = string.Empty;
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                ser.Serialize(ms, obj);
                xml = Encoding.UTF8.GetString(ms.ToArray());
            }

            return xml;
        }



        public static T XmlDeserializeFromString<T>(this string xml)
        {
            using (MemoryStream ms = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(ms);
            }
        }
    }
}
