using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationEnterToTrace
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

                    Thread.Sleep(5000);

                    //EnterToAbiss
                    var itemEnterToAbiss = device.FindObjectInScreen(@"Filament/EnterToAbiss");

                    if (itemEnterToAbiss.IsFound)
                    {
                        TrafficDispatcher.ClickOnPoint(device.IntPtr, itemEnterToAbiss.PositionCenterRandom());

                        Thread.Sleep(5000);
                    }
                    else
                    {
                        ScreenCapture.ScreenShot(device.IntPtr, "PatternNotFound_EnterToAbiss", device.Logger);
                        return false;
                    }
                }
                else
                {
                    ScreenCapture.ScreenShot(device.IntPtr, "PatternNotFound_ActivateGate", device.Logger);

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