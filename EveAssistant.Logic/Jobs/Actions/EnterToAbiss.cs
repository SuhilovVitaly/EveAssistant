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
    public class EnterToAbiss : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[EnterToAbiss]";

        public EnterToAbiss(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 120;

            ActionExits.Add((CommonActionExits.IsShipInAbiss, ExitFromAction));
        }

        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabGates);

            if (OperationUseTranquilFilament.Execute(Device, Ship) == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                Device.Logger("[OperationUseTranquilFilament] fail.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Thread.Sleep(5000);

            if (OperationEnterToTrace.Execute(Device, Ship) == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                Device.Logger("[OperationEnterToTrace] fail.");

                Device.Mouse.Click(new Point(600, 395));
            }

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}