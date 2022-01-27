using System.Diagnostics;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationJumpToAbissGate
    {
        private const string Pattern = @"Panel/Overview/AbyssalTrace";

        public bool Execute(IDevice device, IShip ship)
        {
            device.Logger("[OperationEnterToTrace] Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound == false)
            {
                device.Report($"Pattern_{Pattern.Replace("/", "_")}_NotFound", 
                    $"[OperationEnterToTrace] {Pattern} not found. Work time is {workMetric.Elapsed.TotalSeconds:N2} seconds.");
                return false;
            }

            device.ClickAndReturn(itemOnScreen.PositionCenterRandom());

            var itemActivateGate = device.FindObjectInScreen(@"Panel/SelectedItem/ActivateGate");

            if (itemActivateGate.IsFound == false)
            {
                device.Report("Pattern_ActivateGate_NotFound", 
                    $"[OperationEnterToTrace] 'Panel/SelectedItem/ActivateGate' not found. Work time is {workMetric.Elapsed.TotalSeconds:N2} seconds.");
                return false;
            }

            device.ClickAndReturn(itemActivateGate.PositionCenterRandom(), 
                $"[OperationEnterToTrace] Finish jump. Work time is {workMetric.Elapsed.TotalSeconds:N2} seconds.");

            return true;
        }
    }
}