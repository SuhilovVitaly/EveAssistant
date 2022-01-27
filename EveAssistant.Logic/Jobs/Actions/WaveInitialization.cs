using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class WaveInitialization : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[WaveInitialization]";

        public WaveInitialization(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 40;
        }
        public void AfterExecute()
        {

        }
        public void CommandsExecute()
        {
            Device.UnFocusClick();

            OperationUnlockTarget.Execute(Device);

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabMove);

            OperationOrbitObject.Execute(Device, Types.OverviewAbissLootObject);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}