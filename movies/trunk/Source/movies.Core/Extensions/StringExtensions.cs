using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string Slugify(this string s)
        {
            return s.Slugify(40);
        }

        /// <summary>
        /// Removes special characters, converts spaces to "-" and truncates resulting string to outputMaxLength chars
        /// </summary>
        /// <example>"This is $foo @bar" becomes "this-is-foo-bar"</example>
        public static string Slugify(this string s, int outputMaxLength)
        {
            s = s.Trim();

            // First, remove diacritics (accents, umlauts, etc)
            string formD = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < formD.Length; i++)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(formD[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(formD[i]);
                }
            }

            string rtn = Regex.Replace(stringBuilder.ToString(), @"[^\w -]", string.Empty).Replace(" ", "-").RemoveExtraDashes().ToLower();

            if (rtn.Length > outputMaxLength)
            {
                rtn = rtn.Substring(0, outputMaxLength);
            }

            return rtn;
        }

        /// <summary>
        /// Removes multiple occurrences of a dash with a single dash. E.g., "awesome--part-1" becomes "awesome-part-1".
        /// Also removes prefixing and trailing dashes. E.g., "awesome-part-1-" becomes "awesome-part-1".
        /// </summary>
        public static string RemoveExtraDashes(this string s)
        {
            while (s.Contains("--"))
            {
                s = s.Replace("--", "-");
            }

            s = s.Trim("-".ToCharArray());

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
