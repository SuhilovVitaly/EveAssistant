using System;
using System.IO;
using log4net;

namespace EveAssistant.Common.Configuration
{
    public class ApplicationSettings
    {
        public ILog Logger = LogManager.GetLogger("ApplicationSettingsLog");

        public string Version { set; get; } = "0.1.0";

        public int LanguageId { get; set; } = 0;

        public string EveOnlineClientTitle => ConfigurationTools.GetConfigOptionalStringValue("EveOnlineTitle", "EVE -");

        public string EveFolder => ConfigurationTools.GetConfigOptionalStringValue("EveFolder", "").Replace(@":\", "_").Replace(@"\", "_");

        public string ReportsFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

        public Shortcuts Shortcuts { get; set; } = new Shortcuts();

        public void WriteSettingsToLog()
        {
            Logger.Debug("--------------------------------------------------------------");
            Logger.Debug("Version:              " + Version);
            Logger.Debug("LanguageId:           " + LanguageId);
            Logger.Debug("--------------------------------------------------------------");
        }
    }
}