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

        public void CommandsExecute()
        {
            Thread.Sleep(4000);

            Device.UnFocusClick();

            OperationsManager.Execute(OperationTypes.UnlockTarget, Device, Ship);

            OperationsManager.Execute(OperationTypes.OpenOverviewTab, Device, Ship, Types.OverviewTabMove);

            OperationsManager.Execute(OperationTypes.OrbitObject, Device, Ship, Types.OverviewAbissLootObject);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}