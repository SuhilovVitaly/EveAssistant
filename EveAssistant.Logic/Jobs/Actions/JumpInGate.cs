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
    public class JumpInGate : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[JumpInGate]";

        public JumpInGate(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 360;

            ActionExits.Add((CommonActionExits.IsShipNotMovingToGate, ExitFromAction));
        }
        public void AfterExecute()
        {

        }
        public void CommandsExecute()
        {
            Thread.Sleep(2000);

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabGates);

            Thread.Sleep(2000);

            if (OperationJumpToAbissGate.Execute(Device, Ship) == false)
            {
                Device.Report("Pattern_OverviewAbissGate_NotFound");
                Device.Logger("[OperationEnterToTrace] fail.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Thread.Sleep(1000);

            Device.UnFocusClick();
        }

        private void ExitFromAction()
        {
            Device.Logger($"Exit from {Text} process");

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}