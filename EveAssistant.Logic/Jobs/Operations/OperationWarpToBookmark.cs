using System.Diagnostics;
using System.Threading;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Jobs.Operations
{
    public class OperationWarpToBookmark
    {
        private const string Pattern = @"Bookmarks/";

        public static bool Execute(IDevice device, IShip ship, string bookmark)
        {
            device.Logger("[OperationWarpToBookmark] Start warp to bookmark.");

            var workMetric = Stopwatch.StartNew();

            var itemOnScreen = device.FindObjectInScreen(Pattern + bookmark);

            if (itemOnScreen.IsFound)
            {
                TrafficDispatcher.ContextMenuClick(device, itemOnScreen.PositionCenter, Types.ContextMenuWarpToLocation);
            }
            else
            {
                device.Report("Pattern_Bookmark_NotFound");
                device.Logger("[OperationWarpToBookmark] Bookmark not found. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");
                return false;
            }

            Thread.Sleep(200);

            device.Logger("[OperationWarpToBookmark] Finish warp to bookmark. Work time is " + workMetric.Elapsed.TotalSeconds.ToString("N2") + " seconds.");

            return true;
        }
    }
}