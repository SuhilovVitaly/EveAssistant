using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class NpcKill : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[NpcKill]";

        private const int ReLockTimeoutInSeconds = 20;

        private bool isUseAggressiveMode;

        private bool _isAggressiveMode;

        private const int TimeoutAfterKillNpcShipInMs = 1000;

        public NpcKill(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;

            isUseAggressiveMode = true;

            ActionExits.Add((CommonActionExits.IsTargetLost, ExitFromAction));
        }



        public void CommandsExecute()
        {
            _isAggressiveMode = false;

            if (Device.FindObjectInScreen(Types.ShipInDock).IsFound)
            {
                Device.Report("ShipDestroyed", "Ship Destroyed");
                FinishAction(ExitFromActionReason.ShipDestroyed);
                return;
            }

            Thread.Sleep(500);

            OperationsManager.Execute(OperationTypes.OpenOverviewTab, Device, Ship, Types.OverviewTabNpc);

            var itemOnScreen = Device.FindObjectInScreen(Types.OverviewNps, Device.Zones.Overview);

            Device.Logger($"Pattern {Types.OverviewNps}");

            if (itemOnScreen.IsFound == false)
            {
                Device.Logger($"Pattern '{Types.OverviewNps}' not found.");
                FinishAction(ExitFromActionReason.AllNpcAreKilled);
                return;
            }

            Device.ClickAndReturn(itemOnScreen.PositionCenterRandom(), $"NPC found on coordinates {itemOnScreen.PositionCenter}");

            if (isUseAggressiveMode)
            {
                if (Device.FindObjectInScreen(Types.PanelSelectedItemNpcSkybreaker, Device.Zones.SelectedItem).IsFound)
                {
                    Device.Report("SkybreakerFound", "Enter to aggressive mode.");

                    _isAggressiveMode = true;

                    var orbitButtonOnScreen =
                        Device.FindObjectInScreen(Types.PanelSelectedOrbit, Device.Zones.SelectedItem);

                    Device.Logger($"Click on '{orbitButtonOnScreen.PositionCenterRandom()}'");

                    Device.Click(orbitButtonOnScreen.PositionCenterRandom());

                    Thread.Sleep(1000);

                    Device.Logger($"Press '{Device.Shortcuts.FormFleet}'");

                    Device.Keyboard.PressKey(Device.Shortcuts.FormFleet);

                    Thread.Sleep(1000);
                }
            }

            while (Device.FindObjectInScreen(Types.PanelSelectedItemTargetDisabled, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Distance is long. Waiting...");

                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);

            var selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

            Device.Logger($"Pattern {Types.PanelSelectedItemTarget}");

            if (itemOnScreen.IsFound == false)
            {
                Device.Report("Timeout", $"Pattern '{Types.PanelSelectedItemTarget}' not found.");

                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Device.ClickAndReturn(selectedItemOnScreen.PositionCenterRandom(), $"Button 'Target' found on coordinates {selectedItemOnScreen.PositionCenter}");

            var lockTimeout = 0;

            while (!Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Lock target. Waiting...");
                
                Thread.Sleep(1000);

                selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

                if (selectedItemOnScreen.IsFound)
                {
                    Device.ClickAndReturn(selectedItemOnScreen.PositionCenterRandom(), "Re-Lock target.");
                }

                lockTimeout++;

                if (lockTimeout > ReLockTimeoutInSeconds)
                {
                    Device.Report("ReLockTargetTimeout", "Exit from action by timeout on re-lock target.");
                    FinishAction(ExitFromActionReason.Timeout);
                    return;
                }
            }

            Thread.Sleep(1000);

            if (Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem).IsFound == false)
            {
                Device.Report("ItemUnLockTargetNotFound", $"Pattern '{Types.PanelSelectedItemUnLockTarget}' not found.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            OperationsManager.Execute(OperationTypes.OpenFire, Device, Ship);

            Device.Logger("Start kill selected NPC.");

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            Thread.Sleep(TimeoutAfterKillNpcShipInMs);

            if (isUseAggressiveMode && _isAggressiveMode)
            {
                _isAggressiveMode = false;

                Device.Logger("Exit from NPC kill process. Remove aggressive mode.");
                FinishAction(ExitFromActionReason.ActionCompletedWithAggressiveMode);
                return;
            }

            var unlockTargetButtonOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem);

            if (unlockTargetButtonOnScreen.IsFound)
            {
                Device.Report("UnTarget", "UnTarget execute.");
                Device.Click(unlockTargetButtonOnScreen.PositionCenterRandom());
            }

            Device.Logger("Exit from NPC kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }

    }
}