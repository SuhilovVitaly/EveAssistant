using System;
using System.Collections.Generic;
using System.IO;

namespace EveAssistant.Configuration
{
    public class SettingsOverrider
    {
        public static void Execute(Action<string> logger)
        {
            var settingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace(@"\Roaming", @"\Local\CCP\EVE\XXXTTXXX_sharedcache_tq_tranquility\settings_English_Mining\");

            settingsFolder = settingsFolder.Replace("XXXTTXXX", Global.ApplicationSettings.EveFolder);


            logger($"settingsFolder = {settingsFolder}");

            var removeChars = new List<string>();
            var removeUsers = new List<string>();

            var configurationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration");

            logger($"configurationFolder = {configurationFolder}");

            foreach (var file in Directory.GetFiles(settingsFolder))
            {
                if (file.Contains(@"\core_char_"))
                {
                    removeChars.Add(file);
                }

                if (file.Contains(@"\core_user_"))
                {
                    removeUsers.Add(file);
                }
            }

            foreach (var removeChar in removeChars)
            {
                var fileName = configurationFolder + @"\core_char_xxxxx.dat";
                File.Delete(removeChar);
                File.Copy(fileName, removeChar);

            }

            foreach (var removeUser in removeUsers)
            {
                var fileName = configurationFolder + @"\core_user_xxxxx.dat";
                logger($"removeUser Delete = {removeUser} Copy {fileName}");
                File.Delete(removeUser);
                File.Copy(fileName, removeUser);
            }

            var fileNameTo = settingsFolder + @"\core_public__.yaml";
            var fileNameFrom = configurationFolder + @"\core_public__.yaml";
            logger($"fileNameTo Delete = {fileNameTo} Copy {fileNameFrom}");
            File.Delete(fileNameTo);
            File.Copy(fileNameFrom, fileNameTo);

            fileNameTo = settingsFolder + @"\core_char__.dat";
            fileNameFrom = configurationFolder + @"\core_char__.dat";
            logger($"fileNameTo Delete = {fileNameTo} Copy {fileNameFrom}");
            File.Delete(fileNameTo);
            File.Copy(fileNameFrom, fileNameTo);

            //fileNameTo = settingsFolder + @"\prefs.ini";
            //fileNameFrom = configurationFolder + @"\prefs.ini";
            //logger($"fileNameTo Delete = {fileNameTo} Copy {fileNameFrom}");
            //File.Delete(fileNameTo);
            //File.Copy(fileNameFrom, fileNameTo);


        }
    }
}