using System;
using System.Collections.Generic;
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
    public class NpcKill : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[NpcKill]";

        public NpcKill(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;

            ActionExits.Add((CommonActionExits.IsTargetLost, ExitFromAction));
        }

        public void CommandsExecute()
        {
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
                ScreenCapture.ScreenShot(Device.IntPtr, "Timeout", Device.Logger);
                Device.Logger($"Pattern '{Types.PanelSelectedItemTarget}' not found.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }


            while (!Device.FindObjectInScreen(Types.PanelSelectedItemUnLockTarget, Device.Zones.SelectedItem).IsFound)
            {
                Device.Logger("Lock target. Waiting...");
                // TODO: 30 sec
                Thread.Sleep(1000);
            }

            Thread.Sleep(2000);

            OperationOpenFire.Execute(Device, Ship);

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            Device.Logger("Exit from NPC kill process");
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}