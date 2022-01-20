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
            Device.Mouse.ClickCentreScreen();

            OperationOpenOverviewTab.Execute(Device, Ship, Types.OverviewTabMove);

            Thread.Sleep(1000);

            OperationApproachToObject.Execute(Device, Ship, Types.OverviewAbissLootObject);

            Thread.Sleep(1000);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}