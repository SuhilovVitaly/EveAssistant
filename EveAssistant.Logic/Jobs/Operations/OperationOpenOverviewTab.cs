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

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(pattern);

            if (itemOnScreen.IsFound == false)
            {
                device.Logger($"'{pattern}' pattern is not found. Operation abort.");
                return false;
            }

            device.ClickAndReturn(itemOnScreen.PositionCenterRandom(), $"Pattern '{pattern}' is found. Emulate click. Work time is {workMetric.Elapsed.TotalSeconds:N2}");

            return true;
        }
    }
}