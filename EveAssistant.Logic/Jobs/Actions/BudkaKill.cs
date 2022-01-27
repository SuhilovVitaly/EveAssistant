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
        public void AfterExecute()
        {

        }
        public void CommandsExecute()
        {
            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabMove);

            Thread.Sleep(2000);

            var itemOnScreen = Device.FindObjectInScreen(Types.OverviewAbissLootObject, Device.Zones.Overview);

            Device.Logger($"Pattern {Types.OverviewAbissLootObject}");

            if (itemOnScreen.IsFound)
            {
                Device.ClickAndReturn(itemOnScreen.PositionCenterRandom());
            }
            else
            {
                Device.Report("LootNotFound");
                Device.Logger($"Pattern '{Types.OverviewAbissLootObject}' not found.");
                FinishAction(ExitFromActionReason.LootNotFound);
                return;
            }

            var selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

            if (itemOnScreen.IsFound)
            {
                Device.ClickAndReturn(selectedItemOnScreen.PositionCenterRandom());
            }
            else
            {
                Device.Report("PatternItemTargetNotFound");
                Device.Logger($"Pattern '{Types.PanelSelectedItemTarget}' not found.");
                FinishAction(ExitFromActionReason.PatternNotFound);
                return;
            }

            Thread.Sleep(3000);

            OperationOpenFire.Execute(Device, Ship);
        }

        private void ExitFromActionKillBudka()
        {
            Device.Logger("Exit from BUDKA kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}