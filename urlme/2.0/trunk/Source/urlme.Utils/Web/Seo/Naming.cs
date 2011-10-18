namespace urlme.Utils.Web.Seo
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// This class offers tools to help with Seo naming
    /// </summary>
    public static class Naming
    {
        #region Variables
        /// <summary>
        /// Regex for matching all of the characters we do not want in a url
        /// </summary>
        private static readonly Regex RegExUnsupportedCharacters = new Regex(@"[^a-zA-Z0-9\-]");

        /// <summary>
        /// Regex for matching multiple dashes
        /// </summary>
        private static readonly Regex RegExMultiDash = new Regex(@"[\-]+");

        /// <summary>
        /// The separator between words
        /// </summary>
        private const string Separator = "-";
        #endregion

        #region Methods
        /// <summary>
        /// Gets an seo friendly version of a given value (lowercase, not spaces, all alphanumeric)
        /// </summary>
        /// <param name="value">the value to make seo-friendly</param>
        /// <returns>the seo-friendy version</returns>
        public static string GetSeoName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return // replace non-alphanumeric characters, ensure the separator is not display more than once sequentially, convert to lower case
                    Naming.RegExMultiDash.Replace(Naming.RegExUnsupportedCharacters.Replace(value, Naming.Separator),
                                                  Naming.Separator).ToLower();
        } 
        #endregion
    }
}
