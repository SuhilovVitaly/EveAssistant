using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationApproachToObject
    {
        public static bool Execute(IDevice device, IShip ship, string pattern)
        {
            device.Logger("[OperationApproachToObject] Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(pattern);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(1000);

                var itemApproach = device.FindObjectInScreen(Types.PanelSelectedItemApproach);

                if (itemApproach.IsFound)
                {
                    TrafficDispatcher.ClickOnPoint(device.IntPtr, itemApproach.PositionCenterRandom());
                }
                else
                {
                    device.Report("Pattern_PanelSelectedItemApproach_NotFound");
                    device.Logger($"[OperationApproachToObject] Pattern {Types.PanelSelectedItemApproach} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                    return false;
                }
            }
            else
            {
                device.Report($"Pattern_{pattern.Replace("/","_")}_NotFound");
                device.Logger($"[OperationApproachToObject] Pattern {pattern} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            return true;
        }
    }
}