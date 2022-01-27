using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationUnlockTarget
    {
        public static bool Execute(IDevice device)
        {
            var unlockTargetButtonOnScreen = device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, device.Zones.SelectedItem);

            if (!unlockTargetButtonOnScreen.IsFound) return true;

            device.Report("RemovePreviousTarget");
            device.ClickAndReturn(unlockTargetButtonOnScreen.PositionCenterRandom(), "Remove previous target.");

            return true;
        }
    }
}