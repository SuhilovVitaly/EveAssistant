using System.Diagnostics;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationItemHangarFilterFilaments
    {
        private const string Pattern = @"ItemHangar/FilterFlmDesabled";
        private const string PatternFilterAll = @"ItemHangar/FilterAllEnabled";

        public static bool Execute(IDevice device, IShip ship)
        {
            TrafficDispatcher.ClickOnPoint(device.IntPtr, new Point(860, 5));

            Thread.Sleep(1000);

            device.Logger($"Start open item hangar filters '{Pattern}'.");

            var workMetric = Stopwatch.StartNew();

            var filterAllOnScreen = device.FindObjectInScreen(PatternFilterAll, device.Zones.HangarItemFilters);

            if (filterAllOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, filterAllOnScreen.PositionCenterRandom());

                Thread.Sleep(500);

                TrafficDispatcher.ClickOnPoint(device.IntPtr, new Point(860, 5));
            }

            var filterFilamentsOnScreen = device.FindObjectInScreen(Pattern, device.Zones.HangarItemFilters);

            if (filterFilamentsOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, filterFilamentsOnScreen.PositionCenterRandom());

                Thread.Sleep(500);

                TrafficDispatcher.ClickOnPoint(device.IntPtr, new Point(860, 5));
            }

            Thread.Sleep(200);

            device.Logger($"Finish. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}