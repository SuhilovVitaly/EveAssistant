using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class WarpToBookmark : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[WarpToBookmark]";

        private string _bookmark;

        public WarpToBookmark(IDevice device, IShip ship, string bookmark) : base(device, ship)
        {
            _bookmark = bookmark;
            TimeoutInSeconds = 60;

            ActionExits.Add((CommonActionExits.IsShipStopped, ExitFromActionIfShipInSpace));
        }

        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            OperationWarpToBookmark.Execute(Device, Ship, _bookmark);
        }

        private void ExitFromActionIfShipInSpace()
        {
            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}