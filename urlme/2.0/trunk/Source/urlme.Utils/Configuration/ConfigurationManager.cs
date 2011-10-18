using System;
using System.Collections.Specialized;
using System.Web.Caching;
using System.Xml;
using System.IO;
using System.Linq;
using System.Web;

namespace urlme.Utils.Configuration
{
    /// <summary>
    /// Handles pulling the configuration based on the environment
    /// </summary>
    public sealed class ConfigurationManager
    {
        #region Variables
        /// <summary>
        /// the default configuration file to use
        /// </summary>
        private static string DefaultConfigurationFile = "urlme.Utils.config";

        /// <summary>
        /// the cache key for the configuration key
        /// </summary>
        //private static string ConfigurationManagerCacheKey = "urlme.Utils.Configuration.ConfigurationManager.Instance";

        /// <summary>
        /// the settings environment
        /// </summary>
        private static string settingsEnvironment = string.Empty;

        /// <summary>
        /// the default instance of the configuration manager
        /// </summary>
        private static ConfigurationManager defaultInstance;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the default singleton instance
        /// </summary>
        static ConfigurationManager()
        {
            ConfigurationManager.defaultInstance = new ConfigurationManager();
        }

        /// <summary>
        /// Initializes a new instance of the ConfigurationManager class
        /// </summary>
        private ConfigurationManager()
            : this(ConfigurationManager.DefaultConfigurationFile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ConfigurationManager class
        /// </summary>
        /// <param name="configFile">the name of the configuration file to load</param>
        private ConfigurationManager(string configFile)
        {
            this.ConfigurationFilename = configFile;
            this.Refresh(); // load the data from the configuration
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the configuration file
        /// </summary>
        public string ConfigurationFilename
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the configuration file including the full path
        /// </summary>
        public string ConfigurationFileFullPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the configuration document
        /// </summary>
        public XmlDocument ConfigurationDocument
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the settings environment, e.g. localhost, dev, prod
        /// </summary>
        public string SettingsEnvironment
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the application settings
        /// </summary>
        public NameValueCollection AppSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the connection string settings
        /// </summary>
        public System.Configuration.ConnectionStringSettingsCollection ConnectionStrings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the settings environment
        /// </summary>
        public static string CurrentSettingsEnvironment
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.settingsEnvironment))
                {
                    lock (ConfigurationManager.settingsEnvironment)
                    {
                        string machineName = Environment.MachineName.ToUpper();
                        ConfigurationManager.settingsEnvironment = "/localhost";
                        if (System.Configuration.ConfigurationManager.AppSettings["DevelopmentNames"].Split(',').Where(x => machineName.Contains(x)).Count() > 0)
                        {
                            ConfigurationManager.settingsEnvironment = "/dev";
                        }
                        else if (System.Configuration.ConfigurationManager.AppSettings["ProductionNames"].Split(',').Where(x => machineName.Contains(x)).Count() > 0)
                        {
                            ConfigurationManager.settingsEnvironment = "/prod";
                        }

                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["EnvironmentOverride"]))
                        {
                            ConfigurationManager.settingsEnvironment = System.Configuration.ConfigurationManager.AppSettings["EnvironmentOverride"].ToString();
                        }
                    }
                }

                return ConfigurationManager.settingsEnvironment;
            }
        }

        /// <summary>
        /// Gets the default instance of the ConfigurationManager class
        /// </summary>
        public static ConfigurationManager Instance
        {
            get { return ConfigurationManager.defaultInstance; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Refreshes the configuration data
        /// </summary>
        public void Refresh()
        {
            lock (this)
            {
                this.ConfigurationFileFullPath = ConfigurationManager.GetConfigurationFileFullPath(this.ConfigurationFilename);
                this.ConfigurationDocument = ConfigurationManager.GetConfigurationDocument(this.ConfigurationFileFullPath);
                this.SettingsEnvironment = ConfigurationManager.CurrentSettingsEnvironment;
                this.AppSettings = ConfigurationManager.GetAppSettings(this.ConfigurationDocument, this.SettingsEnvironment);
                this.ConnectionStrings = ConfigurationManager.GetConnSettings(this.ConfigurationDocument, this.SettingsEnvironment);
            }
        }

        /// <summary>
        /// Gets the configuration node, from cache
        /// </summary>
        /// <param name="doc">the configuration document</param>
        /// <param name="settingsSection">the settings node to retrieve</param>
        /// <returns>the settings node</returns>
        private static XmlNode ConfigurationNode(XmlDocument doc, string settingsSection)
        {
            XmlNode xmlNode = doc.SelectSingleNode("configuration");
            foreach (string node in settingsSection.Split('/'))
            {
                xmlNode = xmlNode.SelectSingleNode(node);
            }

            return xmlNode;
        }

        /// <summary>
        /// Gets the configuration file name
        /// </summary>
        /// <returns>the name of the configuration file</returns>
        private static string GetConfigurationFileFullPath(string configurationFile)
        {
            return Path.Combine(ConfigurationManager.GetConfigurationFullPath(), configurationFile);
        }

        /// <summary>
        /// Gets the configuration path
        /// </summary>
        /// <returns>the path to the configuration file</returns>
        private static string GetConfigurationFullPath()
        {
            string appDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!appDirectory.Contains("bin") && Directory.Exists(Path.Combine(appDirectory, "bin")))
            {
                appDirectory = Path.Combine(appDirectory, "bin");
            }

            return appDirectory;
        }

        /// <summary>
        /// Gets the configuration document, avoiding the cache
        /// </summary>
        /// <param name="fileFullPath">the filename of the config to load, including the full path</param>
        /// <returns>the settings node</returns>
        private static XmlDocument GetConfigurationDocument(string fileFullPath)
        {
            if (!System.IO.File.Exists(fileFullPath))
            {
                return null;
            }

            using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(fileFullPath))
            {
                System.Xml.XmlDocument tempdoc = new System.Xml.XmlDocument();
                tempdoc.Load(reader);
                return tempdoc;
            }
        }

        /// <summary>
        /// Gets the AppSettings for a specific settingsEnvironment for the given config
        /// </summary>
        /// <param name="config">the configuration to read from</param>
        /// <param name="settingsEnvironment">the settings environment to retrieve the values for</param>
        /// <returns>the app settings</returns>
        private static NameValueCollection GetAppSettings(XmlDocument config, string settingsEnvironment)
        {
            NameValueCollection returnSettings = new NameValueCollection(); // settings collection that we'll return

            XmlNode envSettingsNode = ConfigurationManager.ConfigurationNode(config, "appSettings" + settingsEnvironment);
            if (envSettingsNode == null)
            {
                return returnSettings;
            }

            XmlNodeList envSettings = envSettingsNode.SelectNodes("add");

            // e.g., appSettings/localhost/add1, appSettings/localhost/add2, etc.
            foreach (XmlNode envSetting in envSettings)
            {
                // add legitimate (non null and non empty key and value) environment-specific setting to return settings collection
                if (!string.IsNullOrEmpty(envSetting.Attributes["key"].Value) && !string.IsNullOrEmpty(envSetting.Attributes["value"].Value))
                {
                    returnSettings.Add(envSetting.Attributes["key"].Value, envSetting.Attributes["value"].Value);
                }
            }

            // inject global settings into environment-specific settings collection, 
            // unless the global setting is already in environment-specific settings
            // collection, in which case the environment-specific setting wins
            System.Xml.XmlNode globalSettingsNode = ConfigurationManager.ConfigurationNode(config, "appSettings/global");
            System.Xml.XmlNodeList globalSettings = globalSettingsNode.SelectNodes("add");

            // for each value in appSettings/global collection
            foreach (System.Xml.XmlNode globalSetting in globalSettings)
            {
                // if value does not exist (null) in environment-specific settings collection && the global setting has a nonnullorempty value to add
                if (returnSettings[globalSetting.Attributes["key"].Value] == null && !string.IsNullOrEmpty(globalSetting.Attributes["value"].Value))
                {
                    // inject the global setting into the return settings collection
                    returnSettings.Add(globalSetting.Attributes["key"].Value, globalSetting.Attributes["value"].Value);
                }
            } // next value in appSettings/global collection

            return returnSettings;
        }

        /// <summary>
        /// Gets the connection string settings from a given configuration for the given environment
        /// </summary>
        /// <param name="config">the configuration to read from</param>
        /// <param name="settingsEnvironment">the environment to pull the settings for</param>
        /// <returns>the connection string settings</returns>
        private static System.Configuration.ConnectionStringSettingsCollection GetConnSettings(XmlDocument config, string settingsEnvironment)
        {
            System.Configuration.ConnectionStringSettings cs = default(System.Configuration.ConnectionStringSettings);
            System.Configuration.ConnectionStringSettingsCollection csc = new System.Configuration.ConnectionStringSettingsCollection();

            System.Xml.XmlNode connectionStringsNode = ConfigurationManager.ConfigurationNode(config, "connectionStrings" + settingsEnvironment);
            if (connectionStringsNode == null)
            {
                return csc;
            }

            System.Xml.XmlNodeList nodes = connectionStringsNode.SelectNodes("add");
            foreach (System.Xml.XmlNode node in nodes)
            {
                cs = new System.Configuration.ConnectionStringSettings();
                cs.ConnectionString = node.Attributes["connectionString"].Value;
                cs.Name = node.Attributes["name"].Value;
                cs.ProviderName = node.Attributes["providerName"].Value;
                csc.Add(cs);
            }

            return csc;
        }

        #endregion
    }
}