using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationOpenShipCargo
    {
        private const string PatternActiveShipCargo = @"Panel/ActiveShipCargo";

        public static void Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger("Start open ship cargo.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(PatternActiveShipCargo);

            if (itemOnScreen.IsFound)
            {
                device.Logger("No need open ship cargo. Already opened.");
            }
            else
            {
                device.Mouse.ClickCentreScreen();

                device.Keyboard.PressKey(device.Shortcuts.OpenCargo);
            }

            Thread.Sleep(100);

            device.Logger("Finish open ship cargo. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
        }
    }
}