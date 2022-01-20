using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationMoveLootToHangar
    {
        public static bool Execute(IDevice device, IShip ship)
        {
            device.Mouse.MouseMoveTo(new Point(860, 5));

            Thread.Sleep(1000);

            OperationItemHangarAll.Execute(device, ship);

            Thread.Sleep(1000);

            TrafficDispatcher.MoveCargoToStationHangar(device,
                device.Zones.ActiveShipCargoPoint,
                device.Zones.ItemHangarCargoPoint,
                device.Zones.ActiveShipCargoPointFirst);

            Thread.Sleep(1000);

            Thread.Sleep(1000);

            OperationItemHangarFilterFilaments.Execute(device, ship);

            return true;
        }
    }
}