using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationSelectWreck
    {
        public static string Name { get; set; } = "[OperationSelectWreck]";

        public static bool Execute(IDevice device, IShip ship)
        {
            var wreckOnScreen = device.FindObjectInScreen(Types.OverviewWreck, device.Zones.Overview);

            if (wreckOnScreen.IsFound == false)
            {
                device.Report("Pattern_OverviewWreck_NotFound", $"[{Name}]Wreck not found in overview.");
                return false;
            }

            device.Logger($"[{Name}] Wreck found in overview. Click on {wreckOnScreen.PositionCenter}");

            device.Click(wreckOnScreen.PositionCenterRandom());

            Thread.Sleep(200);

            device.UnFocusClick();

            Thread.Sleep(1000);

            return true;
        }
    }
}