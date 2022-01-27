using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationLootAll
    {
        public static string Name { get; set; } = "[OperationLootAll]";

        public static bool Execute(IDevice device)
        {
            var lootAllButtonOnScreen = device.FindObjectInScreen(Types.LootAll);

            if (lootAllButtonOnScreen.IsFound == false)
            {
                device.Report("Pattern_LootAll_NotFound");
                return false;
            }

            device.Logger($"[{Name}] LootAll button found. Click on {lootAllButtonOnScreen.PositionCenter}");

            device.ClickAndReturn(lootAllButtonOnScreen.PositionCenterRandom());

            return true;
        }
    }
}