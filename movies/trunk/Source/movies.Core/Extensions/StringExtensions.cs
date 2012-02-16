using System.Web.Script.Serialization;

namespace movies.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Snippet(this string s, int len)
        {
            if (s.Length > len)
            {
                return string.Format("<span alt='{0}' title='{0}'>{1}...</span>", s, s.Substring(0, len));
            }
            return s;            
        }        

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
