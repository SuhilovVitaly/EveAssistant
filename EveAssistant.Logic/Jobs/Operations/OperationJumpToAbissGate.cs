using System.Diagnostics;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationJumpToAbissGate
    {
        private const string Pattern = @"Panel/Overview/AbyssalTrace";

        public static bool Execute(IDevice device, IShip ship)
        {
            device.Logger("[OperationEnterToTrace] Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(200);

                var itemActivateGate = device.FindObjectInScreen(@"Panel/SelectedItem/ActivateGate");

                if (itemActivateGate.IsFound)
                {
                    TrafficDispatcher.ClickOnPoint(device.IntPtr, itemActivateGate.PositionCenterRandom());

                    Thread.Sleep(1000);

                    TrafficDispatcher.ClickOnPoint(device.IntPtr, new Point(860, 5));

                    Thread.Sleep(4000);
                }
                else
                {
                    device.Logger("[OperationEnterToTrace] 'Panel/SelectedItem/ActivateGate' not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

                    return false;
                }
            }
            else
            {
                device.Logger($"[OperationEnterToTrace] {Pattern} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            Thread.Sleep(200);

            device.Logger("[OperationEnterToTrace] Finish warp to bookmark. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}