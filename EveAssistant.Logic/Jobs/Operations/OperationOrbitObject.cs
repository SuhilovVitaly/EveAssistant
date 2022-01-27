using System.Diagnostics;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationOrbitObject
    {
        private const string Name = "[OperationOrbitObject]";
        private const string OrbitButtonPattern = Types.PanelSelectedOrbit;

        public  bool Execute(IDevice device, IShip ship, string pattern)
        {
            device.Logger($"{Name} Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(pattern);

            if (itemOnScreen.IsFound == false)
            {
                device.Report($"Pattern_{pattern.Replace("/", "_")}_NotFound", $"{Name}  Pattern {pattern} not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            device.ClickAndReturn(itemOnScreen.PositionCenterRandom(), $"{Name}  '{pattern}' is found.");

            var itemApproach = device.FindObjectInScreen(OrbitButtonPattern);

            if (itemApproach.IsFound == false)
            {
                device.Report("Orbit_NotFound");
                device.Logger($"{Name}  Pattern {OrbitButtonPattern} not found. Work time is {workMetric.Elapsed.TotalSeconds:N2} seconds.");
                return false;
            }

            device.ClickAndReturn(itemApproach.PositionCenterRandom(), $"{Name} Finish. Pattern 'Orbit' is found. Emulate click. Work time is {workMetric.Elapsed.TotalSeconds:N2} seconds.");

            return true;
        }
    }
}