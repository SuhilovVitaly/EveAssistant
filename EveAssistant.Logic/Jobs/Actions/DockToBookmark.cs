using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class DockToBookmark : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[DockToBookmark]";

        public DockToBookmark(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 80;

            //IsServerDownTime
            ActionExits.Add((CommonActionExits.IsServerDownTime, ExitFromActionByDowntime));
            ActionExits.Add((CommonActionExits.IsShipInDock, ExitFromActionIfShipInSpace));
        }

        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            OperationDockToBookmark.Execute(Device, Ship, "Home");
        }

        private void ExitFromActionIfShipInSpace()
        {
            OperationMoveLootToHangar.Execute(Device, Ship);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }

        //ExitFromActionByDowntime
        private void ExitFromActionByDowntime()
        {
            OperationMoveLootToHangar.Execute(Device, Ship);

            FinishAction(ExitFromActionReason.Downtime);
        }
    }
}