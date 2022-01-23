using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationLootAll
    {
        public static string Name { get; set; } = "[OperationLootAll]";

        public static bool Execute(IDevice device, IShip ship)
        {
            var lootAllButtonOnScreen = device.FindObjectInScreen(Types.LootAll);

            if (lootAllButtonOnScreen.IsFound == false)
            {
                device.Report("Pattern_LootAll_NotFound");
                return false;
            }

            device.Logger($"[{Name}] LootAll button found. Click on {lootAllButtonOnScreen.PositionCenter}");

            device.Click(lootAllButtonOnScreen.PositionCenterRandom());

            Thread.Sleep(1000);

            device.UnFocusClick();

            return true;
        }
    }
}