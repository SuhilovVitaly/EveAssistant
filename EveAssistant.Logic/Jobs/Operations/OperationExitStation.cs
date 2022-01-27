using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationExitStation
    {
        public static string Name { get; set; } = "[OperationExitStation]";
        private const string Pattern = Types.StationExit;

        public static bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger("Start station exit.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound)
            {
                device.Click(itemOnScreen.PositionCenterRandom());
            }
            else
            {
                device.Report("Pattern_StationExit_NotFound", "StationExit pattern is not found. Operation abort.");
                return false;
            }

            device.Logger($"[{Name}] LootAll button found. Click on {itemOnScreen.PositionCenter}");

            Thread.Sleep(200);

            device.Logger($"[{Name}] Finish. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}