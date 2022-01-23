using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationExitStation
    {
        private const string Pattern = Types.StationExit;

        public static bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger("Start station exit.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());
            }
            else
            {
                device.Report("Pattern_StationExit_NotFound");
                device.Logger("StationExit pattern is not found. Operation abort.");
                return false;
            }
            Thread.Sleep(200);

            device.Logger("Finish clear background. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}