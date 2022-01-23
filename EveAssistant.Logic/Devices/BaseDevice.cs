using System;
using System.Drawing;
using System.Drawing.Imaging;
using EveAssistant.Common;
using EveAssistant.Common.Configuration;
using EveAssistant.Common.Device;
using EveAssistant.Common.Device.Events;
using EveAssistant.Common.UserInterface;
using EveAssistant.Graphic;
using EveAssistant.Logic.Tools;

namespace EveAssistant.Logic.Devices
{
    public class BaseDevice
    {
        public int ScreenModeDelta { get; set; } = 31;
        public IntPtr IntPtr { get; set; }

        public string Action { get; set; }

        public string Job { get; set; }

        public string Pilot { get; set; }
        public string LogFileName { get; set; }
        public bool IsDebug { get; set; }
        public IJobMetrics Metrics { get; set; }
        public IDeviceEvents Events { get; set; }

        public IKeyboardInput Keyboard { get; set; }

        public IMouseInput Mouse { get; set; }

        public Shortcuts Shortcuts { get; set; }

        public ScreenZones Zones { get; set; }

        public IPatterns PatternFactory { get; set; }

        public IImageSearchResult FindObjectInScreen(string entity)
        {
            var result = ImageDetect.SearchImage(PatternFactory.GetPatterns(entity), this as IDevice);

            if(IsDebug)
                (this as IDevice)?.Logger($"[FindObjectInScreen] Finished. Pattern '{entity}'. Is found '{result.IsFound}'. Search time is {result.ExecuteTimeInMilliseconds} ms.");

            return result;
        }

        public IImageSearchResult FindObjectInScreen(string entity, double tolerance)
        {
            var result = ImageDetect.SearchImage(PatternFactory.GetPatterns(entity), this as IDevice);

            if (IsDebug)
                (this as IDevice)?.Logger($"[FindObjectInScreen] Finished. Pattern '{entity}'. Is found '{result.IsFound}'. Search time is {result.ExecuteTimeInMilliseconds} ms.");

            return result;
        }

        public IImageSearchResult FindObjectInScreen(string entity, Rectangle zone)
        {
            var result = ImageDetect.SearchImage(PatternFactory.GetPatterns(entity), this as IDevice, zone);

            if (IsDebug)
                (this as IDevice)?.Logger($"[FindObjectInScreen] Finished. Pattern '{entity}'. Is found '{result.IsFound}'. Search time is {result.ExecuteTimeInMilliseconds} ms.");

            return result;
        }

        public void Report(string file)
        {
            try
            {
                var pilotName = (this as IDevice)?.Pilot.Replace(" ", "_");
                (this as IDevice)?.GetScreen().Save($"Reports/[{pilotName}]{file}_" + DateTime.Now.Ticks + @".png", ImageFormat.Png);
            }
            catch (Exception ex)
            {
                (this as IDevice)?.Logger($"Critical error on save report. Exception message is '{ex.Message}'");
            }
        }

        public void Report(string file, string message)
        {
            (this as IDevice)?.Logger(message);
            Report(file);
        }
    }
}