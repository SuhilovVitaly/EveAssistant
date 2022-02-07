using System;

namespace EveAssistant.Logic.Tools
{
    public class Dates
    {
        public static bool IsDownTime(DateTime datetime)
        {
            var start = new TimeSpan(10, 40, 0);
            var end = new TimeSpan(11, 10, 0);
            //, TimeSpan start, TimeSpan end
            // convert datetime to a TimeSpan
            var now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        public static bool IsBeforeDownTime(DateTime datetime)
        {
            var downTime = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                11, 00, 0);

            return downTime > datetime;

        }
    }
}