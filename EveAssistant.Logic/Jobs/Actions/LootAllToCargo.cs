using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class LootAllToCargo : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[LootAllToCargo]";

        public LootAllToCargo(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;
        }

        public void CommandsExecute()
        {
            Thread.Sleep(2000);

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabLoot);

            Thread.Sleep(2000);

            var wreckOnScreen = Device.FindObjectInScreen(Types.OverviewWreck, Device.Zones.Overview);

            if (wreckOnScreen.IsFound == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            TrafficDispatcher.ClickOnPoint(Device.IntPtr, wreckOnScreen.PositionCenterRandom());

            Thread.Sleep(1000);

            Device.UnFocusClick();

            Thread.Sleep(1000);

            var openCargoButtonOnScreen = Device.FindObjectInScreen(Types.PanelSelectedOpenCargo, Device.Zones.SelectedItem);

            if (openCargoButtonOnScreen.IsFound == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            TrafficDispatcher.ClickOnPoint(Device.IntPtr, openCargoButtonOnScreen.PositionCenterRandom());

            Thread.Sleep(1000);

            Device.UnFocusClick();

            Thread.Sleep(1000);

            while (!Device.FindObjectInScreen(Types.LootAll).IsFound)
            {
                Device.Logger("Distance is long. Waiting...");

                Thread.Sleep(1000);
            }

            var lootAllButtonOnScreen = Device.FindObjectInScreen(Types.LootAll);

            if (lootAllButtonOnScreen.IsFound == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            TrafficDispatcher.ClickOnPoint(Device.IntPtr, lootAllButtonOnScreen.PositionCenterRandom());

            Thread.Sleep(1000);

            Device.UnFocusClick();

            FinishAction(ExitFromActionReason.ActionCompleted);
        }

    }
}