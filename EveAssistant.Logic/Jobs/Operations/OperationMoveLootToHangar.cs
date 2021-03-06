using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationMoveLootToHangar
    {
        public bool Execute(IDevice device, IShip ship)
        {
            device.UnFocusClick();

            Thread.Sleep(1000);

            OperationsManager.Execute(OperationTypes.ItemHangarAll, device, ship);

            Thread.Sleep(1000);

            TrafficDispatcher.MoveCargoToStationHangar(device,
                device.Zones.ActiveShipCargoPoint,
                device.Zones.ItemHangarCargoPoint,
                device.Zones.ActiveShipCargoPointFirst);

            Thread.Sleep(1000);

            OperationsManager.Execute(OperationTypes.ItemHangarFilterFilaments, device, ship);

            return true;
        }
    }
}