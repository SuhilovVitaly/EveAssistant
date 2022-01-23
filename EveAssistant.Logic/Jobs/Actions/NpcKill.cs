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
    public class NpcKill : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[NpcKill]";

        private const int ReLockTimeoutInSeconds = 20;

        private bool _isAggressiveMode;

        public NpcKill(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;

            ActionExits.Add((CommonActionExits.IsTargetLost, ExitFromAction));
        }

        public void AfterExecute()
        {
            if (_isAggressiveMode)
            {
                _isAggressiveMode = false;

                Device.Logger("Exit from NPC kill process");
                FinishAction(ExitFromActionReason.ActionCompletedWithAggressiveMode);
                return;
            }

            var unlockTargetButtonOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem);

            if (unlockTargetButtonOnScreen.IsFound)
            {
                Device.Report("UnTarget");
                Device.Logger("UnTarget execute.");
                Device.Click(unlockTargetButtonOnScreen.PositionCenterRandom());
            }
        }

        public void CommandsExecute()
        {
            _isAggressiveMode = false;

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabNpc);

            var itemOnScreen = Device.FindObjectInScreen(Types.OverviewNps, Device.Zones.Overview);

            Device.Logger($"Pattern {Types.OverviewNps}");

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(Device.IntPtr, itemOnScreen.PositionCenterRandom());

                Thread.Sleep(1000);

                Device.UnFocusClick();
            }
            else
            {
                Device.Logger($"Pattern '{Types.OverviewNps}' not found.");
                FinishAction(ExitFromActionReason.AllNpcAreKilled);
                return;
            }

            if (Device.FindObjectInScreen(Types.PanelSelectedItemNpcSkybreaker, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Enter to aggressive mode.");

                Device.Report("SkybreakerFound");

                _isAggressiveMode = true;

                var orbitButtonOnScreen = Device.FindObjectInScreen(Types.PanelSelectedOrbit, Device.Zones.SelectedItem);

                Device.Click(orbitButtonOnScreen.PositionCenterRandom());
            }

            while (Device.FindObjectInScreen(Types.PanelSelectedItemTargetDisabled, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Distance is long. Waiting...");

                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);

            var selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

            Device.Logger($"Pattern {Types.PanelSelectedItemTarget}");

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ClickOnPoint(Device.IntPtr, selectedItemOnScreen.PositionCenterRandom());

                Thread.Sleep(1000);

                Device.UnFocusClick();
            }
            else
            {
                Device.Report("Timeout");
                Device.Logger($"Pattern '{Types.PanelSelectedItemTarget}' not found.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            var lockTimeout = 0;

            while (!Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Lock target. Waiting...");
                
                Thread.Sleep(1000);

                selectedItemOnScreen = Device.FindObjectInScreen(Types.PanelSelectedItemTarget, Device.Zones.SelectedItem);

                if (selectedItemOnScreen.IsFound)
                {
                    Device.Logger("Re-Lock target.");

                    TrafficDispatcher.ClickOnPoint(Device.IntPtr, selectedItemOnScreen.PositionCenterRandom());

                    Thread.Sleep(1000);

                    Device.UnFocusClick();
                }

                lockTimeout++;

                if (lockTimeout > ReLockTimeoutInSeconds)
                {
                    Device.Logger("Exit from action by timeout on re-lock target.");
                    Device.Report("ReLockTargetTimeout");
                    FinishAction(ExitFromActionReason.Timeout);
                    return;
                }
            }

            Thread.Sleep(2000);

            OperationOpenFire.Execute(Device, Ship);

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            AfterExecute();
            Device.Logger("Exit from NPC kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}