using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationUseTranquilFilament
    {
        private const string Pattern = @"Filament/Electrical";

        public static bool Execute(IDevice device, IShip ship)
        {
            device.Logger("[OperationUseTranquilFilament] Start.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ContextMenuClick(device, itemOnScreen.PositionCenter, Types.ContextMenuUseTranquilFilament);

                Thread.Sleep(5000);

                var filamentActivateForFleet = device.FindObjectInScreen(Types.FilamentActivateForFleet);

                if (filamentActivateForFleet.IsFound)
                {
                    TrafficDispatcher.ClickOnPoint(device.IntPtr, filamentActivateForFleet.PositionCenterRandom());

                    Thread.Sleep(500);

                    device.Logger("[OperationUseTranquilFilament] Succeeded.");
                }
                else
                {
                    device.Report("Pattern_FilamentActivateForFleet_NotFound");
                    device.UnFocusClick();
                    return false;
                }
            }
            else
            {
                device.Report($"Pattern_{Pattern.Replace("/", "_")}_NotFound");

                device.Logger("[OperationUseTranquilFilament] Pattern not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            Thread.Sleep(200);

            device.Logger("[OperationUseTranquilFilament] Finish. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}