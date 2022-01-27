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

        public void CommandsExecute()
        {
            Device.Mouse.ClickCentreScreen();

            Logger($"Ship is {Ship.Name} ready.");

            OperationsManager.Execute(OperationTypes.ClearBackground, Device, Ship);

            OperationsManager.Execute(OperationTypes.OpenShipCargo, Device, Ship);

            OperationsManager.Execute(OperationTypes.MoveFilamentToCargo, Device, Ship);

            OperationsManager.Execute(OperationTypes.OpenItemHangarFilters, Device, Ship);

            OperationsManager.Execute(OperationTypes.ItemHangarFilterFilaments, Device, Ship);

            OperationsManager.Execute(OperationTypes.FormFleet, Device, Ship);

            FinishAction(ExitFromActionReason.ActionCompleted);
        }
    }
}