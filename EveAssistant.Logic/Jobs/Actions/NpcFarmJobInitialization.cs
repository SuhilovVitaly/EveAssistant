using EveAssistant.Common.Device;
using EveAssistant.Logic.Job.Action;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Actions
{
    public class NpcFarmJobInitialization : GenericAction, IBasicAction
    {
        public string Text { get; set; } = "[JobInitialization]";

        public NpcFarmJobInitialization(IDevice device, IShip ship) : base(device, ship)
        {
            TimeoutInSeconds = 50;
        }
        public void AfterExecute()
        {
            
        }
        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            Logger($"Ship is {Ship.Name} ready.");

            OperationClearBackground.Execute(Device, Ship);

            OperationOpenShipCargo.Execute(Device, Ship);

            OperationMoveFilamentToCargo.Execute(Device, Ship);

            OperationOpenItemHangarFilters.Execute(Device, Ship);

            OperationItemHangarFilterFilaments.Execute(Device, Ship);

            OperationFormFleet.Execute(Device, Ship);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}