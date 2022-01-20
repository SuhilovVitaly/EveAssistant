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

        public void CommandsExecute()
        {
            Thread.Sleep(2000);

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabGates);

            Thread.Sleep(2000);

            if (OperationJumpToAbissGate.Execute(Device, Ship) == false)
            {
                ScreenCapture.ScreenShot(Device.IntPtr, "PatternNotFound", Device.Logger);
                Device.Logger("[OperationEnterToTrace] fail.");
                FinishAction(ExitFromActionReason.Timeout);
                return;
            }

            Thread.Sleep(1000);

            TrafficDispatcher.ClickOnPoint(Device.IntPtr, new Point(860, 5));
        }

        private void ExitFromAction()
        {
            Device.Logger($"Exit from {Text} process");

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}