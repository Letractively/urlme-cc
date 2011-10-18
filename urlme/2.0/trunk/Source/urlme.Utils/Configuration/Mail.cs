// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mail.cs" company="Salem Web Network">
//   2010
// </copyright>
// <summary>
//   Provides access to Mail-related configuration data/settings
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace urlme.Utils.Configuration
{
    /// <summary>
    /// Provides access to Mail-related configuration data/settings
    /// </summary>
    public static class Mail
    {
        #region Properties
        /// <summary>
        /// Gets the smtp host
        /// </summary>
        public static string SmtpHost
        {
            get
            {
                return ConfigurationManager.Instance.AppSettings["SmtpHost"];
            }
        }

        /// <summary>
        /// Gets the Smtp port
        /// </summary>
        public static int SmtpPort
        {
            get
            {
                return int.Parse(ConfigurationManager.Instance.AppSettings["SmtpPort"]);
            }
        }

        /// <summary>
        /// Gets the FROM address for use for emails sent from the admin
        /// </summary>
        public static string AdminEmailFromAddress
        {
            get { return ConfigurationManager.Instance.AppSettings["AdminEmailFromAddress"]; }
        } 
        #endregion
    }
}
