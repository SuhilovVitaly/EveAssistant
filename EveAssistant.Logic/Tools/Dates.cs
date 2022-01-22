using System;

namespace EveAssistant.Logic.Tools
{
    public class Dates
    {
        public static bool IsDownTime(DateTime datetime)
        {
            TimeSpan start = new TimeSpan(11, 40, 0);
            TimeSpan end = new TimeSpan(12, 10, 0);
            //, TimeSpan start, TimeSpan end
            // convert datetime to a TimeSpan
            var now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }
    }
}