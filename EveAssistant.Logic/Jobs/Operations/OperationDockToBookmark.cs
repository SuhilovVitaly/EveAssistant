using System.Diagnostics;
using System.Drawing;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationDockToBookmark
    {
        private const string Pattern = @"Bookmarks/";

        public static bool Execute(IDevice device, IShip ship, string bookmark)
        {
            device.Logger("[OperationWarpToBookmark] Start warp to bookmark.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern + bookmark);

            if (itemOnScreen.IsFound)
            {
                itemOnScreen.PositionCenter = new Point(itemOnScreen.PositionCenter.X + 50, itemOnScreen.PositionCenter.Y);

                TrafficDispatcher.ContextMenuClick(device, itemOnScreen.PositionCenter, Types.ContextMenuDock);
            }
            else
            {
                device.Report("Pattern_bookmark_NotFound");
                device.Logger("[OperationWarpToBookmark] Bookmark not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            Thread.Sleep(200);

            device.Logger("[OperationWarpToBookmark] Finish warp to bookmark. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}