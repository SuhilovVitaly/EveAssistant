using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class EnableAllActiveModules : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[EnableAllActiveModules]";

        public EnableAllActiveModules(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 50;
        }

        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            OperationsManager.Execute(OperationTypes.EnableActiveModules, Device, Ship);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}