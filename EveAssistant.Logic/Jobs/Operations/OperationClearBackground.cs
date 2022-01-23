using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Logic.Ships;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationClearBackground
    {
        private const string Pattern = @"ShipExteriorDisabled";

        public static bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            device.Logger("Start clear background.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern);

            if (itemOnScreen.IsFound)
            {
                device.Logger("No need clear background. Already enabled.");
            }
            else
            {
                device.UnFocusClick();

                device.Keyboard.PressKey(device.Shortcuts.ClearScreen);
            }

            Thread.Sleep(200);

            device.Logger("Finish clear background. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}