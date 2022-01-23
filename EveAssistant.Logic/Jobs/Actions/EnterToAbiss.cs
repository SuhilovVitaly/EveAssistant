﻿using System;
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
        public void AfterExecute()
        {

        }
        public void CommandsExecute()
        {
            Device.UnFocusClick();

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabGates);

            if (OperationUseTranquilFilament.Execute(Device, Ship) == false)
            {
                Device.Report("Pattern_OverviewTabGates_NotFound");
                Device.Logger("[OperationUseTranquilFilament] fail.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Thread.Sleep(5000);

            if (OperationEnterToTrace.Execute(Device, Ship) == false)
            {
                Device.Report("Pattern_AbissGate_NotFound");
                Device.Logger("[OperationEnterToTrace] fail.");

                Device.Mouse.Click(new Point(600, 395));
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Thread.Sleep(200);
        }

        private void ExitFromAction()
        {
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}