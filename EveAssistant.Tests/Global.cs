using System;
using System.IO;

namespace EveAssistant.Tests
{
    public class Global
    {
        public static string PatternsClientPath { get; set; } = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\EveAssistant\\Patterns");

        public static string PatternsTestsPath { get; set; } = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\EveAssistant.Tests\\Patterns");
    }
}