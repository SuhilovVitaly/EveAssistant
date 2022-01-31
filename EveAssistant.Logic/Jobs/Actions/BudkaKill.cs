using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

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
            OperationsManager.Execute(OperationTypes.OpenOverviewTab, Device, Ship, Types.OverviewTabMove);

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

            var orbitButtonOnScreen = Device.FindObjectInScreen(Types.PanelSelectedOrbit, Device.Zones.SelectedItem);

            Device.Logger($"Click on '{orbitButtonOnScreen.PositionCenterRandom()}'");

            Device.Click(orbitButtonOnScreen.PositionCenterRandom());

            Thread.Sleep(1000);

            Device.Logger($"Press '{Device.Shortcuts.FormFleet}'");

            Device.Keyboard.PressKey(Device.Shortcuts.FormFleet);

            Thread.Sleep(1000);

            while (Device.FindObjectInScreen(Types.PanelSelectedItemTargetDisabled, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Distance is long. Waiting...");

                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);

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

            OperationsManager.Execute(OperationTypes.OpenFire, Device, Ship);
        }

        private void ExitFromActionKillBudka()
        {
            Device.Keyboard.PressKey(Device.Shortcuts.ShipStop);

            Device.Logger("Exit from BUDKA kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}