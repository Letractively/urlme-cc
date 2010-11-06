using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;

namespace urlme.Core.Web.Services.Security
{

    public class Cryptography
    {
        public static string Encrypt(string cleanString)
        {
            byte[] clearBytes;
            clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string hashedText = BitConverter.ToString(hashedBytes);
            return hashedText;
        }

    }

    public class KeyChecker
    {
        public static void ValidateSiteKey(string SiteKey)
        {
            string[] siteArray = SiteKey.Split(':');
            //SiteKey takes the form siteCode:uniqueKey (e.g., crossdaily:0CC9F751-B08D-4C77-BBBD-9DB91F6E5B1F)
            if (siteArray.Length != 2)
            {
                throw new ArgumentException(string.Format("The SiteKey '{0}' is invalid.", SiteKey));
            }

            string siteName = siteArray[0];
            System.Guid uniqueKey = default(System.Guid);
            try
            {
                uniqueKey = new System.Guid((siteArray[1]));
            }
            catch (Exception)
            {
                // empty
                uniqueKey = new System.Guid();
            }

            ValidateKey(siteName, uniqueKey);
        }


        private static void ValidateKey(string siteCode, Guid uniqueKey)
        {
            // get configuration information from app's config file
            NameValueCollection config = default(NameValueCollection);
            config = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("sites/" + siteCode);
            if (config == null)
            {
                throw new ArgumentException(string.Format("The siteCode '{0}' is invalid.", siteCode));
            }

            // ******* VERIFY THAT A SITE'S UNIQUE KEY IS CORRECT ***********
            System.Guid expectedKey = new System.Guid(config["uniqueKey"]);
            if (!uniqueKey.Equals(expectedKey))
            {
                throw new ArgumentOutOfRangeException("The unique key provided in the sitekey field is invalid");

            }
        }

        public static string GetSiteAttribute(string SiteKey, string KeyName)
        {
            string[] siteArray = SiteKey.Split(':');
            //SiteKey takes the form siteCode:uniqueKey (e.g., crossdaily:0CC9F751-B08D-4C77-BBBD-9DB91F6E5B1F)
            if (siteArray.Length != 2)
            {
                throw new ArgumentException(string.Format("The SiteKey '{0}' is invalid.", SiteKey));
            }
            NameValueCollection config = default(NameValueCollection);
            config = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("sites/" + siteArray[0]);
            string response = config[KeyName];
            if (response == null)
            {
                throw new ArgumentException("The key name/value pair was not found in the list");
            }
            else
            {
                return response;
            }
        }


    }
}