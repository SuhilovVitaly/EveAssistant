using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class BudkaKill : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[BudkaKill]";

        public BudkaKill(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;

            ActionExits.Add((CommonActionExits.IsTargetLost, ExitFromActionKillBudka));
        }

        public void CommandsExecute()
        {
            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabMove);

            var itemOnScreen = Device.FindObjectInScreen(Types.OverviewAbissLootObject, Device.Zones.Overview);

            Device.Logger($"Pattern {Types.OverviewAbissLootObject}");

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(Device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(1000);

                TrafficDispatcher.ClickOnPoint(Device.IntPtr, new Point(860, 5));
            }
            else
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "LootNotFound", Device.Logger);
                Device.Logger($"Pattern '{Types.OverviewAbissLootObject}' not found.");
                FinishAction(ExitFromActionReason.LootNotFound);
                return;
            }

            var selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(Device.IntPtr, selectedItemOnScreen.PositionCenterRandom());

                Thread.Sleep(1000);

                TrafficDispatcher.ClickOnPoint(Device.IntPtr, new Point(860, 5));
            }
            else
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                Device.Logger($"Pattern '{Types.PanelSelectedItemTarget}' not found.");
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            Thread.Sleep(10000);

            OperationOpenFire.Execute(Device, Ship);
        }

        private void ExitFromActionKillBudka()
        {
            Device.Logger("Exit from BUDKA kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}