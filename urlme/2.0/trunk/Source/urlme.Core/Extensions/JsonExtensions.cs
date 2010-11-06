using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace urlme.Core.Extensions
{
    public static class JsonExtensions
    {
        public static string JsonSerializeToString(this object obj)
        {
            string json = string.Empty;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            return json;
        }



        public static T JsonDeserializeFromString<T>(this string json)
        {
            using (MemoryStream ms = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }
        }
    }
}
