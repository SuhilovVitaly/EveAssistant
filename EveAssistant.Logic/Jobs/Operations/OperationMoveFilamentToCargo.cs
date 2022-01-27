using System.Diagnostics;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Graphic;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationMoveFilamentToCargo
    {
        public bool Execute(IDevice device, IShip ship)
        {
            Thread.Sleep(1000);

            var workMetric = Stopwatch.StartNew();

            device.Logger("Start move filaments to cargo.");

            var filament = ImageDetect.SearchImage(device.PatternFactory.GetPatterns(@"Filament/Electrical"), device, new Rectangle(794, 694, 1191 - 794, 994 - 694));

            if (filament.IsFound)
            {
                TrafficDispatcher.FastDrugAndDrop(device,
                    new Point(filament.PositionCenter.X,filament.PositionCenter.Y),
                    new Point(150, 720), device.Logger);
            }

            device.UnFocusClick();

            device.Logger("Finish move filament to cargo. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}