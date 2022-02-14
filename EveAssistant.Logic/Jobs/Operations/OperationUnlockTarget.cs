using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationUnlockTarget
    {
        public bool Execute(IDevice device, IShip ship)
        {
            var unlockTargetButtonOnScreen = device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, device.Zones.SelectedItem);

            if (!unlockTargetButtonOnScreen.IsFound) return false;

            device.Report("RemovePreviousTarget");
            device.ClickAndReturn(unlockTargetButtonOnScreen.PositionCenterRandom(), "Remove previous target.");

            return true;
        }
    }
}