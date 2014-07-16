namespace ianhd.core.Extensions
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string TrimToNull(this String text)
        {
            if (String.IsNullOrEmpty((text ?? "").ToString().Trim()))
                return null;
            return text.Trim();
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

        public static string RemoveExtraDashes(this string s)
        {
            while (s.Contains("--"))
            {
                s = s.Replace("--", "-");
            }

            s = s.Trim("-".ToCharArray());

            return s;
        }

        public static string StripHtml(this string s)
        {
            return Regex.Replace(s, "<.*?>", string.Empty);
        }
    }
}
