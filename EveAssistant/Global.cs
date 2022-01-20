using System;
using System.IO;
using EveAssistant.Common;
using EveAssistant.Common.Configuration;
using EveAssistant.Graphic;

namespace EveAssistant
{
    public static class Global
    {
        public static ApplicationSettings ApplicationSettings = ApplicationSettingsManager.Load();

        public static IPatterns PatternFactory = new Patterns(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patterns"));
    }
}
