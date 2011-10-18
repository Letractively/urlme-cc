using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Reflection;

namespace urlme.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric<T>(this T obj)
        {
            const string sysType = "SYSTEM.INT16,SYSTEM.INT32,SYSTEM.INT64,SYSTEM.SINGLE,SYSTEM.DOUBLE,SYSTEM.LONG,SYSTEM.DECIMAL,SYSTEM.BYTE,SYSTEM.SHORT,SYSTEM.USHORT,SYSTEM.UINT,SYSTEM.FLOAT,SYSTEM.ULONG";
            foreach (string substring in sysType.Split(','))
                if (obj.GetType().ToString().ToUpper() == substring.ToString().ToUpper())
                    return true;
            return false;
        }

        public static string ToString(this object obj, int MaxLength)
        {
            return obj.ToString(MaxLength, false);
        }

        public static string ToString(this object obj, int MaxLength, bool showContinuation)
        {
            string txt = obj as string;
            if (!String.IsNullOrEmpty(txt) && txt.Length > MaxLength)
                return txt.Substring(0, MaxLength) + ((showContinuation) ? "..." : string.Empty);
            return txt;
        }

        public static string TrimToNull(this string text)
        {
            if (string.IsNullOrEmpty((text ?? string.Empty).ToString().Trim()))
                return null;
            return text.Trim();
        }

        public static string HighlightKeywords(this string text, string keywords)
        {
            if (text == null || text.Length == 0 || keywords == null || keywords.Length == 0)
                return text;

            text = text.Trim();
            text = text.Replace("\"", "");
            text = Regex.Replace(text, "'[^s]+", "");
            if (text.EndsWith("'"))
                text = text.Remove(text.Length - 1, 1);

            string pattern = keywords.Replace("?", "[a-z]").Replace("*", "[a-z]*");
            pattern = "\\b" + pattern.Replace(" ", "\\b|\\b") + "\\b";
            Regex exp = new Regex(pattern, RegexOptions.IgnoreCase);
            return exp.Replace(text, new MatchEvaluator(HighlightMatch));

        }

        private static string HighlightMatch(Match m)
        {
            return "<span class=\"Highlight\">" + m.Value + "</span>";
        }

        public static string StripHtml(this object text)
        {
            if (text == null)
                return text as string;
            string val = Regex.Replace(text.ToString(), "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>", "");
            val = Regex.Replace(val, "&[a-z]+;", ""); // &nbsp; &quot; etc
            return val.Replace("<!--", "");
        }

        public static object ReplaceWhenNullWith(this object obj, object replacementObject)
        {
            return obj ?? replacementObject;
        }

        public static string ToJSON<T>(this T obj)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                //serialize data to a stream, then to a JSON string
                DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof(T));
                jsSerializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        public static bool IsEmail(this string text)
        {
            // Return true if text is a valid e-mail format.
            return Regex.IsMatch(text ?? string.Empty, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static string ToSeoString(this string text)
        {
            if (text == null)
                return text;
            // ÁÉÍÑÓÚÜ¡¿áéíñóúü
            return Regex.Replace(text.Trim().ToLower().Replace("&", "and").Replace("'", ""), "[^a-zA-Z0-9-][ÁÉÍÑÓÚÜáéíñóúü]", "-");
        }

        public static bool IsGuid(this string text)
        {
            return Regex.IsMatch(text ?? "", @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$");
        }
    }
}
