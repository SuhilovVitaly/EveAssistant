using System;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace EveAssistant.Common.Configuration
{
    public static class ApplicationSettingsManager
    {
        public static ApplicationSettings Load()
        {
            var settingFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Settings.dat");

            return Load(settingFileName);
        }

        public static ApplicationSettings Load(string settingFileName)
        {
            var log = LogManager.GetLogger(typeof(ApplicationSettingsManager));

            var settings = new ApplicationSettings();

            if (File.Exists(settingFileName))
            {
                log.Debug("Settings file " + settingFileName + " is found. Start loading and parsing.");

                try
                {
                    var settingsContent = File.ReadAllText(settingFileName);

                    dynamic jsonResponse = JsonConvert.DeserializeObject(settingsContent);

                    settings.LanguageId = jsonResponse.LanguageId == null ? 0 : jsonResponse.LanguageId;
                    settings.Version = jsonResponse.Version == null ? 0 : jsonResponse.Version;
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Critical error. Exception is: {0}", e);
                }
            }
            else
            {
                log.Debug("Settings file " + settingFileName + " not found. Start load default values.");

                SetDefaultsSettings(settings);
            }

            settings.WriteSettingsToLog();

            return settings;
        }

        public static void Save(ApplicationSettings settings)
        {
            var settingFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Settings.dat");

            Save(settingFileName, settings);
        }

        public static void Save(string settingFileName, ApplicationSettings settings)
        {
            var settingsContent = JsonConvert.SerializeObject(new
            {
                settings.Version,
                settings.LanguageId
            });


            if (File.Exists(settingFileName))
            {
                File.Delete(settingFileName);
            }

            using (var sw = new StreamWriter(settingFileName, true))
            {
                sw.Write(settingsContent);
            }

        }

        private static void SetDefaultsSettings(ApplicationSettings settings)
        {
            settings.Version = "0.1.1";
            settings.LanguageId = 0;
        }
    }
}