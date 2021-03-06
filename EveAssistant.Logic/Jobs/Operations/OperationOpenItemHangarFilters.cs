using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationOpenItemHangarFilters
    {
        private const string Pattern = @"ItemHangar/OpenFilter";

        public bool Execute(IDevice device, IShip ship)
        {
            device.UnFocusClick();

            Thread.Sleep(1000);

            device.Logger($"Start open item hangar filters '{Pattern}'.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern, device.Zones.HangarItemFilters);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(500);

                device.UnFocusClick();
            }
            else
            {
                device.Logger($"'{Pattern}' pattern is not found. Operation abort.");
                return false;
            }
            Thread.Sleep(200);

            device.Logger($"Finish. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}