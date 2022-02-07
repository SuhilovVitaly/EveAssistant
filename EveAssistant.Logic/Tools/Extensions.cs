using System;

namespace EveAssistant.Logic.Tools
{
    public static class Extensions
    {
        public static string ToReadableString(this TimeSpan span)
        {
            if (span.TotalDays > 1000)
            {
                return "00:00:00";
            }

            return span.ToString("hh\\:mm\\:ss");
        }
    }
}