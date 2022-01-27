using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Job.Action.Exit;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class JumpInGate : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[JumpInGate]";

        public JumpInGate(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 420;

            ActionExits.Add((CommonActionExits.IsCantActivateGate, ExitFromActionCantActivateGate));
            ActionExits.Add((CommonActionExits.IsShipNotMovingToGate, ExitFromAction));
        }

        public void CommandsExecute()
        {
            Thread.Sleep(2000);

            OperationsManager.Execute(OperationTypes.OpenOverviewTab, Device, Ship, Types.OverviewTabGates);

            Thread.Sleep(2000);

            if (OperationsManager.Execute(OperationTypes.JumpToAbissGate, Device, Ship) == false)
            {
                Device.Report("Pattern_OverviewAbissGate_NotFound", "[OperationEnterToTrace] fail.");
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

        private void ExitFromActionCantActivateGate()
        {
            Device.Logger($"Exit from {Text} process on 'CantActivateGate'");

            var itemOnScreen = Device.FindObjectInScreen(Types.WindowExitCantActivateGate);

            if (itemOnScreen.IsFound)
            {
                Device.Logger($"[{Text}] Exit button found on screen. Click on {itemOnScreen.PositionCenter}");

                Device.Click(itemOnScreen.PositionCenterRandom());

                Thread.Sleep(2000);
            }

            FinishAction(ExitFromActionReason.CantActivateGate);
        }
    }
}