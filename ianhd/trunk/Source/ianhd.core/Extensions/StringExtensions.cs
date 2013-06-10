namespace ianhd.core.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string TrimToNull(this String text)
        {
            if (String.IsNullOrEmpty((text ?? "").ToString().Trim()))
                return null;
            return text.Trim();
        }
    }
}
