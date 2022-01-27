using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Jobs.Status;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationFormFleet
    {
        private const string Name = "[OperationFormFleet]";

        public bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger($"{Name} Operation start.");

            if (AllStates.IsFleetCommander(device).IsFound) return true;

            var fleetWindowOnScreen = AllStates.IsFleetWindowOpened(device);

            if (fleetWindowOnScreen.IsFound == false)
            {
                device.Keyboard.PressKey(device.Shortcuts.FormFleet);
            }

            Thread.Sleep(1000);

            device.ContextMenuClick(Types.FleetWindow, Types.ContextMenuFormFleet);

            Thread.Sleep(3000);

            device.Logger($"{Name} Operation end.");

            return AllStates.IsFleetCommander(device).IsFound;
        }
    }
}