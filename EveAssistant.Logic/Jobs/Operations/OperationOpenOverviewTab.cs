using System.Diagnostics;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationOpenOverviewTab
    {
        public static bool Execute(IDevice device, IShip ship, string pattern)
        {
            device.UnFocusClick();

            Thread.Sleep(1000);

            device.Logger($"Start open overview tab '{pattern}'.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(pattern);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(500);

                device.UnFocusClick();
            }
            else
            {
                device.Logger($"'{pattern}' pattern is not found. Operation abort.");
                return false;
            }
            Thread.Sleep(200);

            device.Logger($"Finish open overview tab '{pattern}'. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}