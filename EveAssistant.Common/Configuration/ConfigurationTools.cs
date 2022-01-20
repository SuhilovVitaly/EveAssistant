using System;
using System.Collections.Specialized;
using System.Configuration;

namespace EveAssistant.Common.Configuration
{
    public static class ConfigurationTools
    {
        public static string GetConfigOptionalStringValue(string keyName, string defaultValue = "")
        {
            if (string.IsNullOrWhiteSpace(keyName)) return defaultValue;

            if (ConfigurationManager.AppSettings.Get(keyName) != null)
                return ConfigurationManager.AppSettings[keyName];

            return defaultValue;
        }

        public static bool GetConfigOptionalBoolValue(string keyName, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(keyName)) return defaultValue;

            if (ConfigurationManager.AppSettings.Get(keyName) != null)
                return Convert.ToBoolean(ConfigurationManager.AppSettings[keyName]);

            return defaultValue;
        }

        public static string GetConfigFromSectionOptionalStringValue(string keyName, string section, string defaultValue = "")
        {
            if (string.IsNullOrWhiteSpace(keyName)) return defaultValue;

            var sectionCollection = ConfigurationManager.GetSection(section) as NameValueCollection;

            if (sectionCollection != null)
            {
                if (sectionCollection[keyName] == null) return defaultValue;

                return sectionCollection[keyName];
            }

            return defaultValue;
        }
    }
}