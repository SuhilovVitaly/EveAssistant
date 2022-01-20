using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationSelectNpc
    {
        private const string Pattern = Types.OverviewNps;
        private const string Name = "[OperationSelectNpc]";

        public static bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger($"{Name} Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern, device.Zones.Overview);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());
            }
            else
            {
                device.Logger($"{Name} pattern is not found. Operation abort.");
                return false;
            }

            Thread.Sleep(200);

            device.Logger($"{Name} Finish. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}