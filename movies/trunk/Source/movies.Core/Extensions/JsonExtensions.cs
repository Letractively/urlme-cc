using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System;

namespace movies.Core.Extensions
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(this string json)
        {
            return new JavaScriptSerializer().Deserialize<T>(json);
        }

        public static T FromXml<T>(this string xml)
        {
            return (T)FromXml(xml, typeof(T));
        }

        private static object FromXml(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        //public static string ToJson(this object obj)
        //{
        //    return new JavaScriptSerializer().Serialize(obj);
        //}

        //public static T FromXml<T>(this string xml)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(xml);
        //    MemoryStream mem = new MemoryStream(bytes);
        //    return new XmlSerializer(typeof(T)).Deserialize<T>(mem);
        //}

        //public static string Serialize(this object obj)
        //{
        //    MemoryStream mem = new MemoryStream();
        //    XmlSerializer ser = new XmlSerializer(obj.GetType());
        //    ser.Serialize(mem, obj);
        //    ASCIIEncoding ascii = new ASCIIEncoding();
        //    return ascii.GetString(mem.ToArray());
        //}

        //public static object Deserialize(Type typeToDeserialize, string xmlString)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
        //    MemoryStream mem = new MemoryStream(bytes);
        //    XmlSerializer ser = new XmlSerializer(typeToDeserialize);
        //    return ser.Deserialize(mem);
        //}

        //public static string JsonSerializeToString(this object obj)
        //{
        //    string json = string.Empty;
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        ser.WriteObject(ms, obj);
        //        json = Encoding.UTF8.GetString(ms.ToArray());
        //    }

        //    return json;
        //}

        //public static T JsonDeserializeFromString<T>(this string json)
        //{
        //    using (MemoryStream ms = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(json)))
        //    {
        //        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        //        return (T)ser.ReadObject(ms);
        //    }
        //}
    }
}
