using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationEnableActiveModules
    {
        public bool Execute(IDevice device, IShip ship)
        {
            device.Logger("Start enable active modules.");

            var workMetric = Stopwatch.StartNew();

            Thread.Sleep(200);

            foreach (var shipActiveModule in ship.ActiveModules)
            {
                device.Keyboard.PressKey(shipActiveModule);
                device.Logger("Module " + shipActiveModule + " is enabled.");
                Thread.Sleep(200);
            }

            device.Logger("All modules enabled. Work time is " + workMetric.Elapsed.Seconds + " seconds.");

            return true;
        }
    }
}