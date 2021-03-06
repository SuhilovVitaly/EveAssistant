using System;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class StationExit : GenericAction, IBasicAction
    {
        private const int TimeoutAfterStationExitMs = 5000;

        public string Text { get; set; } = "[StationExit]";

        public StationExit(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 20;

            ActionExits.Add((CommonActionExits.IsShipInSpace, ExitFromActionIfShipInSpace));
        }

        public void CommandsExecute()
        {
            Device.Metrics.StartCycleTime = DateTime.Now;

            Device.Mouse.ClickCentreScreen();

            OperationsManager.Execute(OperationTypes.ExitStation, Device, Ship);

            Device.Mouse.ClickCentreScreen();
        }

        private void ExitFromActionIfShipInSpace()
        {
            Thread.Sleep(TimeoutAfterStationExitMs);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}