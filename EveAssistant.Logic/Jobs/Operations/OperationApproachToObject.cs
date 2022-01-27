using System.Diagnostics;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationApproachToObject
    {
        public bool Execute(IDevice device, IShip ship, string pattern)
        {
            device.Logger("[OperationApproachToObject] Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(pattern);

            if (itemOnScreen.IsFound == false)
            {
                device.Report($"Pattern_{pattern.Replace("/", "_")}_NotFound", $"[OperationApproachToObject] Pattern {pattern} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            device.ClickAndReturn(itemOnScreen.PositionCenterRandom(), $"Pattern '{pattern}' is found.");

            var itemApproach = device.FindObjectInScreen(Types.PanelSelectedItemApproach);

            if (itemApproach.IsFound == false)
            {
                device.Report("Approach_NotFound");
                device.Logger($"[OperationApproachToObject] Pattern {Types.PanelSelectedItemApproach} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            device.ClickAndReturn(itemApproach.PositionCenterRandom(), $"Pattern 'Approach' is found. Emulate click.");

            return true;
        }
    }
}